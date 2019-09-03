#! /bin/bash
cd GRader/GRader
msbuild /p:Configuration=Release GRader.csproj
sudo cp bin/Release/GRader.exe /usr/bin
sudo chmod +x /usr/bin/GRader.exe
sudo service vpl-jail-system restart
cd ../..
