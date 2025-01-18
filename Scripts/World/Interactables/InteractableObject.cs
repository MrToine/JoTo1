using Godot;
using System;

public enum InteractableObjectType {
    Item,
    Chest,
    Door,
    Sign,
    Switch
}

public enum InteractableObjectState {
    Used,
    Unused,
    Opened,
    Closed,
    Locked,
    Unlocked
}

public enum InteractableObjectEffect {
    None,
    Heal,
    Damage,
    Teleport,
    Open,
    Close,
    Lock,
    Unlock
}

// Classe abstraite représentant un objet interactif dans le jeu.
// Sert de base pour tous les objets interactifs spécifiques.
public abstract partial class InteractableObject: Node3D
{
    private const int NoHistoryStep = -1; // Constante pour indiquer qu'il n'y a pas d'avancement de l'histoire

    [Export] public string Name { get; set; }
    [Export] public InteractableObjectType Type { get; set; }
    [Export] public InteractableObjectEffect Effect { get; set; }
    [Export] public int Value { get; set; }
    [Export (PropertyHint.MultilineText)] public string Description { get; set; }
    [Export] public int ChangeHistoryStep { get; set; } = NoHistoryStep; // Utilisation de la constante

    [Signal] public delegate void InteractedEventHandler();

    protected bool _isInteracting = false;

    public override void _Ready() {
        
        AddToGroup("game_elements");
    }

    public void Interact() {
        if (!_isInteracting) {
            _isInteracting = true;
            GlobalState.Instance.CurrentStoryStep = ChangeHistoryStep; // Changer l'état de l'histoire
            EmitSignal(nameof(Interacted)); // Émettre un signal à chaque interaction
        }
    }
}