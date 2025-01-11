using Godot;
using System;
using System.Collections.Generic;

// Gère l'arbre de compétences du joueur
// Permet d'ajouter et de débloquer de nouvelles compétences
public class SkillsTree {
    // Liste des compétences débloquées par le joueur
    public List<Skill> Skills { get; private set; }

    public SkillsTree() {
        Skills = new List<Skill>();
    }

    // Ajoute une nouvelle compétence à l'arbre
    // @param skill: La compétence à ajouter
    public void AddSkill(Skill skill) {
        Skills.Add(skill);
    }

    // Tente de débloquer une nouvelle compétence
    // @param skill: La compétence à débloquer
    // @return: true si la compétence a été débloquée, false si déjà débloquée
    public bool UnlockSkill(Skill skill) {
        if (Skills.Contains(skill)) {
            return false;
        }

        Skills.Add(skill);
        return true;
    }
}