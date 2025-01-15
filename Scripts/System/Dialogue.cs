using Godot;
using System;

public partial class Dialogue : Node
{
    public string Id { get; set; }
    public string Speaker { get; set; }
    public string Text { get; set; }
    public string[] Choices { get; set; }

    private DialogueManager _dialogueManager;
    private string _npcName;
    
    public override void _Ready()
    {
        _dialogueManager = GetNode<DialogueManager>("/root/DialogueManager");
        _npcName = Name;
        _dialogueManager.LoadDialogue(_npcName);
    }
}