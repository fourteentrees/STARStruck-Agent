# STARStruck Agent
agent for [STARStruck](https://github.com/fourteentrees/STARstruck) which itself is used to remotely manage AffiliateAds, LOT8s greetings, and SpecialMessage.xml entries for the IntelliSTAR 2

This REQUIRES the server to be installed and configured first. Install this once you are done with that.

## Quick setup how-to
**BEFORE INSTALLING, MAKE SURE YOU BACK UP THE FOLLOWING FILES IN A LOCATION *OUTSIDE OF YOUR I2 DIRECTORY*:**
- Managed\Events\AffiliateAds.xml
- Managed\Events\LOT8WelcomeProductTextPhrases.xml
- Managed\Events\SpecialMessage.xml
- Managed\Events\WxDotComPromoText.xml
This application OVERWRITES these files with ones from the server. If you switch back you might want to keep your original files.

First off you need .NET 6.0 but you probably already have that from i2ME. If not - [get that now](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

Now follow these simple steps
1. download a build from Releases
2. Save and extract to C:\STARStruck-Agent
3. Run it once by double-clicking
4. Edit the StarstruckURL field and point it to the server
5. Import "Sync from STARStruck.xml" into your Task Scheduler
  - Make sure the run as user is i2Admin or i2Dev.
6. Now run it from task scheduler.