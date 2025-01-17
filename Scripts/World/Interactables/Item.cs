using Godot;
using System;

public partial class Item: InteractableObject, IInteractable
{
    public override void _PhysicsProcess(double delta) {
        if(_isInteracting) {
            GD.Print("L'objet est en train d'être ramassé...");
            GD.Print("Nom de l'objet : " + Name);
            GD.Print("Parent de l'objet : " + GetParent().Name);
            // On supprime l'objet du monde
            QueueFree();
            GD.Print("QueueFree() appelé sur l'objet : " + Name);
        }
    }

    public new void Interact() {
        GD.Print("Tu ramasse l'objet ");
        base.Interact();
    }

    public void _on_area_entered(Node body) {
        var player = body.GetParent();
        GD.Print(player);
        if (player is Player playerNode) {
            playerNode.SetCurrentInteractable(this);
        }
    }

    public void _on_area_exited(Node body) {
        var player = body.GetParent();
        if (player is Player playerNode) {
            playerNode.ClearCurrentInteractable();
        }
    }
}