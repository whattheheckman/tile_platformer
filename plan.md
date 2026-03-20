make scripts for a 2d platformer unity prototype.

you should include the following types of scripts at least:
- player scripts
- enemy scripts
- game state scripts
- coin collectable
- moveable platforms
- projectile scripts
- level features

## player features
- player has health
- player movement is 2d physics based.
- player has animations for idle, moving, and shooting a projectile
- player should flash current sprite red when hurt
- player can collect collectables
- player can be hurt by obstacles in tilemap or by enemies
- player has hurt sound

## enemy features
- enemies move from side to side on platform without falling off (use 2d raycasts)
- enemies can have different speeds (expose as a serialized field)
- enemy has hurt sound 


## moving platform features
- platforms should move side to side on path defined using transforms set in editor

## game state features
- game can be paused with a pause screen overlay
- game restarts from current level when player dies
- game has a win condition upon touching an object
- each level has a timer for how long it's taken to complete

## collectable features
- collectables have a idle animation, and collect animation
- collectables should make a sound when collected

## levels features
- levels transition based on cinemachine boundaries
- level paused on boundary
- level has different audio per boundary (maybe attach audio source to different cinemachine boundary? if you have a different/better idea do something else)

there are no existing script files, just read the directory structure to see where to place files

use serialize field for variables like a projectile spawned by the player, otherwise use getcomponenet and require component

after script is implemented include simple notes and instructions on how to use script in comments at top of each script

thanks and good luck.
