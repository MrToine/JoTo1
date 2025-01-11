using Godot;
using System;

// Compétence de type Feu
// Hérite de la classe de base Skill
public class FireSkill: Skill {
    public FireSkill(
        int daysForUnlock,
        string name,
        string description,
        string element,
        string effect,
        float scope,
        int degats,
        int countDown
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
}