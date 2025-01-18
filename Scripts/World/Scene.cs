using Godot;
using System;

public partial class Scene : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalState.Instance.CurrentStoryStep = 1;
		GD.Print("Etat d'avancement de l'histoire : " + GlobalState.Instance.CurrentStoryStep);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
