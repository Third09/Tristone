#!/usr/bin/env bash
# =====================================================================
# Tri-Stone database setup for the office server (Linux), like database/setup-database.ps1 on Windows.
# Creates construction_db if needed, then runs every numbered script in ../database (001_..., 002_...)
# that hasn't been run yet, in order, each in one transaction, and records it in schema_versions.
# Safe to run again at any time: already-applied scripts are skipped.
#
# Usage (on the server, from this folder):   sudo ./setup-database.sh
# PSQL can be set to run another psql (used for testing on Windows), e.g. PSQL="psql -h localhost -U postgres".
# =====================================================================
set -euo pipefail

HERE="$(cd "$(dirname "$0")" && pwd)"
SCRIPTS="${SCRIPTS:-$HERE/../database}"
DATABASE="${DATABASE:-construction_db}"
PSQL="${PSQL:-sudo -u postgres psql}"
export PGOPTIONS="-c client_min_messages=warning"   # hide "already exists, skipping" notices

run() {   # run <database> <psql args...>
    local db="$1"; shift
    $PSQL -d "$db" -v ON_ERROR_STOP=1 -q -X "$@"
}

echo "Database: $DATABASE"

# 1. Create the database if it doesn't exist.
if [ "$(run postgres -t -A -c "SELECT 1 FROM pg_database WHERE datname = '$DATABASE'")" != "1" ]; then
    echo "Creating database $DATABASE..."
    run postgres -c "CREATE DATABASE $DATABASE"
fi

# 2. Table that remembers which scripts already ran.
run "$DATABASE" -c "CREATE TABLE IF NOT EXISTS schema_versions (
    version     INT          PRIMARY KEY,
    script_name VARCHAR(200) NOT NULL,
    applied_at  TIMESTAMP    NOT NULL DEFAULT NOW());"

applied=" $(run "$DATABASE" -t -A -c "SELECT version FROM schema_versions" | tr -d '\r' | tr '\n' ' ') "

# 3. Run each new numbered script inside one transaction, then record it.
for file in "$SCRIPTS"/[0-9][0-9][0-9]_*.sql; do
    name="$(basename "$file")"
    version=$((10#${name:0:3}))
    if [[ "$applied" == *" $version "* ]]; then
        echo "  skip  $name (already applied)"
        continue
    fi
    echo "  run   $name"
    run "$DATABASE" -1 -f - < "$file"   # through stdin: the postgres user may not be allowed into your home folder
    run "$DATABASE" -c "INSERT INTO schema_versions (version, script_name) VALUES ($version, '$name')"
done

echo "Database is up to date."
