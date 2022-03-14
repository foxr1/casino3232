# CASINO 3232

## Overview
**CASINO 3232** is my coursework for CSC3232 Gaming Technologies and Simulations. 

### Lobby
After entering the lobby from the main menu, there is a room filled with NPCs that walk and dance around the lobby. They will carry out various actions depending on the volume of the music. They can also be pickpocketted to earn money in the game. 

#### NPCs

##### Standard NPCs
The player can crouch, *(Left Control)*, when behind an NPC in the lobby, they will get an option to pickpocket to earn money in the game. The closer the player is to the NPC, the quicker they take money. This can be used to purchase the door to access the room to play *Rummy*, or to play *Blackjack* as an alternative to pickpocketting.

##### Wardens
There will be 3 *Wardens* that spawn in the lobby coloured in blue, they will wander around the lobby and if they catch you trying to pickpocket another NPC, they will chase after you to kick you out.

##### DJ
There is a DJ located in the lobby which is constantly dancing to the music. There is a slider placed infront of him which the player can interact with to change the volume of the music. The louder the music, the more the NPCs will dance, and when placed at a loud enough volume, they will all congregate (including the *Wardens*) on the dance floor in the middle of the lobby.

### Blackjack
The player starts with 50 coins when they join the lobby, they can then bet this money for a chance to win up to 2x their bet. The objective is to get as close to 21 while also beating the dealers hand. 

- If the player gets *Blackjack* (21), they are rewarded with **1.5x** their bet amount
- If they beat the dealers score without going bust, they are awarded **2x** their bet amount
- If they tie with the dealer, they are awarded their **initial** bet back

The player can also walk up to the dealer in the room and purchase a key for 250 coins as an alternative to winning the key from playing *Rummy*.

### Maze
In the *Maze* game, the player can bet on either a blue or orange *mouse* which both try to solve a randomly generated maze as quickly as possible, the goal is to reach the centre of the maze. The player can modify the size of the maze, and in doing so will increase the bet multiplier, increasing the amount the player can win from the game. This starts at x1.5 with the *maze* size set to 15x15 and increased to x5 at size 50x50.

### Rummy
When the player has 100 coins or more, they can attempt the *Rummy* card game. The player starts with 7 cards and must win by having 3 or more of a set or run in their hand. Within the game there is more detailed instructions along with example winning hands. There is an AI that competes against the player and if the player beats the AI, they will be rewarded with a key to access the *Puzzle Room* where the player is faced with multiple levels involving physics based puzzles that they need to solve. This AI competes against the opponent by gaining knowledge as the game progresses, keeping track of each player's deck as well as what cards have been discarded.
 
### Shop
In the *Shop* the player can purchase two powerups, both of which cost 500 coins.
- **Undetectable Pickpocketing:** When purchased, the player will no longer be detected by *NPCs* or *Wardens* when pickpocketing in the lobby. 
- **Running Shoes:** When purchased, the player can run using the *Shift* key. 

There is also a fish tank under the floor, demonstrating an implementation of *Flocking AI*.
 
### Puzzle Game
The user is introduced with some basic instructions to help them understand the controls and mechanics of the game, which is then evolved into more challenging puzzles later on. They can pick up objects, increase/decrease their size/mass and also throw the object. There are 10 levels to solve each of which tries to build on previously taught mechanics or teaching the user about new ones.

### Space
If the user is caught any NPC they are trying to pickpocket, they will be chased. If the NPC contacts the player, they will be ejected to space. This will also happen if the Warden catches the player trying to pickpocket any other NPC. From here, they will need to solve a simple parkour to get back to the lobby. They are however faced with an oxygen meter which depletes over time. Throughout the stage there are oxygen canisters which can replenish their oxygen meter so they are able to reach the door to enter the casino.

## Sources

All code has been approriately commented with references to any tutorials or forums used to help complete parts of my code.

### Models

#### Player/NPC model
This was made by myself in Blender using a tutorial series on YouTube: [Game Character Creation in Blender](https://www.youtube.com/playlist?list=PLKklF7YNi0lPbmf095F8c-pyS7HDOM5I_). I used this to create a simple low-poly character with idle and walking animations to use for both the player and NPCs. I then adapted what I learnt from this to create crouching and dancing animations.

#### Disco Ball
This was found at: https://sketchfab.com/3d-models/disco-ball-e4c3b485680843c7a7a827d04ac28743

#### Oxygen Tank
This is one of the models included in the package found at: https://www.turbosquid.com/3d-models/3d-parts-cart-1704660

### Unity Packages

#### Depencencies
If not automatically installed, this project requires TextMeshPro, ProBuilder and Post-Processing Unity packages to be installed.

#### Playing Cards
All the card sprites were downloaded from the Unity Asset Store at: https://assetstore.unity.com/packages/3d/props/tools/free-playing-cards-pack-154780

#### Wood Textures
The wood textures used for signs across the game were from the Unity Asset Store at: https://assetstore.unity.com/packages/2d/textures-materials/wood/wooden-floor-materials-150564

#### Space Skybox
The space skybox used in the lobby and space scene were from the Unity Asset Store at: https://assetstore.unity.com/packages/2d/textures-materials/sky/spaceskies-free-80503

### Sounds
All the sounds below were created by me:
- The "speech" sound effect for NPCs in the lobby
- Card being placed or picked up

#### Music
Music for multiple scenes in the game were taken from the Unity Asset Store at: https://assetstore.unity.com/packages/audio/music/electronic/electronic-music-loops-141736

#### In-game sound effects
Sounds like when hovering/clicking a button and win/lose sounds were taken from the Unity Asset Store at: https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116
The ice freezing sound effect used in the *Space* scene was taken from: https://freesound.org/people/kyles/sounds/452645/

## Running The Game
There is a pre-built executable in the "Build" folder to play the game.

*Created by Oliver Fox*
