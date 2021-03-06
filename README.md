# Bard class for Solasta

This is a mod for the game Solasta: Crown of the Magister.
It adds bard class with following college subclasses: College of Lore, College of Nature and College of Virtue.
This mod requires SolastaModApi https://github.com/SolastaMods/SolastaModApi
and (starting from version 1.0.0) SolastaModHelpers https://github.com/Holic75/SolastaModHelpers

# Know Issue
When the bard attempts to use a reaction during other party memeber turn 
(like Music of the Spheres from college of Virtue) and the camera attempts
to focus on this action, the game might freeze completely. 
Please set contextual camera frequency to 0 (in Settings->Game->Camera) if you experience this issue.

# How to Install

1. Download and install [Unity Mod Manager (UMM)](https://www.nexusmods.com/site/mods/21)
2. Execute UMM, Select Solasta, and Install
3. Download and install [SolastaModApi](https://www.nexusmods.com/solastacrownofthemagister/mods/48) using UMM
4. Download and install [SolastaModHelpers] using UMM
5. Download and install the mod (.zip file from Releases page) via UMM 

# How to Compile

0. Install all required development pre-requisites:
	- [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/downloads/)
	- [.NET "Current" x86 SDK](https://dotnet.microsoft.com/download/visual-studio-sdks)
1. Download and install [Unity Mod Manager (UMM)](https://www.nexusmods.com/site/mods/21)
2. Execute UMM, Select Solasta, and Install
3. Download and install [SolastaModApi](https://www.nexusmods.com/solastacrownofthemagister/mods/48) using UMM
4. Download and install [SolastaModHelpers] using UMM
5. Point SolastaInstallDir variable to your solasta isntall directory in *.csproj
