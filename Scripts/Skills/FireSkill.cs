using Godot;
using System;

// Compétence de type Feu qui inflige des dégâts de feu à la cible
// Hérite de la classe de base Skill
public class FireSkill: Skill {
    public FireSkill(): base(
        0,
        "FireBall",
        "Une boule de feu",
        "Fire",
        "Inflige des dégâts de feu",
        1.0f,
        10,
        3
    ) {}

    public override void ApplyEffect(Player player) {
        GD.Print("On fait un barbecue !");
    }
}