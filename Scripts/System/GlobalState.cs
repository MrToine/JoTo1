using Godot;
using System;

public partial class GlobalState : Node
{
    public int CurrentStoryStep { get; set; }

    private static GlobalState _instance;
    public static GlobalState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalState();
            }
            return _instance;
        }
    }

    public override void _Ready()
    {
        SetProcess(false);
        GD.Print("Le CurrentStoryStep est à " + CurrentStoryStep);
    }
}