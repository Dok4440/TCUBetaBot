#!/bin/sh
echo ""
echo "Running TCU with auto restart and updating to latest build!"
root=$(pwd)
youtube-dl -U

sleep 5s

while :
do cd "$root/TCU"
dotnet restore
dotnet build -c Release
cd "$root/TCU/src/NadekoBot"
dotnet run -c Release
youtube-dl -U
cd "$root"
wget -N https://raw.githubusercontent.com/Dok4440/TCURawScripts/Latest_version/TCU_installer_latest.sh
bash "$root/TCU_installer_latest.sh"
sleep 5s
done

echo ""
echo "That didn't work? Please report to Dok#4440 on Discord."
sleep 3s

cd "$root"
bash "$root/linuxAIO.sh"
echo "Done"

rm "$root/TCUARU_Latest.sh"
exit 0

#done
