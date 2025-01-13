using Godot;
using System;

public partial class FireBall : FireSkill
{
    public FireBall() : base(
        daysForUnlock: 1,
        name: "Boule de Feu",
        description: "Lance une boule de feu enflammée sur l'ennemi",
        effect: "Inflige des dégâts de feu et peut brûler la cible",
        scope: 5.0f,
        degats: 25,
        countDown: 0.2f
    ) { }

    public override void ApplyEffect(Player player)
    {
        // Applique les dégâts de base
        // player.TakeDamage(Degats);
        
        // 30% de chance d'appliquer un effet de brûlure
        if (GD.Randf() < 0.3f)
        {
            // TODO: Implémenter l'effet de brûlure
            // player.ApplyBurnEffect(3); // 3 tours de brûlure
        }
    }
}
