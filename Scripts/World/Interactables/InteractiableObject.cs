using Godot;
using System;

public enum InteractableObjectType {
    Item,
    Chest,
    Door,
    Sign,
    Switch
}
// Classe abstraite représentant un objet interactif dans le jeu.
// Sert de base pour tous les objets interactifs spécifiques.
public abstract partial class InteractableObject: Node3D
{
    [Export] public string Name { get; set; }
    [Export] public InteractableObjectType Type { get; set; }
    [Export (PropertyHint.MultilineText)] public string Description { get; set; }

    [Signal] public delegate void InteractedEventHandler();

    protected bool _isInteracting = false;

    public void Interact() {
        if (!_isInteracting) {
            _isInteracting = true;
            EmitSignal(nameof(Interacted)); // Émettre un signal à chaque interaction
        }
    }
}