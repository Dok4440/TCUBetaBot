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
wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/develop/TCURawScripts/TCU_installer_latest.sh
bash "$root/TCU_installer_latest.sh"
sleep 5s
done

echo ""
echo "That didn't work? Please report to TCU's support server."
sleep 3s

cd "$root"
bash "$root/linuxAIO.sh"
echo "Done"

rm "$root/TCUARU_Latest.sh"
exit 0

#done
