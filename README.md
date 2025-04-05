# Warudo Environment and Prop Animation Controller
This repository provides a set of scripts to make you control your animation in Warudo Editor throw dummy AnchorNode, it doesn't require install mods to Warudo.

## How to use
### Create a Manager
Create an Empty gameobject in the scene. And add "W Interaction Manager" Script to it.

![image](https://github.com/user-attachments/assets/8903b4e5-d4a6-42b6-b73f-4e6f0215d65c)

![image](https://github.com/user-attachments/assets/5615f221-2dc2-4b9d-b680-17fac6d38ff0)

You need set the "Manager Prefix" at least, or it may conflict with others.   

This script will automatically search Anchor Node in Warudo with specific name.  

There are three child script to provide different feature.

* AnimationController: Sync AnchorNode State to Animator component.
* TransformToAnchorController: Sync GameObject Transform to specific AnchorNode.
* AnchorToTransformController: Sync AnchorNode Transform to GameObject

Left the Waypoints/Animations/Transforms empty, these field will automatically filled by script.  

### Add Script to GameObject
#### Animation Controller
![image](https://github.com/user-attachments/assets/b92ac36b-6ad3-4027-bde0-8c34d1c9860b)

This script require Animator on the same object.It will sync value to the Animator Param.  
* Name Policy: How to name this object on AnchorNode, `SELF` will use this gameobject's name(If this object is the root of Prefab, The actual name of each object on scene will be use), `PARENT` will use the parent gameobject, `OTHER_GAMEOBJECT` will use custom gameobject, `CUSTOM` will use a custom name.
* Custom Name: When Name Policy is `CUSTOM`, will use this name.
* Custom Name Game Object: When Name Policy is `OTHER_GAMEOBJECT`, will use the name of this game object.
* Name Prefix: Prefix the name.
* Manager Name: The Manager GameObject Name.
* Stop Animator At Startup: Will pause the animation on startup.
* Animator Speed: the speed sync to animator.
* Animation Params Type: `FLOAT` will use AnchorNode Transform X Axis value as float value pass into Animator Param. `INT` will use AnchorNode Transform X Axis value and cast it to integer value pass into Animator Param. `BOOL` will use the enable state of AnchorNode to the Animator Param.
* Param Name: Which Param should be use.
* Value Float: When Animation Params Type is FLOAT, this value will sync.
* Value Int: When Animation Params Type is INT, this value will sync.
* Value Bool: When Animation Params Type is BOOL, this value will sync.

The Anchor Node Name will concat by `Manager Prefix` + `Animation Sync Name Prefix` + `Name Prefix` + `Name`

#### Transform To Anchor Controller
![image](https://github.com/user-attachments/assets/f8893c95-1fe2-4cdd-a8b9-7c71639d068a)

This script will sync GameObject Transform to AnchorNode.
* Name Policy: How to name this object on AnchorNode, `SELF` will use this gameobject's name(If this object is the root of Prefab, The actual name of each object on scene will be use), `PARENT` will use the parent gameobject, `OTHER_GAMEOBJECT` will use custom gameobject, `CUSTOM` will use a custom name.
* Custom Name: When Name Policy is `CUSTOM`, will use this name.
* Custom Name Game Object: When Name Policy is `OTHER_GAMEOBJECT`, will use the name of this game object.
* Name Prefix: Prefix the name.
* Manager Name: The Manager GameObject Name.

### Anchor To Transform Controller
![image](https://github.com/user-attachments/assets/13da6dab-3a4e-4b25-8608-c679a15804be)

This script will sync AnchorNode Transform to GameObject.
* Name Policy: How to name this object on AnchorNode, `SELF` will use this gameobject's name(If this object is the root of Prefab, The actual name of each object on scene will be use), `PARENT` will use the parent gameobject, `OTHER_GAMEOBJECT` will use custom gameobject, `CUSTOM` will use a custom name.
* Custom Name: When Name Policy is `CUSTOM`, will use this name.
* Custom Name Game Object: When Name Policy is `OTHER_GAMEOBJECT`, will use the name of this game object.
* Name Prefix: Prefix the name.
* Manager Name: The Manager GameObject Name.

## Name Search Policy
The Example  
**Manager:**    
Manager Prefix: `Hello_`  
Transform To Anchor Sync Name Prefix: `T2ASync_`  

**Transform To Anchor Controller:**  
GameObject Name: `World`  
Name Policy: `SELF`  
Name Prefix: `Obj_`

The result of name will be `Hello_T2ASync_Obj_World`
