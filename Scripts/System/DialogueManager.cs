using System.Collections.Generic;
using System.Text.Json;
using Godot;

public partial class DialogueManager : Node3D
{
	private Dictionary<string, Dictionary<string, Dialogue>> _npcDialogues = new();
	private Control _dialogueUI;
	private Label _dialogueText;
	private VBoxContainer _choicesContainer;
	private Camera3D _camera;
	private string _currentNpcName;

	// Charge les dialogues pour un PNJ spécifique
	public void LoadDialogue(string npcName)
	{
		_currentNpcName = npcName;
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
			var dialogue = _npcDialogues[npcName][dialogueId];
			if (dialogue.ConditionStep <= GlobalState.Instance.CurrentStoryStep) {
				return dialogue;
			}
		}

		GD.PrintErr($"Dialogue {dialogueId} introuvable pour le PNJ {npcName}.");
		return null;
	}

	public void ShowDialogue(Dialogue dialogue)
	{
		var player = GetNode<Player>("../PlayerCtrl");
		if (player != null)
		{
			player.StartDialogue();
			player.SetProcessInput(false);
			player.SetProcess(false);
			player.SetPhysicsProcess(false);
		}

		// Désactive le pivot de la caméra
		var camPivot = GetNode<Node3D>("../PlayerCtrl/CamPivot");
		if (camPivot != null)
		{
			camPivot.SetProcessInput(false);
			camPivot.SetProcess(false);
		}

		// Active le mode souris
		Input.MouseMode = Input.MouseModeEnum.Visible;

		_dialogueText.Text = dialogue.Text;
		foreach (Node child in _choicesContainer.GetChildren())
		{
			child.QueueFree();
		}

		bool hasValidChoices = false;
		foreach (var choice in dialogue.Choices) {
			var nextDialogue = GetDialogue(_currentNpcName, choice.Key);
			if (nextDialogue != null) {
				var button = new Button();
				button.Text = choice.Value;
				button.Connect("pressed", Callable.From(() => OnChoiceSelected(choice.Key)));
				_choicesContainer.AddChild(button);
				hasValidChoices = true;
			}
		}

		if (!hasValidChoices) {
			var button = new Button();
			button.Text = "Fermer";
			button.Connect("pressed", Callable.From(() => HideDialogue()));
			_choicesContainer.AddChild(button);
		}

		_dialogueUI.Visible = true;
	}

	public void HideDialogue()
	{
		var player = GetNode<Player>("../PlayerCtrl");
		if (player != null)
		{
			player.EndDialogue();
			player.SetProcessInput(true);
			player.SetProcess(true);
			player.SetPhysicsProcess(true);
		}

		// Réactive le pivot de la caméra
		var camPivot = GetNode<Node3D>("../PlayerCtrl/CamPivot");
		if (camPivot != null)
		{
			camPivot.SetProcessInput(true);
			camPivot.SetProcess(true);
		}

		// Réactive tous les autres éléments de jeu
		foreach (Node node in GetTree().GetNodesInGroup("game_elements"))
		{
			if (node is Control control)
			{
				control.SetProcessInput(true);
			}
		}
		
		// Réactive complètement la caméra
		if (_camera != null)
		{
			_camera.SetProcessInput(true);
			_camera.SetProcessMode(ProcessModeEnum.Inherit);
		}

		// Recapture la souris
		Input.MouseMode = Input.MouseModeEnum.Captured;

		_dialogueUI.Visible = false;
	}

	private void OnChoiceSelected(string nextDialogueId)
	{
		var nextDialogue = GetDialogue(_currentNpcName, nextDialogueId);
		if (nextDialogue != null)
		{
			ShowDialogue(nextDialogue);
		}
		else
		{
			HideDialogue();
		}
	}

	public void _on_dialogue_ui_ready()
	{
		_dialogueUI = GetNode<Control>("../DialogueUI");
		if (_dialogueUI == null)
		{
			GD.PrintErr("DialogueUI not found!");
			return;
		}

		_dialogueText = _dialogueUI.GetNode<Label>("Panel/VBoxContainer/DialogueText");
		if (_dialogueText == null)
		{
			GD.PrintErr("DialogueText not found!");
			return;
		}

		_choicesContainer = _dialogueUI.GetNode<VBoxContainer>("Panel/VBoxContainer/ChoicesContainer");
		if (_choicesContainer == null)
		{
			GD.PrintErr("ChoicesContainer not found!");
			return;
		}

		_camera = GetNode<Camera3D>("../PlayerCtrl/CamPivot/SpringArm3D/Camera3D");
		if (_camera == null)
		{
			GD.PrintErr("Camera3D not found!");
			return;
		}

		_dialogueUI.Visible = false;
	}
}
