# Gambler's Dice

## Core mechanics
This is a very simple dice game that is found in **Kingdom Come: Deliverance 2**. I found it very fun to play and would often take time away from quests just to play dice with random NPCs and try my best to find all of the various weighted die that can be found. It will first be just the player playing against nobody, but later will feature PVE and possibly PVP. I want to see if I can program a working NPC AI that makes good decisions based off of the roll that they got. The core things I want for right now are:
- 6 simulated dice rolls
- Working point system
- Working bust system
- Graphical dice rolling with physics
- Intuitive selection of dice for points

## Later Implementations
- NPC behavior that picks dice for points
- Easy Medium and Hard difficulty of AI
- Adding a 2 player system
- Make everything look pretty

## Rules
```
1 = 100                    Each additional die after 3 doubles the score:
5 = 50                       2 + 2 + 2 + 2 = 400
1 + 1 + 1 = 1000             2 + 2 + 2 + 2 + 2 = 800
2 + 2 + 2 = 200            
3 + 3 + 3 = 300
4 + 4 + 4 = 400              1 + 2 + 3 + 4 + 5 + 6 = 1500
5 + 5 + 5 = 500              2 + 3 + 4 + 5 + 6 = 750
6 + 6 + 6 = 600              1 + 2 + 3 + 4 + 5 = 500
