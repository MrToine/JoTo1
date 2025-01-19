using Godot;
using System;

public partial class Teleport : Node3D
{
    // On défini l'export du fichier de la scene de destination
    [Export] public PackedScene DestinationScenePath;

	private string sceneName;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// On récupère le nom actuel de la scene à partir du nom du fichier
		sceneName = GetTree().CurrentScene.SceneFilePath;
	}

	public void _on_area_entered(Node body)
    {
        if (body.GetParent() is Player playerNode)
        {
            // On charge la scene de destination
            var destinationScene = DestinationScenePath.Instantiate() as Node3D;
            if (destinationScene == null)
            {
                GD.PrintErr("La scène de destination n'a pas pu être instanciée.");
                return;
            }

            // On ajoute la scene de destination au parent de la scene actuelle
            GetTree().Root.AddChild(destinationScene);
            GetTree().CurrentScene = destinationScene;

            // On déplace le joueur à la position de la scene de destination
			string destination = sceneName.Replace("res://", "").Replace(".tscn", "").Replace("Scenes/scene_", "");
            var spawnPoint = destinationScene.GetNode<Node3D>($"SpawnFromScene{destination}");
            if (spawnPoint == null)
            {
                GD.PrintErr($"Le point de spawn 'SpawnFrom{destination}' n'a pas été trouvé dans la scène de destination.");
                return;
            }

            playerNode.GlobalTransform = spawnPoint.GlobalTransform;

            // On supprime la scene actuelle
            GetTree().CurrentScene.QueueFree();
        }
    }
}
