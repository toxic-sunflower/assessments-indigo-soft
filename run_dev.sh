#!/bin/bash
set -euo pipefail

./run_dev_db.sh run

dotnet restore
dotnet build

cd ./source/AccessTracker

dotnet run