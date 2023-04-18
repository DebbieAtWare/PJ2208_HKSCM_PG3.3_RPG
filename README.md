# PJ2208_HKSCM_PG3.3_RPG
* **Author**: Debbie Wong debbie@ware.hk
* **Project link**: [PJ2208_HK Science Museum_PG Interactive 2021](https://drive.google.com/drive/folders/16270uT0EqpZD2BSOZLHnVnw37C7qTm9n)

## Project Description
HKSCM PG3.3 is a RPG game, standalone window program. It consists of 3 parts:
* Main Game Program (current github project)
* Arduino Program for physical button
* [Light Mini Program for physical lighting](https://github.com/WAREproject/PJ2208_HKSCM_PG3.3_Light)

## Development Environment
* Unity 2020.3.8
* Windows 11

## Resolution
* Landscape 1920x1080

## Input Control
The onsite setup should be connected with Arduino and physical buttons. But for testing, keyboard control is also availble.
* Up - "W"
* Down - "S"
* Left - "A"
* Right - "D"
* Menu(B) - "E"
* Confirm(A) - "Space"

## CMS Excel
Some of the game contents can be adjusted in the CMS Excel. 
* Location: `/RPG_Data/StreamingAssets/Exh3.3_Content.xlsx`
* Please keep the **same file name** when replace
### dialog box support image
* Image files location: `/RPG_Data/StreamingAssets/SupportImage/`
* Please keep the **same file name** when replace and type the same file name in the **excel**
* Please remind to include the **file extension** in the excel image file name (eg. M01_Hylonomus_1.png)

## Installation Setup
1. Copy the release project folder to `C:/User/Desktop/`
2. Check the Config.json at `/RPG_Data/StreamingAssets/Config.json`
3. The Unity main game program is at `/RPG.exe`
