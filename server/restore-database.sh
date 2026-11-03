#!/usr/bin/env bash
# =====================================================================
# Tri-Stone: put a database copy (.dump) onto the office server.
#   - Moving in: the home PC's real data (made there with server\export-database.cmd).
#   - Repairs:   a nightly backup, e.g. /srv/tristone/backups/2026-11-30/construction_db.dump
#
#     sudo ./restore-database.sh /path/to/construction_db.dump
#
# REPLACES everything in construction_db on this server (asks first, and saves a copy of the
# current database next to the backups before replacing it). Then runs any newer numbered
# scripts and gives tristone_app its rights again. The app login's password is kept.
# Pictures and scans are separate: copy them into \\SERVER\TriStoneFiles (or untar files.tar.gz).
# =====================================================================
set -euo pipefail

if [ "$(id -u)" -ne 0 ]; then echo "Run it with sudo."; exit 1; fi
DUMP="${1:-}"
if [ -z "$DUMP" ] || [ ! -f "$DUMP" ]; then echo "Usage: sudo ./restore-database.sh /path/to/construction_db.dump"; exit 1; fi
HERE="$(cd "$(dirname "$0")" && pwd)"
DB=construction_db

read -r -p "This REPLACES the whole Tri-Stone database on this server with $(basename "$DUMP"). Type YES to continue: " ok
[ "$ok" = "YES" ] || { echo "Stopped. Nothing changed."; exit 1; }

if sudo -u postgres psql -tAc "SELECT 1 FROM pg_database WHERE datname = '$DB'" | grep -q 1; then
    SAFE="/srv/tristone/backups/before-restore-$(date +%F-%H%M).dump"
    install -d -m 700 /srv/tristone/backups
    sudo -u postgres pg_dump -Fc "$DB" > "$SAFE"
    echo "Current database saved to $SAFE"
    # Sign everyone out of the database before replacing it.
    sudo -u postgres psql -q -c "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '$DB' AND pid <> pg_backend_pid()" >/dev/null
    sudo -u postgres dropdb "$DB"
fi
sudo -u postgres createdb "$DB"
# --no-owner --no-acl: everything belongs to postgres here; the app login gets its rights again below (022).
sudo -u postgres pg_restore --no-owner --no-acl --role=postgres -d "$DB" < "$DUMP"

# Newer scripts (the copy may be older than this server's code), then the app login's rights again.
"$HERE/setup-database.sh"
sudo -u postgres psql -q -d "$DB" -f - < "$HERE/../database/022_app_role.sql"

# The copy carries the home PC's files folder (C:\TriStoneFiles): point it at this server's share again.
ADDR="$(hostname -I | awk '{print $1}')"
read -r -p "Server address the office PCs use [$ADDR]: " a; ADDR="${a:-$ADDR}"
sudo -u postgres psql -q -d "$DB" -c "UPDATE app_settings SET value = '\\\\$ADDR\\TriStoneFiles' WHERE key = 'file_storage_path'"
echo "Done. Office PCs can sign in again."
