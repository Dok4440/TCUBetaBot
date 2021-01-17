# TCU_err_* list
This directory in the Guide branch is the complete error list of TCU. All errors logged **inside of Discord & thus as a return from TCU** will be listed here. That being said, these errors aren't messages that mean the bot is broken in any way. They simply mean **you have been doing something wrong**. Hence this error list.

## List of errors
* [TCU_err_04](all proper links will be added at the final ERROR commit.)
* [TCU_err_08](^)
* [FAQ](^)


## TCU_err_04 - Insufficient permissions.
This command requires Bot ownership for global custom reactions, and Administrator for server custom reactions.
You'll have to do this in a **server** where you have Admin permissions. Doing this in DMs is **only for bot owners**. There is no way to add or edit a custom reactions without the sufficient permissions. You can change this, however, by editing the source;  
```
if (!AdminInGuildOrOwnerInDm())
{
    await ReplyErrorLocalizedAsync("insuff_perms").ConfigureAwait(false);
    return;
}
```  
Comment out or edit [this](https://gitlab.com/Dok4440/TCUBetaBot/-/blob/develop/TCU.Core/Modules/CustomReactions/CustomReactions.cs#L38) in the source

## TCU_err_08 - No custom reaction found.
This error can appear as "No custom reaction found" & "No custom reaction found with that ID". Both of these errors mean you've been specifying or searching for a custom reaction that does not exist. The easiest way to "solve" this problem is by doing `.lcr`. This lists all custom reactions in the current server. And when done in DMs, it lists all global custom reactions (when you're a bot owner, `...selfhost`).

*For people who are really weird (yes there's quite a few)*; if you really want to use any CR command on an unexisting custom reaction. You can make one. do `.h acr`

## TCU_err_common1
You tried a command & got a "common1" error; this means that the guide will *assume* this error was "clear" enough for you to figure out what's going on inside of Discord. Therefore there's explanation on _common1 errors. Do you wish to get more information on these errors / you're still confused as to why this error is appearing. Simply ask for help in [TCU's support server](https://discord.gg/bYGcGCCRr2).   

 Current list of these errors;

- "No stats for that trigger found, no action taken."

## FAQ
### Why aren't these numbers adding up / making sense?  
The errors are not numbered in terms of addition; it does not start with zero and adds up similarly. The numbering is based on the number of the line in the [ResponseStrings](https://gitlab.com/Dok4440/TCUBetaBot/-/blob/develop/src/TCU/_strings/ResponseStrings.en-US.json). This makes it easier to modify when you host the bot yourself.

### I'm still confused as to what the heck my error means..  
No problem :), [TCU's support server](https://discord.gg/bYGcGCCRr2) is ready for you!  

### Why isn't there any information on NSFW errors?     
As Dok tries to keep everything, apart from actual NSFW Discord channels, safe for work, we're not giving any "extra" support on any commands and/or issues within the NSFW module. Must there be an *actual* bug in that module happening on public TCU, you can report this in [TCU's support server](https://discord.gg/bYGcGCCRr2).

### I'm trying to do a command, but it doesn't reply or show me any error, why?  
First off. TCU does not always reply to a "wrongly" done command, if you do want TCU to do this, check out our "verbose error" command by doing `.h ve`. Back to the question, there's a couple things that could have gone wrong here.

- You're selfhosting & the bot isn't running while you *think* it is. Check your console. If it's running, check for errors & report them in [TCU's support server](https://discord.gg/bYGcGCCRr2).
- You're using public TCU & you're unsure if the bot is down or not. In TCU's support server, check #support, for bot downtime. **It will always be noted there**. Must it be online and you can't do command(s), contact that same channel as well.


