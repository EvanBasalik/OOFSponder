# OOFSponder ReadMe

![Image of primary UI](/ReadMeImages/PrimaryOOF.png)

## Getting Started

👉 Try the new CDN-based, signed version here: [OOFSponder CDN Install](https://aka.ms/OOFSponderInstall) 👈

> **Update:** The CDN-based release is now rolling out to production! Existing ClickOnce users are being automatically migrated to the new CDN-based install. The migration will show some approval/install prompts, but general functionality won't change and it will still continue to auto-update.

> Note: Alpha and Insider are already migrated to the new CDN release. Production is now fully rolling out with automatic migration for all users on weekdays.

Note that Windows may throw a warning the first time you install the older OOFSponder, but if you select _Keep_ and then _Run anyway_ it will install properly. Future upgrades will not show the same warning, nor will the CDN-based installer.

Set the hours to when you work, if you don't work that day check Off Work.  

_Hint: If you set the start time and end time to the same time for a day it won't set up the OOF message for that day._

Compose both your External and Internal OOF message.

_Hint #1: Create the message in Outlook and copy it in to the window, OOFSponder will keep all your formatting._  
_Hint #2: Hitting Save Settings will push your OOF message to Exchange Online immediately._

Click the Save Settings button to store all your settings locally. OOFSponder will wake up every 10 minutes and do the math to set your OOF message as appropriate.

If you close the window OOFSponder will continue running in the background. You can double click the icon in your system tray to open it back up.

## Alternate Messages

If you're going to be out of the office unexpectedly, or maybe you're just taking an extra day (or it's a Company Holiday), you can select the _Alternate OOF_ radio button and set appropriate alternate messages. Update your working hours and/or Off Work days for the change, and click Save Settings. You're all set. On your next working day after setting _Alternate OOF_ OOFSponder will automatically revert back to your Primary OOF Messages after the start of your configured working hours.

![Image of secondary UI](/ReadMeImages/AlternateOOF.png)

## Extended OOF

If you are going on extended OOF (maybe you're finally taking that 2 week vacation you've been planning), select the _Alternate OOF_ radio button, set the alternate OOF messages accordingly, and then pick the day BEFORE you want your normal OOF schedule to resume. This functionality can also be used when you leave early since OOFSponder treats the time you click _Enable Extended OOF_ as the start time for the alternate message. If you're only leaving early for the current day, simply use the Alternate Messages process above. On the day after the one you selected on the calender, Extended OOF automatically reverts back to your Primary OOF Messages and configured schedule. Your OOF Message won't revert back the Primary until after the start of your configured working hours.

![Image of secondary UI Selecting Date](/ReadMeImages/ExtendedOOF-1.png)

![Image of secondary UI Enabling Extended OOF](/ReadMeImages/ExtendedOOF-2.png)

## Audience Scope

The _Audience Scope_ dropdown controls who receives your external OOF message. There are three options:

- **None** – No external OOF message is sent to anyone outside your organization. When this option is selected, the External Message editor is disabled.
- **Contacts Only** – Only senders who are in your contacts list will receive the external OOF message.
- **All** – All external senders will receive the external OOF message.

_Hint: If you do not want to send an external OOF message at all, set the Audience Scope to **None**._

![Image of the Audience Scope Drop Down](/ReadMeImages/AudienceScopeDropDown.png)

## Managing Saved Messages

Whenever you edit any of your OOF Messages, OOFSponder automatically saves a copy for posterity. By default, OOFSponder keeps up to 10 messages of each of the four message types (Primary External/Internal and Alternate External/Internal).

You can access your saved messages by going to "File...", then "Open Saved OOF Message...".

![Image of Selecting the Open Saved OOF Messages Menu Item](/ReadMeImages/OpenSavedOOFMessages.png)

You can also configure how many saved copies OOFSponder keeps by going to "File...", then "Max Message History (x)..." ("x" shows the current number of messages being kept).  
When you save a change to the Max History value, OOFSponder will immediately attempt to cleanup any older messages above the new value.  
Valid values are from 1 to 99.  

![Image of Selecting the Max Message History Menu Item](/ReadMeImages/MaxMessageHistoryMenuItem.png)

![Image of the Max Message History Dialogue Box](/ReadMeImages/MaxMessagHistoryDialogue.png)

## Legacy ClickOnce Installer
If you still want the legacy ClickOnce installer, use [this old link](https://evanbasalik.github.io/OOFSponder/production/OOFScheduling.application).
