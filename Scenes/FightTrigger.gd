extends Area2D

var maxCreatureID = 0
var CreatureID = 0

var CSharpComm: Object
# Called when the node enters the scene tree for the first time.
func _ready():
	var instance = load("res://Logic/DB/gdsDbWorkerInterface.cs").new()
	instance.executeMethod()
	self.maxCreatureID = instance.buffer
	
	self.CreatureID = self.get_meta("CreatureID")
	print(instance.buffer)
	print("init trigger with id: %s" % self.CreatureID)

	body_shape_entered.connect(on_body_enter)

	self.CSharpComm = instance


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func on_body_enter(body_rid: RID, body: Node2D, body_shape_index: int, local_shape_index: int):
	print("About to engage. Creature id = %s" % self.CreatureID)

	var arena = load("res://Scenes/Arena.tscn")
	arena.set_meta("OpponentId", self.CreatureID)
	
	CSharpComm.SetOpponentCreature(self.CreatureID)

	get_tree().change_scene_to_packed(arena)
