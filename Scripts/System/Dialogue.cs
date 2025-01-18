using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Dialogue
{
    public string Id { get; set; }

    [JsonPropertyName("conditionStep")]
    public int ConditionStep { get; set; }

    public string Speaker { get; set; }
    public string Text { get; set; }
    public Dictionary<string, string> Choices { get; set; }

    private DialogueManager _dialogueManager;
    private string _npcName;
}