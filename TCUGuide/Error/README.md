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



## FAQ
**Why aren't these numbers adding up / making sense?**  
The errors are not numbered in terms of addition; it does not start with zero and adds up similarly. The numbering is based on the number of the line in the [ResponseStrings](https://gitlab.com/Dok4440/TCUBetaBot/-/blob/develop/src/TCU/_strings/ResponseStrings.en-US.json). This makes it easier to modify when you host the bot yourself.

**I'm still confused as to what the heck my error means..**  
No problem :), [TCU's support server](https://discord.gg/bYGcGCCRr2) is ready for you!


