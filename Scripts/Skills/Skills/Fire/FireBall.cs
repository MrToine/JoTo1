using Godot;
using System;

// Compétence spécifique de type Feu qui inflige des dégâts de feu à la cible
// Hérite de la classe parente FireSkill
public class FireBallSkill : FireSkill {
    public FireBallSkill() : base(
        0,
        "FireBall",
        "Une boule de feu",
        "Inflige des dégâts de feu",
        1.0f,
        10,
        3
    ) {}

    public override void ApplyEffect(Player player) {
        GD.Print("On fait un barbecue avec une boule de feu !");
    }
}