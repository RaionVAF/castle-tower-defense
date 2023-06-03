# castle-tower-defense
 
Team members: Cameron Ehnat, Raion Vincent Amper Fet, Tyler Krauth, Joshua Silva, Maureen Zajic

CMSC425: Game Programming

Concept
Knight’s Kingdom is a medieval tower defense game. The player then takes control of the knight and has to manage the defense of the castle. There are towers along the border of the kingdom that shoot at the undead enemies and protect the villages around them. Sometimes when these enemies are killed, materials such as wood, rock, and iron appear. These materials can be picked up and brought to a blacksmith. The blacksmith can use these materials to upgrade the towers. Wood upgrades the speed of the tower projectiles, rock upgrades the damage of each projectile, and iron allows the player to upgrade the towers from having ballistas to having cannons, significantly increasing damage. There are three areas the player can access, the north village, the south village, and the east village. The player will have to manage their resources to upgrade all towers defending each village and not allow the village or kingdom wall to fall. The undead enemies will come from the forest surrounding the villages and make their way towards the castle. They will first attack village buildings, then once those are all destroyed, attack the castle wall. Enemies include zombies, skeletons, undead knights, and a final boss. Each enemy has unique attacks and drop rates for materials. Defeating enemies also gets the player XP, which the player can use with the alchemist. The alchemist can use the XP to repair village buildings and castle walls. This is important because if any of the castle walls are destroyed, the kingdom will be overrun and the game will end.

Visual and Artistic Elements
The game consists of many medieval elements, such as the castle, the towers, the knight, the blacksmith, and the alchemist. We wanted all of the elements of the game to be as cohesive as possible. So our UI elements are also made to match the medieval theme of the game and the health bars are purple to match the accents on the castle, towers, and knight. Our game also has background music and sounds such as zombie groans, arrows shooting, and menu clicks to make the experience more immersive. All characters are animated; the knight has walking animations, the king, blacksmith, and alchemist have talking animations, and the enemies have attack animations. When an enemy dies, they have an animated death and particles representing XP appearing. The interaction popup above the blacksmith and alchemist is animated to go up and down.

Algorithmic Complexity
Enemies are programmed to make their way towards and attack buildings using a NavMesh as accessible terrain and a method attached to each that find the closest target to their current position. When the player is close to a building, a popup of either a health bar or interaction will appear above the building. The health bar will fill and deplete depending on the health of the building. If the player is near a blacksmith or alchemist, an interaction button will appear and, if pressed, bring the player in and out of the blacksmith and alchemist menus. In these menus, players can choose upgrades that will upgrade the speed and power of the towers and will heal buildings. Towers are programmed to shoot enemies that enter their sphere collider. Skeletons are programmed to shoot buildings and the tower wall. When an enemy dies, they have a certain chance of dropping a material where they die which the player can click on and use for upgrades. The camera is programmed to move whenever the player moves to a different village. There is a pause menu which pauses the game and allows the player to change the volume of the game and quit. For player movement to be consistent, the horizontal and vertical movement had to be adjusted based on axes relative to the current village the player inhabits.

Gameplay
Knight’s Kingdom requires the player to oversee three villages while all are attacked by the undead. Only one village can be seen at a time, forcing the player to be engaged by constantly checking on each village. The player will have to gather all the materials and XP they can and determine the best way to spend them. If the player only upgrades one village’s defenses, the other two will be overrun and the kingdom will fall. Materials are also not guaranteed to drop from enemies, meaning all upgrades are not guaranteed. Throughout the game, there are undead knights with high health that will test the DPS of the towers. Near the end of the onslaught, they will face a demon king with the highest health and damage in the game. If the player is able to defeat that final boss, they will have saved the kingdom and won the game.

Completeness
The game begins with a celebration of the kingdom’s 50th anniversary when suddenly, the undead attack. The king of the kingdom asks the legendary knight to defend the kingdom from these beasts. Afterwards, the king presents the tutorial, showing the player how to move, how the blacksmith and alchemist work, and the general objective of the game. Buttons appear to help the player with movement and shops and health bars appear over buildings when the player is close by. Upgrades improve as the game goes on and as the player collects iron, they are able to transform their towers to use cannon, providing a huge damage improvement over normal upgrades. The player watches over three villages and will face multiple waves of enemies, becoming increasingly difficult to deal with as time goes on. The controls consist of WASD for movement, pressing ‘E’ for shop interactions, and clicking on materials on the ground.

Intangibles
UI Elements found <a href=https://www.kenney.nl/assets/ui-pack-rpg-expansion>here</a> 
Fonts found <a href=https://fonts.google.com/specimen/Lilita+One?preview.text=Title%20of%20Game%20Castle%20Defense&preview.text_type=custom&category=Display)>here</a>, <a href=https://fonts.google.com/specimen/Sigmar?preview.text=%7C%7C&preview.text_type=custom&category=Display>here</a>, and <a href=https://fonts.google.com/specimen/Neucha?preview.text=Hello%20Brave%20Knight&preview.text_type=custom&category=Handwriting>here</a>

Sounds found:
[Arrow Hit Sound] (https://www.kenney.nl/assets/impact-sounds)
[Background Music] (https://www.fesliyanstudios.com/royalty-free-music/download/retro-forest/451)
[Boss Death] (https://opengameart.org/content/15-monster-gruntpaindeath-sounds)
[Boss Noises] (https://opengameart.org/content/big-scary-troll-sounds)
[Bow and Arrow Shot] (https://opengameart.org/content/bow-arrow-shot)
[Cannon] (https://opengameart.org/content/battle-at-sea)
[Dark Knight] (https://opengameart.org/content/15-monster-gruntpaindeath-sounds)
[Error Sound] (https://freesound.org/people/distillerystudio/sounds/327735/)
[Menu Click] (https://www.kenney.nl/assets/ui-audio)
[Skeleton Hit] (https://quicksounds.com/sound/8410/skeleton-footstep)
[Skeleton Death] (https://quicksounds.com/sound/8409/skeleton-footstep-4)
[Zombie Death] (https://opengameart.org/content/15-monster-gruntpaindeath-sounds)
[Zombie (Idle)] (https://opengameart.org/content/zombie-voice-sound-effect)
