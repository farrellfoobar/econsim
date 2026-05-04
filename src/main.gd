extends Node


const READABLE_NAME_MODE = true;

var camera: Camera2D;

# Called when 'MainScene' is instaniated, which is instantly because 'MainScene' is marked as the main scene in godot
func _ready() -> void:
	var tileSet: TileSet = load("res://res/tiles/tileset.tres");
	
	var hexMap: TileMapLayer = TileMapLayer.new();
	self.add_child(hexMap);
	hexMap.set_tile_set(tileSet);

	hexMap.set_cell( Vector2i(0,0), 1, Vector2i(0, 0), 0 );
	hexMap.set_cell( Vector2i(1,0), 1, Vector2i(0, 0), 0 );
	hexMap.set_cell( Vector2i(1,1), 1, Vector2i(0, 0), 0 );
	hexMap.set_cell( Vector2i(2,1), 1, Vector2i(0, 0), 0 );


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
