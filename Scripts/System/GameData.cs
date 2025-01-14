using Godot;
using System.Collections.Generic;

public class GameData
{
    public Vector3 PlayerPosition { get; set; }
    // public List<string> Inventory { get; set; }
    public List<string> Skills { get; set; }
    public int LifePoint { get; set; }
    public int MaxLifePoint { get; set; }
    public int AttackPoint { get; set; }
    public int DefensePoint { get; set; }
    public int Day { get; set; }

    public GameData()
    {
        PlayerPosition = Vector3.Zero;
        // Inventory = new List<string>();
        Skills = new List<string>();
        LifePoint = 100;
        MaxLifePoint = 100;
        AttackPoint = 10;
        DefensePoint = 5;
        Day = 1;
    }
}