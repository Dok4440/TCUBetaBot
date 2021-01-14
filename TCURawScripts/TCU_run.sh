#!/bin/sh
echo ""
echo "TCU Latest_version"
echo "Dok#4440 was here."
root=$(pwd)
youtube-dl -U

if hash dotnet 2>/dev/null
then
	echo "Dotnet installed."
else
	clear
	echo "Git is not installed."
	echo ""
	echo 'Press CTRL + C to exit.'
	echo "Program will automatically exit after 60 seconds."
	sleep 60s
	exit 0
fi
cd "$root/TCU"
dotnet restore
dotnet build -c Release
cd "$root/TCU/src/NadekoBot"
echo "Running TCU. Please wait."
dotnet run -c Release
echo "Done"

cd "$root"
rm "$root/TCU_run.sh"
exit 0

#done
