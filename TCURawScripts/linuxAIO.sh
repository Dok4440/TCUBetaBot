#!/bin/sh

# If you wish to edit this file, make sure to read the Wiki, README.md and TCU's self-hosting system
# Some things you probably need to know before changing any of these `.sh` files;

# There's 2 possibilities. You fork this repo, make changes and make a pull-request to my repository.
# If I approve of the changes, I will accept the pull-request and your "version" of TCU-self-hosting will be uploaded to the public repo.

# You can copy the files, and edit them that way (I don't support/recommend this option.)
# I will never add "new" stuff, you changed/added in the sourcecode unless it's done from a fork, so for easy accessibily/no file loss, FORK.
# If you do copy the files, there's no way of running them in a Linux-distro from Windows, unless you paste all these files in the distro itself./
# If you copy these files, you will probably like the option of putting these files online, and pull links to run the self-hosted version.
# Like you can see below, the link "https://raw.githubusercontent.com/Dok4440/TCURawScripts/Latest_stable_version/TCU_master_installer.sh" is a Github-raw link
# If you do copy/download these files without forking MY repo, I recommend you make your own repo to pull these raw shell-scripts from.

# Talking about the links, make sure they're correct, so after a fork you'll need to change every single one of them.
# If you come accross any issues FROM THE LATEST STABLE VERSION changing the code, I'm willing to help. 
# Know there's a limit to my support and I can deny any request for help.

# Furthermore I will try to put as much info to every (considered) hard line to help you out on the "changing-code" process
# This is ONLY for '.sh' files and does not take account in any of TCU's sourcecode unrelated to self-hosting.

# Thanks for using TCU <3

echo ""
echo "Welcome to the TCU self-host installer. Downloading the latest installer..."
root=$(pwd)


# Starting off; the "linuxAIO.sh" file is the file you always bash whenever changing TCU's config.
# It simply opens 'TCU_master_installer' but the reason you need to bash THIS file is simple;
# As you can see at like 35 & 36; it cd's into root and deletes TCU_master_installer after opening the file.

wget -N https://raw.githubusercontent.com/Dok4440/TCURawScripts/Latest_stable_version/TCU_master_installer.sh


bash TCU_master_installer.sh
cd "$root"
rm "$root/TCU_master_installer.sh"
exit 0
