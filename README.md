## ReadMe

**OOFSponder 3.0 (new features such as OOF message saving, defaulting to minimized) is here!!
If you are willing to try it, it can be installed from [https://evanbasalik.github.io/OOFSponder/production/OOFScheduling.application].
_Note that you'll need to move to the released version manually for now. Automated upgrade from 2.6+ is coming in the near future, though!_**

![Image of primary UI](https://github.com/EvanBasalik/OOFSponder/blob/main/ReadMeImages/Primary.png)
**Getting Started**

Install from [here](https://oofsponderinstall.blob.core.windows.net/install/install.htm). Note that Windows will throw a warning the first time you install OOFSponder, but if you select _Run anyway_ it will install properly. Future upgrades will not show the same warning.

Set the hours to when you work, if you don't work that day check Off Work.  

_Hint: If you set the start time and end time to the same time for a day it won't set up the OOF message for that day._

Compose both your External and Internal OOF message.

_Hint #1: Create the message in Outlook and copy it in to the window, OOFSponder will keep all your formatting._ <br>
_Hint #2: Hitting Save Settings will push your OOF message to Exchange Online immediately._

Click the Save Settings button to store all your settings locally. OOFSponder will wake up every 10 minutes and do the math to set your OOF message as appropriate.

If you close the window OOFSponder will continue running in the background. You can double click the icon in your system tray to open it back up.

If you are going on extended OOF, select the _Secondary OOF_ radio button, set the secondary OOF messages accordingly, and then pick the day when you want your normal OOF schedule to resume. This functionality can also be used when you leave early since OOFSponder treats the time you select _Enable Extended OOF_ as the start time for the secondary message.
![Image of secondary UI](https://github.com/EvanBasalik/OOFSponder/blob/main/ReadMeImages/Secondary.png)
