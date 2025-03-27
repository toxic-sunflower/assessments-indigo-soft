#!/bin/bash
set -euo pipefail

pushd "$PWD"

cd ./source/AccessTracker.Cli

dotnet restore
dotnet build
dotnet run "$1" "$2"

popd 