using System.Collections.Generic;
using System.Text.Json;
using Godot;

public partial class DialogueManager : Node3D
{
    private Dictionary<string, Dictionary<string, Dialogue>> _npcDialogues = new();
    private Control _dialogueUI;
    private Label _dialogueText;
    private VBoxContainer _choicesContainer;

    public override void _Ready()
    {
        _dialogueUI = GetNode<Control>("../DialogueUI");
        _dialogueText = _dialogueUI.GetNode<Label>("Panel/VBoxContainer/DialogueText");
        _choicesContainer = _dialogueUI.GetNode<VBoxContainer>("Panel/VBoxContainer/ChoicesContainer");
        _dialogueUI.Visible = false;
        GD.Print("DialogueManager est prêt à l'emploi.");
    }

    // Charge les dialogues pour un PNJ spécifique
    public void LoadDialogue(string npcName)
    {
        string path = $"res://Datas/Dialogues/{npcName}.json";
        if (!FileAccess.FileExists(path))
        {
            GD.PrintErr($"Le fichier {npcName} est introuvable. <{path}>.");
            return;
        }

        try
        {
            using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            string jsonText = file.GetAsText();
            var dialogues = JsonSerializer.Deserialize<Dictionary<string, Dialogue>>(jsonText);
            _npcDialogues[npcName] = dialogues;
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"Erreur lors du chargement du fichier {npcName}.json : {e.Message}");
        }
    }

    // Récupère un dialogue spécifique pour un PNJ donné
    public Dialogue GetDialogue(string npcName, string dialogueId)
    {
        if (_npcDialogues.ContainsKey(npcName) && _npcDialogues[npcName].ContainsKey(dialogueId))
        {
            return _npcDialogues[npcName][dialogueId];
        }

        GD.PrintErr($"Dialogue {dialogueId} introuvable pour le PNJ {npcName}.");
        return null;
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        // On fige la camera sans mettre le jeu en pause
        foreach (Node node in GetTree().GetNodesInGroup("game_elements"))
        {
            if (node is Control control)
            {
                control.SetProcessInput(false);
            }
        }

        // On laisse la souris visible
        Input.MouseMode = Input.MouseModeEnum.Visible;

        _dialogueText.Text = dialogue.Text;
        foreach (Node child in _choicesContainer.GetChildren())
        {
            child.QueueFree();
        }

        foreach (var choice in dialogue.Choices)
        {
            var button = new Button();
            button.Text = choice;
            button.Connect("pressed", Callable.From(() => OnChoiceSelected(choice)));
            _choicesContainer.AddChild(button);
        }

        _dialogueUI.Visible = true;
    }

    public void HideDialogue()
    {
        // On défige la camera
        foreach (Node node in GetTree().GetNodesInGroup("game_elements"))
        {
            if (node is Control control)
            {
                control.SetProcessInput(true);
            }
        }
        
        // On laisse la souris invisible
        Input.MouseMode = Input.MouseModeEnum.Captured;

        _dialogueUI.Visible = false;
    }

    private void OnChoiceSelected(string choice)
    {
        GD.Print($"Choix sélectionné : {choice}");
        HideDialogue();
    }
}