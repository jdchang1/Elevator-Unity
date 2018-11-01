Easy Elevator (Unity 5.3.5f1)
By: Nifty Studios Inc.
http://www.niftystudios.com 
info@niftystudios.com 


#######################################################################################################
* IMPORTANT: The Resources/LEDPanelTexturesV2 folder MUST be placed in your Projects Assets Root folder. 
   * IE: (YourProjectsName)/Assets/Resources/LEDPanelTexturesV2
#######################################################################################################

________________


PACKAGE STRUCTURE:
* IMPORTANT: The Resources/LEDPanelTextures folder MUST be placed in your Projects Assets Root folder. 
   * IE: (YourProjectsName)/Assets/Resources/LEDPanelTexturesV2
* /Easy Elevator/
   * /Demo Scene/
      * /Demo Models/
         * [ DEMO SCENE MODELS ]
         * /Materials/
      * /Demo Prefabs/
         * [ DEMO SCENE PREFABS ]
      * Demo Scene
      * /Demo Scene/
         * [ DEMO SCENE  LIGHTMAPS ] - Removed in Unity 5 package
      * /Demo Script/
         * elevFollow.cs
         * secondCamTrigger.cs
      * /Shader/
         * RimLight.shader
   * /Elevator/
      * /Easy Elevator (non scripted prefabs)/
         * ElevatorPREFAB
         * ElevHallFramePREFAB
      * ElevatorScriptedPREFAB_V2
      * ElevHallFrameGroupPREFAB_V2
      * ElevHallFrameScriptedPREFAB_V2
      * /Models/
         * ElevatorV2
         * /Materials/
      * /Scripts/
         * callBtnTrigger.cs
         * controlTrigger.cs
         * elevControl.cs
         * elevHallFrameController.cs
      * /Textures/
         * [ ELEVATOR TEXTURES ]
* /Resources/
      * /LEDPanelTexturesV2/
         * [ TEXTURES FOR LED DISPLAY ]


________________


SETUP:
* IMPORTANT: Ensure the Resources/LEDPanelTexturesV2 folder is placed in your Projects Assets Root folder. 


1. Drag ElevatorScriptedPREFAB_V2 into your scene
   1. Check tag on prefab. If not set, add a tag to the prefab ( ie: elev01 )
   2. In the Inspector window, check that the Hall Frame Tag field in the Elev Control (Script) component has a string assigned ( ie: elev01hallFrame )    
1. Drag ElevHallFrameGroupPREFAB_V2 into your scene
   1. Expand the group, select all the Gameobjects inside the group and assure the tag matches the string on the ElevatorScriptedPREFAB_V2 > Elev Control (Script) > Hall Frame Tag field
   2. Assure that the string in Elev Tag field of Elev Hall Frame Controller (Script) matches the tag set on the ElevatorScriptedPREFAB_V2


________________


DEFAULT CONTROLS ( found in elevControl.cs > Update() )


HALL:
When close to the call button on the hall frame (inside trigger) , the button highlights to green.
Press E to activate the button (button changes to yellow when pressed)


ELEVATOR:
When close to the elevator panel (inside trigger), the button for the current floor highlights to green.
Press R to highlight the next floor button
Press F to highlight the previous floor button
Press E to activate the button (button changes to yellow)




________________


CLOSER LOOK AT SCRIPTS:


elevControl.cs ( attached to ElevatorScriptedPREFAB )
Button On Mat <Material> >> The material used for the activated buttons
Button Off Mat <Material> >> The material used for the non activated buttons
Button Selector Mat <Material> >> The button highlight material
Led Mat <Material> >> The material used for the both the elevator and hall frame LED displays. 
* IMPORTANT: When using multiple elevators, this material must be unique to each elevator / hall frame combo. This material is assigned to the LED Displays in the Start() function 
Led Panel <Transform> >> The LED Panel of the this elevator
Led Mat Switch Delay <Float> >> The amount of time the LED panel is blank while changing floors
Bth Light Group <Transform> >> The group of button lights on this elevator panel
Hall Frame Tag <String> >> The tag used for the each hall frame gameobject in the HallFrameGoupPREFAB that this elevator can travel to
* IMPORTANT: This string MUST match the tag on the ElevHallFrameScriptedPREFAB gameobjects parented to the ElevHallFrameGroupPREFAB. When using multiple elevators, this string AND tag MUST be different for each elevator / hall frame combo
Cur Floor Level <Int> >> The floor the elevator will be on when the game starts
Time Btwn Floors <Float> >> How long the elevator takes to travel between floors
Doors Open <Bool> >> If true, the doors will be open when the game starts
Wait For Fixed Update <Bool> >> A fix for player jitter when the elevator is moving


elevHallFrameController.cs ( attached to ElevHallFrameScriptedPREFAB )
Floor <Int> >> The floor this hall frame is on
Call button Light <Transform> >> The button light gameobject on this hall frame
Hall Led Panel <Transform> >> The LED display on this hall frame
Elev Tag <String> >> The tag for the elevator associated with this hall frame
* IMPORTANT: This string MUST match the tag on the ElevatorScriptedPREFAB gameobject that will be using this hall frame to travel to. When using multiple elevators, this string AND tag MUST be different for each elevator / hall frame combo


controlTrigger.cs ( attached to ElevatorScriptedPREFAB>TriggerControls  )
( NO CONFIGURATION REQUIRED )


callBtnTrigger.cs ( attached to ElevHallFrameScriptedPREFAB>TriggerCallBtn )
( NO CONFIGURATION REQUIRED )




________________


NOTES:


The position of the ElevatorScriptedPREFAB_V2 in the scene is not important. On Start(), the position is set to the Hall Frame position of the current floor set in the ElevatorScriptedPREFAB ( Elev Control ( Script )> Cur Floor Level )


Animations need to be labeled “OpenDoorsV2” / ”CloseDoorsV2”


The textures for the LED mat are, and MUST be in the “[YourUnityProject]/Assets/Resources/LEDPanelTexturesV2” folder. 


If using more than 1 elevator, the LED mat assigned in the inspector for each elevator must be different. The script will assign this material to the elevator LED and the Hall frame LEDs that belong to this elevator.
ie: 3 elevator setup will require 3 LED materials elevLED, elevLED2, elevLED3. Assign each material to 
ElevatorScriptedPREFAB_V2>Elev Control (Script)>Led Mat 
ElevatorScriptedPREFAB2_V2>Elev Control (Script)>Led Mat 
ElevatorScriptedPREFAB3_V2>Elev Control (Script)>Led Mat


________________
DEMO SCENE GUIDE

Import the First Person Character from the Unity Standard Assets package. (can be downloaded and imported from the Unity Asset Store)

Drag the "FPSController" prefab into the Demo scene, and make the following changes to it:

CHARACTER CONTROLLER
Skin Width = 0.02
Radius = 0.3

FIRST PERSON CONTROLLER
Use Head Bob = OFF


The controls for the Elevator in the Demo Scene are:
E - Press Button
R - Select Next Button
F - Select Previous Button

________________
CHANGE LOG:
Apr 23 2018
As per new Asset submission guidelines, the First Person Character controller included in the Unity Standard Assets package was removed.
Modified the Demo scene to display the Elevator controls
Removed Nifty Studios Logo from the Demo Scene

Feb 21 2017
Adjusted Elevator UVs, animations, created new textures and materials for Unity 5 PBR Standard material.
Modified  elevControl.cs to work with Standard Material (will not work with legacy)
Modified elevHallFrameController.cs to work with new animations


Mar 4, 2015
Repackaged for Unity 5
Fixed script API changes from Unity 4 to 5
Re-linked animation clips in prefabs ( links were lost when importing into Unity 5 ) 
Removed deprecated lightmaps folder -> /Easy Elevator/Demo Scene/Demo Scene. The Demo Scene no longer has lightmaps
