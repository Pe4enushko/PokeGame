extends CharacterBody2D
const SPEED = 5

func _physics_process(delta):
	var direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	move_and_collide(direction * SPEED)
