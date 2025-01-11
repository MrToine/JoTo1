using Godot;
using System;

// Classe abstraite représentant une compétence dans le jeu.
// Sert de base pour toutes les compétences spécifiques.
public abstract class Skill {
    // Nombre de jours nécessaires pour débloquer la compétence
    [Export] public int DaysForUnlock { get; private set; }

    // Nom de la compétence
    [Export] public string Name { get; private set; }

    // Description détaillée de la compétence
    [Export] public string Description { get; private set; }

    // Élément associé à la compétence (Feu, Eau, etc.)
    [Export] public string Element { get; private set; }

    // Description de l'effet de la compétence
    [Export] public string Effect { get; private set; }

    // Portée de la compétence
    [Export] public float Scope { get; private set; }

    // Points de dégâts infligés par la compétence
    [Export] public int Degats { get; private set; }

    // Temps de recharge de la compétence en tours
    [Export] public int CountDown { get; private set; }

    public Skill(
        int daysForUnlock,
        string name, 
        string description,
        string element,
        string effect,
        float scope,
        int degats,
        int countDown
    ) {
        DaysForUnlock = daysForUnlock;
        Name = name;
        Description = description;
        Element = element;
        Effect = effect;
        Scope = scope;
        Degats = degats;
        CountDown = countDown;
    }

    // Applique l'effet de la compétence sur le joueur cible
    // @param player: Le joueur sur lequel appliquer l'effet
    public abstract void ApplyEffect(Player player);
}