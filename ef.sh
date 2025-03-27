#!/bin/bash
set -euo pipefail

show_usage() {
  echo "Использование:"
  echo "  $0 mg-add <MigrationName> [SolutionDir] [--verbose]"
  echo "  $0 mg-remove [SolutionDir] [--verbose]"
  echo "  $0 mg-list [SolutionDir] [--verbose]"
  echo "  $0 db-update [SolutionDir] [--verbose]"
  echo "  $0 db-revert [MigrationName|--full] [SolutionDir] [--verbose]"
  exit 1
}

COMMAND=${1:-}
shift || true

VERBOSE=false
POSITIONAL_ARGS=()

for arg in "$@"; do
  case "$arg" in
    --verbose) VERBOSE=true ;;
    *) POSITIONAL_ARGS+=("$arg") ;;
  esac
done

MIGRATION_NAME=""
SOLUTION_DIR="."

case "$COMMAND" in
  mg-add)
    [[ ${#POSITIONAL_ARGS[@]} -lt 1 ]] && show_usage
    MIGRATION_NAME=${POSITIONAL_ARGS[0]}
    [[ ${#POSITIONAL_ARGS[@]} -ge 2 ]] && SOLUTION_DIR=${POSITIONAL_ARGS[1]}
    ;;
  mg-remove|mg-list|db-update)
    [[ ${#POSITIONAL_ARGS[@]} -ge 1 ]] && SOLUTION_DIR=${POSITIONAL_ARGS[0]}
    ;;
  db-revert)
    [[ ${#POSITIONAL_ARGS[@]} -ge 1 ]] && {
      if [[ "${POSITIONAL_ARGS[0]}" != "--full" ]]; then
        MIGRATION_NAME=${POSITIONAL_ARGS[0]}
      fi
    }
    [[ ${#POSITIONAL_ARGS[@]} -ge 2 ]] && SOLUTION_DIR=${POSITIONAL_ARGS[1]}
    ;;
  *) show_usage ;;
esac

EF_VERBOSE_FLAG=""
$VERBOSE && EF_VERBOSE_FLAG="--verbose"

# Пути
DATA_PROJECT="$SOLUTION_DIR/source/AccessTracker.Data"
STARTUP_PROJECT="$SOLUTION_DIR/source/AccessTracker"
MIGRATIONS_OUTPUT_DIR="Migrations"

pushd "$SOLUTION_DIR" > /dev/null || {
  echo "❌ Не удалось перейти в каталог $SOLUTION_DIR"
  exit 1
}

case "$COMMAND" in
  mg-add)
    echo "📦 Добавление миграции '$MIGRATION_NAME'..."
    dotnet ef migrations add "$MIGRATION_NAME" \
      --project "$DATA_PROJECT" \
      --startup-project "$STARTUP_PROJECT" \
      --output-dir "$MIGRATIONS_OUTPUT_DIR" \
      $EF_VERBOSE_FLAG
    echo "✅ Миграция создана в $MIGRATIONS_OUTPUT_DIR."
    ;;

  mg-remove)
    echo "❌ Удаление последней миграции..."
    dotnet ef migrations remove \
      --project "$DATA_PROJECT" \
      --startup-project "$STARTUP_PROJECT" \
      $EF_VERBOSE_FLAG
    echo "✅ Миграция удалена."
    ;;

  mg-list)
    echo "📄 Список миграций:"
    dotnet ef migrations list \
      --project "$DATA_PROJECT" \
      --startup-project "$STARTUP_PROJECT" \
      $EF_VERBOSE_FLAG
    ;;

  db-update)
    echo "⏳ Применение миграций..."
    dotnet ef database update \
      --project "$DATA_PROJECT" \
      --startup-project "$STARTUP_PROJECT" \
      $EF_VERBOSE_FLAG
    echo "✅ База данных обновлена."
    ;;

  db-revert)
    TARGET="$MIGRATION_NAME"

    if [[ "$TARGET" == "--full" ]]; then
      TARGET="0"
    elif [[ -z "$TARGET" ]]; then
      TARGET=$(dotnet ef migrations list \
        --project "$DATA_PROJECT" \
        --startup-project "$STARTUP_PROJECT" \
        $EF_VERBOSE_FLAG | tail -n 2 | head -n 1 | awk '{print $1}')
    fi

    if [[ -z "$TARGET" ]]; then
      echo "❌ Не удалось определить миграцию для отката."
      popd > /dev/null
      exit 1
    fi

    echo "↩️ Откат до миграции: $TARGET"
    dotnet ef database update "$TARGET" \
      --project "$DATA_PROJECT" \
      --startup-project "$STARTUP_PROJECT" \
      $EF_VERBOSE_FLAG
    echo "✅ Откат выполнен."
    ;;
esac

popd > /dev/null
