#!/bin/sh
echo ""
echo "Running TCU with auto restart normally! (without updating)"
root=$(pwd)
youtube-dl -U

sleep 5s
cd "$root/TCU"
dotnet restore && dotnet build -c Release

while :; do cd "$root/TCU/src/NadekoBot" && dotnet run -c Release && youtube-dl -U; sleep 5s; done
echo ""
echo "That didn't work? Please report in TCU's support server."
sleep 3s

cd "$root"
bash "$root/linuxAIO.sh"
echo "Done"

exit 0

#done
