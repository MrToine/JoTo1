using Godot;
using System;

// Classe abstraite représentant une compétence dans le jeu.
// Sert de base pour toutes les compétences spécifiques.
public abstract partial class Skill : Node3D 
{
    // Nombre de jours nécessaires pour débloquer la compétence
    [Export (PropertyHint.Range, "0,100,1")] 
    public int DaysForUnlock { get; set; }

    // Nom de la compétence
    [Export] 
    public new string Name { get; set; }

    // Description détaillée de la compétence
    [Export (PropertyHint.MultilineText)] 
    public string Description { get; set; }

    // Élément associé à la compétence (Feu, Eau, etc.)
    [Export] 
    public string Element { get; set; }

    // Description de l'effet de la compétence
    [Export (PropertyHint.MultilineText)] 
    public string Effect { get; set; }

    // Portée de la compétence
    [Export (PropertyHint.Range, "0.1,10.0")] 
    public float Scope { get; set; }

    // Points de dégâts infligés par la compétence
    [Export (PropertyHint.Range, "0,100")] 
    public int Degats { get; set; }

    // Temps de recharge de la compétence en tours
    [Export (PropertyHint.Range, "0,10")] 
    public float CountDown { get; set; }

    public Skill(
        int daysForUnlock,
        string name, 
        string description,
        string element,
        string effect,
        float scope,
        int degats,
        float countDown
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