#!/bin/sh
echo ""
echo "TCU's pm2 Startup. Please ensure you have installed pm2/NodeJS/npm with the installer script first! Running TCU with pm2 means that pm2 runs TCU in the background of your machine and auto-restart even after reboot. If you are running the bot already, you can close the session you are currently using and start TCU with this method."

echo ""
echo ""
root=$(pwd)


choice=5
	echo "1. Run in pm2 with Auto Restart normally without Auto Update."
	echo "2. Run in pm2 with Auto Restart and Auto Update."
	echo "3. Run TCU in pm2 normally without Auto Restart or Auto Update."
	echo "4. Exit"
	echo -n "Choose [1] to Run TCU in pm2 with auto restart on "die" command without updating itself, [2] to Run in pm2 with Auto Updating on restart after using "die" command, and [3] to run without any auto-restarts or auto-updates."
while [ $choice -eq 5 ]; do
read choice
if [ $choice -eq 1 ] ; then
	echo ""

	wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCUARN.sh
	cd "$root"
	echo "Starting Nadeko in pm2 with auto-restart and no auto-update..."
	sudo pm2 start "$root/TCUARN.sh" --interpreter=bash --name=TCU_DO_NOT_RENAME_THIS
	sudo pm2 startup
	sudo pm2 save
	echo ""
	echo "If you did everything correctly, pm2 should have started up TCU! Please use sudo pm2 info TeaCupUtilities to check. You can view pm2 logs with sudo pm2 logs TeaCupUtilities"
else
	if [ $choice -eq 2 ] ; then
		echo ""

		wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCUARU_Latest.sh

		cd "$root"
		echo "Starting Nadeko in pm2 with auto-restart and auto-update..."
		sudo pm2 start "$root/TCUARU_Latest.sh" --interpreter=bash --name=TCU_DO_NOT_RENAME_THIS
		sudo pm2 startup
		sudo pm2 save
		echo ""
		echo "If you did everything correctly, pm2 should have started up TCU! Please use sudo pm2 info TeaCupUtilities to check. You can view pm2 logs with sudo pm2 logs TeaCupUtilities"
	else
		if [ $choice -eq 3 ] ; then
		echo ""

		wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCU_run.sh

		cd "$root"
		echo "Starting Nadeko in pm2 normally without any auto update or restart.."
		sudo pm2 start "$root/TCU_run.sh" --interpreter=bash --name=TCU_DO_NOT_RENAME_THIS
		sudo pm2 startup
		sudo pm2 save
		echo ""
		echo "If you did everything correctly, pm2 should have started up TCU! Please use sudo pm2 info TeaCupUtilities to check. You can view pm2 logs with sudo pm2 logs TeaCupUtilities"
		else
			if [ $choice -eq 4 ] ; then
				echo ""
				echo "Exiting..."
				cd "$root"
				exit 0
			else
				clear
				echo "1. Run in pm2 with Auto Restart normally without Auto Update."
	            echo "2. Run in pm2 with Auto Restart and Auto Update."
	            echo "3. Run TCU in pm2 normally without Auto Restart or Auto Update."
	            echo "4. Exit"
	            echo -n "Choose [1] to Run TCU in pm2 with auto restart on "die" command without updating itself, [2] to Run in pm2 with Auto Updating on restart after using "die" command, and [3] to run without any auto-restarts or auto-updates."
				choice=5
			fi
		fi
	fi
fi
done

cd "$root"
rm "$root/TCUpm2start.sh"
exit 0
