# The Idea
To start: just a simple dynamic economic sim with towns, merchants (maybe more agents?), and supply and demand pricing. 

# Code Structure
`src/logic` should handle all internal game logic and never make calls to godot namespace

`src/render` should handle all calls to godot for rendering


# Todo:

### fixes:
* fix food consumption so a starving town has 0 food instead of > 1 person's worth

### short term
* laborers consume non-food items
* bridges & roads
* bans, tarrifs, toll booths

### long term
* market ui: so player can buy/sell
* price report ui: give the player the ability to see prices everywhere