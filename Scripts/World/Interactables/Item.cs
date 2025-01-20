using Godot;
using System;

public partial class Item: InteractableObject, IInteractable
{
    public override void _PhysicsProcess(double delta) {
        if(_isInteracting) {
            QueueFree();
        }
    }

    public new void Interact() {
        base.Interact();
    }

    public void _on_area_entered(Node body) {
        var player = body.GetParent();
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