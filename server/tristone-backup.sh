#!/usr/bin/env bash
# =====================================================================
# Tri-Stone nightly backup (installed as /usr/local/bin/tristone-backup by install-server.sh; cron 23:30).
# Makes /srv/tristone/backups/YYYY-MM-DD/ with:
#   construction_db.dump   the whole database (pg_dump custom format; restore with server/restore-database.sh)
#   files.tar.gz           the scans and pictures folder
# Keeps KEEP_DAYS days, and copies each backup to COPY_TO (e.g. a USB drive) when set
# (both in /etc/tristone-backup.conf). Run it by hand any time:  sudo tristone-backup
# =====================================================================
set -euo pipefail

COPY_TO=""
KEEP_DAYS=14
[ -f /etc/tristone-backup.conf ] && . /etc/tristone-backup.conf

BACKUPS=/srv/tristone/backups
DAY="$(date +%F)"
OUT="$BACKUPS/$DAY"
install -d -m 700 "$OUT"

echo "$(date '+%F %T') backup start"
sudo -u postgres pg_dump -Fc construction_db > "$OUT/construction_db.dump"
tar -czf "$OUT/files.tar.gz" -C /srv/tristone files
echo "$(date '+%F %T') made $OUT ($(du -sh "$OUT" | cut -f1))"

# Old backups out.
find "$BACKUPS" -mindepth 1 -maxdepth 1 -type d -mtime +"$KEEP_DAYS" -exec rm -rf {} +

# A second copy somewhere else (a backup on the same laptop is lost with the laptop).
if [ -n "$COPY_TO" ]; then
    if [ -d "$COPY_TO" ]; then
        install -d "$COPY_TO/tristone-backups"
        cp -r "$OUT" "$COPY_TO/tristone-backups/"
        find "$COPY_TO/tristone-backups" -mindepth 1 -maxdepth 1 -type d -mtime +"$KEEP_DAYS" -exec rm -rf {} +
        echo "$(date '+%F %T') copied to $COPY_TO"
    else
        echo "$(date '+%F %T') WARNING: $COPY_TO not found (USB drive not plugged in?). Only the copy on this laptop was made."
    fi
fi
echo "$(date '+%F %T') backup done"
