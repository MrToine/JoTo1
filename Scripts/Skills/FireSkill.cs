using Godot;
using System;

// Compétence de type Feu
// Hérite de la classe de base Skill
public partial class FireSkill: Skill {
    public FireSkill(
        int daysForUnlock,
        string name,
        string description,
        string effect,
        float scope,
        int degats,
        float countDown
    ): base(
        daysForUnlock,
        name,
        description,
        "Fire",
        effect,
        scope,
        degats,
        countDown
    ) {}

    public override void ApplyEffect(Player player) {}
}