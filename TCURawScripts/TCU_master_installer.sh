#!/bin/sh

echo "Welcome to TCU."
root=$(pwd)
echo ""
choice=9
	echo "1. Download TCU"
	echo "2. Run TCU (Normally)"
	echo "3. Run TCU with Auto Restart in this session"
	echo "4. Auto-Install Prerequisites (For Linux)"
	echo "5. Set up credentials.json (If you have downloaded TCU already)"
	echo "6. Auto-Install pm2"
	echo "7. Start TCU in pm2 (Complete option 6 first!)"
	echo "8. Exit"
	echo -n "Choose [1] to Download, [2 or 3] to Run, [6 and 7] for pm2 setup/startup or [8] to Exit."
while [ $choice -eq 9 ]; do
read choice
if [ $choice -eq 1 ] ; then

	echo ""
	echo "Downloading TCU, please wait."
	sleep 5s

	wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCU_installer_latest.sh && bash "$root/TCU_installer_latest.sh"

	echo ""
	bash "$root/linuxAIO.sh"
else
		if [ $choice -eq 2 ] ; then
			echo ""
			echo "Running TCU Normally, if you are running this to check Tea Cup Utils, use .die command on discord to stop the bot."

			wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCU_run.sh && bash "$root/TCU_run.sh"

			echo ""
			echo "Welcome back to NadekoBot."
			sleep 2s
			bash "$root/linuxAIO.sh"
		else
			if [ $choice -eq 3 ] ; then
				echo ""
				echo "Running Nadeko with Auto Restart you will have to close the session to stop the auto restart."
				sleep 5s

				wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCUAutoRestartAndUpdate.sh && bash "$root/TCUAutoRestartAndUpdate.sh"

				echo ""
				echo "That did not work?"
				sleep 2s
				bash "$root/linuxAIO.sh"
			else
				if [ $choice -eq 4 ] ; then
					echo ""
					echo "Getting the Auto-Installer for Debian/Ubuntu"

					wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCUautoinstaller.sh && bash "$root/TCUautoinstaller.sh"

					echo ""
					echo "Welcome back..."
					sleep 2s
					bash "$root/linuxAIO.sh"
				else
					if [ $choice -eq 5 ] ; then
						echo ""
						echo
echo -e "Let's begin creating a new credentials.json file if you are about to run the TCU for the first time. \n \nPlease read JSON Explanations in the guide... \n \nPress [Y] when you are ready to continue or [N] to exit."
while true; do
    read -p "[y/n]: " yn
    case $yn in
        [Yy]* ) clear; break;;
        [Nn]* ) echo Exiting...; exit;;
        * ) echo "Couldn't get that please type [y] for Yes or [n] for No.";;
    esac
done
clear
cd "$root/TCU/src/NadekoBot"
mv credentials.json credentials.json.old

echo Please enter your bot client ID:
echo ""
read clientid
echo ""
echo Alright saved \'$clientid\' as your client ID.
echo ""
echo "----------"
echo ""

echo Please enter your bot token \(It is not bot secret, it should be ~59 characters long.\):
echo ""
read token
echo ""
echo Alright saved \'$token\' as your bot\'s token.
echo ""
echo "----------"
echo ""

echo Please enter your own ID \(Refer to the guide, it will be bot\'s owner ID.\):
echo ""
read ownerid
echo ""
echo Alright saved \'$ownerid\' as owner\'s ID.
echo ""
echo "----------"
echo ""

echo Please enter Google API key \(Refer to the guide.\):
echo ""
read googleapi
echo ""
echo Alright saved \'$googleapi\' as your bot\'s Google API Key.
echo ""
echo "----------"
echo ""

echo -e "Please enter Mashape Key or Just Press [Enter Key] to skip. (optional) \nRefer to the JSON Explanations guide:"
echo ""
read mashapekey
echo ""
echo Alright saved \'$mashapekey\' as your bot\'s Mashape Key.
echo ""
echo "----------"
echo ""

echo -e "Please enter OSU API Key or Just Press [Enter Key] to skip. (optional) \nRefer to the JSON Explanations guide:"
echo ""
read osu
echo ""
echo Alright saved \'$osu\' as your bot\'s OSU API Key.
echo ""
echo "----------"
echo ""

echo -e "Please enter Cleverbot API Key or Just Press [Enter Key] to skip. (optional) \nRefer to the JSON Explanations guide:"
echo ""
read cleverbot
echo ""
echo Alright saved \'$cleverbot\' as your bot\'s Cleverbot API Key.
echo ""
echo "----------"
echo ""

echo -e "Please enter Twitch Client ID or Just Press [Enter Key] to skip. (optional) \nRefer to the JSON Explanations guide:"
echo ""
read twitchcid
echo ""
echo Alright saved \'$twitchcid\' as your bot\'s Twitch Client ID.
echo ""
echo "----------"
echo ""

echo -e "Please enter Location IQ Api Key or Just Press [Enter Key] to skip. (optional) \nRefer to the JSON Explanations guide:"
echo ""
read locationiqapi
echo ""
echo Alright saved \'$locationiqapi\' as your bot\'s Location IQ API Key.
echo ""
echo "----------"
echo ""

echo -e "Please enter Timezone DB Api Key or Just Press [Enter Key] to skip. (optional) \nRefer to the JSON Explanations guide:"
echo ""
read timedbapi
echo ""
echo Alright saved \'$timedbapi\' as your bot\'s Timezone DB API Key.
echo ""
echo "----------"
echo ""

echo "{
  \"ClientId\": $clientid,
  \"Token\": \"$token\",
  \"OwnerIds\": [
    $ownerid
  ],
  \"GoogleApiKey\": \"$googleapi\",
  \"MashapeKey\": \"$mashapekey\",
  \"OsuApiKey\": \"$osu\",
  \"CleverbotApiKey\": \"$cleverbot\",
  \"TwitchClientId\": \"$twitchcid\",
  \"LocationIqApiKey\": \"$locationiqapi\",
  \"TimezoneDbApiKey\": \"$timedbapi\",
  \"Db\": null,
  \"TotalShards\": 1
}" | cat - >> credentials.json
echo Credentials setup completed.
sleep 5
clear
cd "$root"
bash "$root/linuxAIO.sh"
					else
						if [ $choice -eq 6 ] ; then
						echo ""
						echo "Starting the setup for pm2 with TCU. This only has to be done once."

						wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCUpm2setup.sh && bash "$root/TCUpm2setup.sh"

						echo ""
						echo "Welcome back..."
						sleep 2s
						bash "$root/linuxAIO.sh"
						else
							if [ $choice -eq 7 ] ; then
							echo ""
							echo "Getting the pm2 startup options for TCU.."

							wget -N https://gitlab.com/Dok4440/TCUBetaBot/-/raw/1.0/TCURawScripts/TCUpm2start.sh && bash "$root/TCUpm2start.sh"

							echo ""
							sleep 2s
							bash "$root/linuxAIO.sh"
							else
								if [ $choice -eq 8 ] ; then
									echo ""
									echo "Exiting..."
									cd "$root"
									exit 0
								else
									clear
									echo "1. Download TCU"
									echo "2. Run TCU (Normally)"
									echo "3. Run TCU with Auto Restart in this session"
									echo "4. Auto-Install Prerequisites (For Linux)"
									echo "5. Set up credentials.json (If you have downloaded TCU already)"
									echo "6. Auto-Install pm2"
									echo "7. Start TCU in pm2 (Complete option 6 first!)"
									echo "8. Exit"
									echo -n "Choose [1] to Download, [2 or 3] to Run, [6 and 7] for pm2 setup/startup or [8] to Exit."
									choice=9
								fi
							fi
						fi
					fi
				fi
			fi
		fi
	fi
done

cd "$root"
exit 0
