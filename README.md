# RimChat

RimChat is Rimworld mod that allows in game chat between players through in game chat server hosting and joining. This project is mainly for learning Rimworld modding and networking.

# Development
To setup the development environment locally please follow the below steps. These instructions assume Windows and Visual Studio are being used.

### Fork and Clone the project

Create your own fork of the project and clone it locally by following the github documentation [here](https://help.github.com/articles/fork-a-repo/). Once you have committed any local changes to your forked repository, open a [pull request](https://help.github.com/articles/about-pull-requests/) against the main repo.

### Create the local mod directory

Create a subdirectory for RimChat in your Rimworld Mods directory and copy the Mod content, typically this can be found at C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods

For example, if you created the subdirectory RimChat these folders should be copied over
..\RimWorld\Mods\RimChat
- About
- Assemblies
- Defs
- Languages

### Setup Visual Studio

The project should already be set up after cloning, but you may need to add your local references. The location for these can be found in the quick check list below. For an overall understanding of setting up Visual Studio for Rimworld modding, I recommend following the excellent tutorial by Jecrell on the Ludeon Forums [here](https://ludeon.com/forums/index.php?topic=33219.msg338631#msg338631) for setting up the development environment.

Visual Studio quick check list:
- RimChat Project->Properties->Application->Target Framework should be .NET Framework 3.5
- RimChat Project->Properties->Build->Output path should be set to the Mod location you created, ex ..\RimWorld\Mods\RimChat\Assemblies\
- RimChat Project->Properties->Build->Advanced->Debugging information should be set to None
- Add the Rimworld Assemblies, Assembly-CSharp and UnityEngine using your local rimworld dlls, typically found at C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed\
- Make sure the following references have their Properties->Copy Local set to False: Assembly-Csharp, UnityEngine, websocket-sharp to prevent issues reloading the entire game dll and to avoid load order issues with websocket-sharp
