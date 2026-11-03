#!/usr/bin/env bash
# =====================================================================
# Tri-Stone office server setup (Phase 6, owner decisions 2026-10-27)
# Run ONCE on the Linux Mint laptop that becomes the office server:
#     cd Tristone/server
#     sudo ./install-server.sh
#
# What it does:
#   1. Installs PostgreSQL 18 (official PostgreSQL apt repository).
#   2. Opens the database to the office network only (never the internet): pg_hba + ufw firewall.
#   3. Sets the postgres (admin) password, creates construction_db with every numbered script,
#      and turns on the app's own limited login, tristone_app (script 022), with its password.
#   4. Shares /srv/tristone/files as \\SERVER\TriStoneFiles (Samba) for scans and pictures,
#      for one Linux user "tristone", and points the app at it (app_settings.file_storage_path).
#   5. Nightly backups at 23:30 (database + files) into /srv/tristone/backups, kept 14 days,
#      also copied to a USB drive when one is set in /etc/tristone-backup.conf.
#
# Safe to run again (it keeps existing data). Passwords are asked for and never written to files.
# =====================================================================
set -euo pipefail

if [ "$(id -u)" -ne 0 ]; then echo "Run it with sudo:  sudo ./install-server.sh"; exit 1; fi
HERE="$(cd "$(dirname "$0")" && pwd)"
DB=construction_db
FILES=/srv/tristone/files
BACKUPS=/srv/tristone/backups

say() { echo; echo "== $*"; }

ask_password() {   # ask_password <what> -> prints the password (asked twice, at least 12 characters)
    local a b
    while true; do
        read -r -s -p "  $1 (12+ characters): " a; echo >&2
        read -r -s -p "  Type it again: " b; echo >&2
        if [ "$a" != "$b" ]; then echo "  They don't match. Try again." >&2; continue; fi
        if [ "${#a}" -lt 12 ]; then echo "  Too short. Use at least 12 characters." >&2; continue; fi
        if [[ "$a" == *"'"* ]]; then echo "  Please don't use the ' character." >&2; continue; fi
        printf '%s' "$a"; return
    done
}

# ---------- 0. Questions ----------
say "Tri-Stone office server setup"
SERVER_IP="$(hostname -I | awk '{print $1}')"
echo "  This laptop's address now: $SERVER_IP (the plan is 192.168.1.50, reserved in the router)."
read -r -p "  Office network [192.168.1.0/24]: " LAN; LAN="${LAN:-192.168.1.0/24}"
read -r -p "  Server address the office PCs will use [$SERVER_IP]: " ADDR; ADDR="${ADDR:-$SERVER_IP}"
echo
echo "  Three passwords (write them down on paper, keep them safe):"
PG_ADMIN_PW="$(ask_password "postgres admin password (only for setup and repairs)")"
APP_PW="$(ask_password "tristone_app password (typed once on each office PC)")"
FILES_PW="$(ask_password "files share password for user 'tristone' (typed once on each office PC)")"

# ---------- 1. PostgreSQL 18 ----------
say "Installing PostgreSQL 18"
apt-get update -q
apt-get install -y -q curl ca-certificates gnupg samba ufw
. /etc/os-release
CODENAME="${UBUNTU_CODENAME:-$VERSION_CODENAME}"   # Linux Mint is based on Ubuntu
install -d /usr/share/postgresql-common/pgdg
curl -fsSL https://www.postgresql.org/media/keys/ACCC4CF8.asc -o /usr/share/postgresql-common/pgdg/apt.postgresql.org.asc
echo "deb [signed-by=/usr/share/postgresql-common/pgdg/apt.postgresql.org.asc] https://apt.postgresql.org/pub/repos/apt $CODENAME-pgdg main" \
    > /etc/apt/sources.list.d/pgdg.list
apt-get update -q
apt-get install -y -q postgresql-18
PGCONF=/etc/postgresql/18/main

# ---------- 2. Office network only ----------
say "Opening the database to $LAN only"
cat > "$PGCONF/conf.d/tristone.conf" <<EOF
# Tri-Stone: accept connections from the office network (pg_hba.conf decides who may sign in).
listen_addresses = '*'
password_encryption = 'scram-sha-256'
EOF
if ! grep -q "Tri-Stone office network" "$PGCONF/pg_hba.conf"; then
    cat >> "$PGCONF/pg_hba.conf" <<EOF
# Tri-Stone office network (install-server.sh): the app login and the admin, from the office LAN only
host    $DB    tristone_app    $LAN    scram-sha-256
host    all    postgres        $LAN    scram-sha-256
EOF
fi
systemctl restart postgresql
sudo -u postgres psql -q -c "ALTER ROLE postgres PASSWORD '$PG_ADMIN_PW'"

# ---------- 3. Database + app login ----------
say "Creating / updating the database"
"$HERE/setup-database.sh"
sudo -u postgres psql -q -d "$DB" -c "ALTER ROLE tristone_app LOGIN PASSWORD '$APP_PW'"

# ---------- 4. Files share ----------
say "Sharing the files folder as \\\\$ADDR\\TriStoneFiles"
id tristone >/dev/null 2>&1 || useradd --system --no-create-home --shell /usr/sbin/nologin tristone
install -d -o tristone -g tristone -m 2770 "$FILES"
printf '%s\n%s\n' "$FILES_PW" "$FILES_PW" | smbpasswd -s -a tristone >/dev/null
smbpasswd -e tristone >/dev/null
if ! grep -q "^\[TriStoneFiles\]" /etc/samba/smb.conf; then
    cat >> /etc/samba/smb.conf <<EOF

# Tri-Stone scans and pictures (install-server.sh)
[TriStoneFiles]
   path = $FILES
   valid users = tristone
   read only = no
   force user = tristone
   create mask = 0660
   directory mask = 2770
   hosts allow = $LAN 127.0.0.1
EOF
fi
systemctl restart smbd
sudo -u postgres psql -q -d "$DB" -c \
    "UPDATE app_settings SET value = '\\\\$ADDR\\TriStoneFiles' WHERE key = 'file_storage_path'"

# ---------- 5. Firewall ----------
say "Firewall: only the office network may connect"
ufw default deny incoming >/dev/null
ufw default allow outgoing >/dev/null
ufw allow from "$LAN" to any port 22 proto tcp >/dev/null     # remote repairs (SSH)
ufw allow from "$LAN" to any port 5432 proto tcp >/dev/null   # database
ufw allow from "$LAN" to any app Samba >/dev/null             # files share
ufw --force enable >/dev/null

# ---------- 6. Nightly backups ----------
say "Nightly backups at 23:30 into $BACKUPS"
install -d -m 700 "$BACKUPS"
install -m 755 "$HERE/tristone-backup.sh" /usr/local/bin/tristone-backup
[ -f /etc/tristone-backup.conf ] || cat > /etc/tristone-backup.conf <<'EOF'
# Tri-Stone backups (read by /usr/local/bin/tristone-backup).
# Copy each backup to a second place too, e.g. a USB drive's folder. Empty = no copy.
COPY_TO=""
# Days of backups to keep.
KEEP_DAYS=14
EOF
cat > /etc/cron.d/tristone-backup <<'EOF'
# Tri-Stone nightly backup (install-server.sh)
30 23 * * * root /usr/local/bin/tristone-backup >> /var/log/tristone-backup.log 2>&1
EOF

say "Done"
cat <<EOF
  Database:  $ADDR : 5432, database $DB
  App login: tristone_app  (password: the one you just chose)
  Files:     \\\\$ADDR\\TriStoneFiles  (user tristone)
  Backups:   $BACKUPS every night at 23:30 (set COPY_TO in /etc/tristone-backup.conf for a USB copy)

  On each office PC:
    1. Double-click server\\connect-files.cmd (asks for the files password once).
    2. Open Tri-Stone > Database Settings: Server $ADDR, User tristone_app, the app password.
EOF
