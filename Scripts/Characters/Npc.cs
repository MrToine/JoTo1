using Godot;
using System;
using System.Collections.Generic;

public partial class Npc : Character, IInteractable
{	
    [Export] public string NpcName { get; set; }

    private DialogueManager _dialogueManager;

    public override void _Ready()
    {
        // Gestion animationtree
        // GetNode<AnimationTree>("../[ManaGolem]EarthQuake/AnimationPlayer/AnimationTree").Active = false;

        _dialogueManager = GetNode<DialogueManager>("../DialogueManager");
        if (_dialogueManager == null)
        {
            GD.Print("DialogueManager not found!");
            return;
        }
        
        _dialogueManager.LoadDialogue(NpcName);

        var area = GetNode<Area3D>("AreaCollider");
        area.Connect("body_entered", Callable.From((Node body) => _on_area_entered(body)));
        area.Connect("body_exited", Callable.From((Node body) => _on_area_exited(body)));
        base._Ready();
    }

    public void StartDialogue() {
        GD.Print($"Dialogue avec le PNJ {NpcName}");
        var dialogue = _dialogueManager.GetDialogue(NpcName, "start");
    
        if (dialogue != null) {
            _dialogueManager.ShowDialogue(dialogue);
        } else {
            GD.PrintErr($"Le dialogue n'existe pas pour le PNJ {NpcName}");
        }
    }

    public void Interact()
    {
        GD.Print("Interaction avec le PNJ " + Name);
        StartDialogue();
    }

	public void _on_area_entered(Node body) {
		GD.Print("Collision avec " + body.Name);
		if (body is Player playerNode) {
			GD.Print("Le joueur est proche du PNJ " + Name);
            playerNode.SetCurrentInteractable(this);
		}
	}

	public void _on_area_exited(Node body) {
		if (body is Player playerNode) {
			GD.Print("Le joueur s'éloigne du PNJ " + Name);
            playerNode.ClearCurrentInteractable();
		}
	}
}
