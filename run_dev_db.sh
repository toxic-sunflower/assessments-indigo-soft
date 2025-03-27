#!/bin/bash
set -euo pipefail

# === Аргументы ===
MODE=${1:-run}                 # run или clean
ENVIRONMENT=${2:-Development} # Среда (по умолчанию Development)

# === Переменные окружения ===
export DB_USER=${DB_USER:-tracker}
export DB_PASSWORD=${DB_PASSWORD:-secret}
export DB_NAME=${DB_NAME:-accesstracker}
export DB_PORT=${DB_PORT:-5433}
export ASPNETCORE_ENVIRONMENT="$ENVIRONMENT"

API_PROJECT="source/Assessments.IndigoSoft.AccessTracker"

if [[ "$MODE" == "clean" ]]; then
  echo "🧹 Остановка и удаление контейнера accesstracker-db и volume pgdata..."
  docker stop accesstracker-db >/dev/null 2>&1 || true
  docker rm accesstracker-db >/dev/null 2>&1 || true
  docker volume rm pgdata >/dev/null 2>&1 || true
  echo "✅ Всё очищено."
  exit 0
fi

# Проверка: не запущен ли уже контейнер
if docker ps --format '{{.Names}}' | grep -q '^accesstracker-db$'; then
  echo "ℹ️ Контейнер accesstracker-db уже запущен. Пропускаем создание."
else
  echo "🐘 Запуск PostgreSQL в режиме '$ASPNETCORE_ENVIRONMENT'..."
  docker run -d \
    --name accesstracker-db \
    -e POSTGRES_USER="${DB_USER}" \
    -e POSTGRES_PASSWORD="${DB_PASSWORD:-secret}" \
    -e POSTGRES_DB="${DB_NAME}" \
    -p "${DB_PORT}":5432 \
    -v pgdata:/var/lib/postgresql/data \
    postgres:15-alpine
fi

# === Информация о подключении ===
echo ""
echo "🌱 ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT"
echo "💡 Строка подключения для API:" 
echo "Host=localhost;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASSWORD"
echo ""
