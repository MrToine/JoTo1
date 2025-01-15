using System.Collections.Generic;
using System.Text.Json;
using Godot;

public partial class DialogueManager : Node3D
{
    private Dictionary<string, Dictionary<string, Dialogue>> _npcDialogues = new();

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
}