#!/bin/sh
clear
echo "TCU Installer started."

if hash git 1>/dev/null 2>&1
then
    echo ""
    echo "Git Installed."
else
    clear
    echo "Git is not installed."
    echo ""
    echo 'Press CTRL + C to exit.'
    echo "Program will automatically exit after 60 seconds."
    sleep 60s
    exit 0
fi


if hash dotnet 1>/dev/null 2>&1
then
    echo ""
    echo "Dotnet installed."
else
    echo ""
    echo "Dotnet is not installed."
    echo ""
    echo 'Press CTRL + C to exit.'
    echo "Program will automatically exit after 60 seconds."
    sleep 60s
    exit 0
fi

root=$(pwd)
tempdir=TCU_Temp

rm -r "$tempdir" 1>/dev/null 2>&1
mkdir "$tempdir"
cd "$tempdir" || return

clear
echo "Downloading TCU, please wait."
echo ""
echo "This process usually takes about 15 seconds."
sleep 5s
git clone -b develop --recursive --depth 1 https://gitlab.com/Dok4440/TCUBetaBot
clear
echo "TCU downloaded."

echo ""
echo "Downloading TCU dependencies"
echo ""
echo "This process usually takes about 10 seconds."
sleep 5s
cd "$root/$tempdir/TCUBetaBot" || return;
dotnet restore
clear
echo "TCU Dependecies are downloaded"

echo ""
echo "Building TCU for you!"
echo ""
echo "This process usually takes about 10 seconds."
sleep 5s
dotnet build --configuration Release
clear
echo "Build done."
sleep 3s

cd "$root" || return

if [ ! -d TCU ]
then
    mv "$tempdir"/TCUBetaBot TCU

else
        echo ""
        echo 'Renaming "TCU" to "TCU_Old"'
        rm -rf TCU_Old 1>/dev/null 2>&1
        echo ""
        echo 'Removed already existing "TCU_Old" folder (only applies when you updated TCU before.)'
        sleep 2s
        mv -fT TCU TCU_Old 1>/dev/null 2>&1
        mv "$tempdir"/TCUBetaBot TCU
        echo ""
        echo "Renamed. Copying credentials.json to new version"
        sleep 2s
        cp -f "$root/TCU_Old/src/NadekoBot/credentials.json" "$root/TCU/src/NadekoBot/credentials.json" 1>/dev/null 2>&1
        echo ""
        echo "Credentials.json copied to the new version"
        echo "Copying database to the new version."
        sleep 2s
        cp -RT "$root/TCU_Old/src/NadekoBot/bin/" "$root/TCU/src/NadekoBot/bin/" 1>/dev/null 2>&1
        cp -RT "$root/TCU/src/NadekoBot/bin/Release/netcoreapp1.0/data/NadekoBot.db" "$root/TCU/src/NadekoBot/bin/Release/netcoreapp2.1/data/NadekoBot.db" 1>/dev/null 2>&1
	      cp -RT "$root/TCU/src/NadekoBot/bin/Release/netcoreapp1.1/data/NadekoBot.db" "$root/TCU/src/NadekoBot/bin/Release/netcoreapp2.1/data/NadekoBot.db" 1>/dev/null 2>&1
        cp -RT "$root/TCU/src/NadekoBot/bin/Release/netcoreapp2.0/data/NadekoBot.db" "$root/TCU/src/NadekoBot/bin/Release/netcoreapp2.1/data/NadekoBot.db" 1>/dev/null 2>&1
        mv -f "$root/TCU/src/NadekoBot/bin/Release/netcoreapp1.0/data/NadekoBot.db" "$root/TCU/src/NadekoBot/bin/Release/netcoreapp1.0/data/TCU_Old.db" 1>/dev/null 2>&1
        mv -f "$root/TCU/src/NadekoBot/bin/Release/netcoreapp1.1/data/NadekoBot.db" "$root/TCU/src/NadekoBot/bin/Release/netcoreapp1.1/data/TCU_Old.db" 1>/dev/null 2>&1
        mv -f "$root/TCU/src/NadekoBot/bin/Release/netcoreapp2.0/data/NadekoBot.db" "$root/TCU/src/NadekoBot/bin/Release/netcoreapp2.0/data/TCU_Old.db" 1>/dev/null 2>&1
        echo ""
        echo "Database copied to the new version"
        echo "Copying other data to the new version"
        cp -RT "$root/TCU_Old/src/NadekoBot/data/" "$root/TCU/src/NadekoBot/data/" # 1>/dev/null 2>&1
        echo ""
        echo "Other data copied to the new version"
        sleep 3s
fi

rm -r "$tempdir"
clear
echo "Installation Complete."
echo "TCU has been installed successfully!"
echo ""
echo "Thanks for using TCU <3"
cd "$root" || return
rm "$root/TCU_installer_latest.sh"
sleep 5s

cd "$root" || return
exit 0
