## The Idea
To start: just a simple dynamic economic sim with towns, merchants (maybe more agents?), and supply and demand pricing. 

## Code Structure
`src/logic` should handle all internal game logic and never make calls to godot namespace

`src/render` should handle all calls to godot for rendering