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

## FAQ
**Why aren't these numbers adding up / making sense?**  
The errors are not numbered in terms of addition; it does not start with zero and adds up similarly. The numbering is based on the number of the line in the [ResponseStrings](https://gitlab.com/Dok4440/TCUBetaBot/-/blob/develop/src/TCU/_strings/ResponseStrings.en-US.json). This makes it easier to modify when you host the bot yourself.

**I'm still confused as to what the heck my error means..**  
No problem :), [TCU's support server](https://discord.gg/bYGcGCCRr2) is ready for you!


