#!/usr/bin/env bash
set -e

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PROJECT="$SCRIPT_DIR/src/ConwayGame.Desktop/ConwayGame.Desktop.csproj"

DIST_DIR="$SCRIPT_DIR/Executable files"
mkdir -p "$DIST_DIR"

RUNTIMES=("win-x64" "osx-arm64")

for rid in "${RUNTIMES[@]}"; do
  echo "==============="
  echo "=== Publishing for $rid ==="
  
  TMP_DIR="$SCRIPT_DIR/tmp/$rid"
  rm -rf "$TMP_DIR"
  mkdir -p "$TMP_DIR"

  dotnet publish "$PROJECT" \
    -c Release \
    -r $rid \
    --self-contained true \
    /p:PublishSingleFile=true \
    /p:IncludeNativeLibrariesForSelfExtract=true \
    -o "$TMP_DIR"

  if [[ "$rid" == "win-x64" ]]; then
    mkdir -p "$DIST_DIR/Windows"
    cp "$TMP_DIR/ConwayGame.Desktop.exe" "$DIST_DIR/Windows/"
  
  elif [[ "$rid" == "osx-arm64" ]]; then
    APP_NAME="ConwayGame.app"
    APP_DIR="$DIST_DIR/MacOS Silicon/$APP_NAME"
    
    mkdir -p "$APP_DIR/Contents/MacOS"
    mkdir -p "$APP_DIR/Contents/Resources"

    # Копируем в наглую всё содержимое публикации
    cp -R "$TMP_DIR"/* "$APP_DIR/Contents/MacOS/"

    mv "$APP_DIR/Contents/MacOS/ConwayGame.Desktop" "$APP_DIR/Contents/MacOS/ConwayGame"


    # Создаём минимальный Info.plist
    cat > "$APP_DIR/Contents/Info.plist" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
  <key>CFBundleName</key>
  <string>Conway Game</string>
  <key>CFBundleExecutable</key>
  <string>ConwayGame</string>
  <key>CFBundleIdentifier</key>
  <string>com.yourcompany.conwaygame</string>
  <key>CFBundleVersion</key>
  <string>1.0</string>
  <key>CFBundlePackageType</key>
  <string>APPL</string>
</dict>
</plist>
EOF
  fi
done

echo "==============="
echo "==============="
echo "✅ Все сборки готовы в папке $DIST_DIR"
