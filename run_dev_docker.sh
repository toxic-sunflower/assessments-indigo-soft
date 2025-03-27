#!/bin/bash
set -euo pipefail

# === Переменные по умолчанию ===
export DB_USER=${DB_USER:-tracker}
export DB_PASSWORD=${DB_PASSWORD:-secret}
export DB_NAME=${DB_NAME:-accesstracker}

COMPOSE_FILE="./deploy/docker-compose.yml"

MODE=${1:-}

# === Очистка volumes и контейнеров ===
if [[ "$MODE" == "--clean" ]]; then
  echo "🧹 Остановка и удаление контейнеров + volume (pgdata)..."
  docker compose -f "$COMPOSE_FILE" down --volumes
  echo "✅ Очистка завершена."
  exit 0
fi

# === Горячая перезагрузка API ===
if [[ "$MODE" == "--reload" ]]; then
  echo "♻️  Запуск с hot reload (только API), остальные в фоне..."
  docker compose -f "$COMPOSE_FILE" up --build -d postgres

  pushd source/Assessments.IndigoSoft.AccessTracker
  dotnet watch run
  popd
  exit 0
fi

# === Полный запуск всех сервисов ===
echo "🚀 Запуск AccessTracker (API + БД)..."
docker compose -f "$COMPOSE_FILE" up --build
