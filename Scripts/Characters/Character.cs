using Godot;
using System;

public partial class Character : CharacterBody3D
{
	// Constantes pour les valeurs par défaut
	private const float DefaultSpeed = 1.75f;
	private const float DefaultExtraSpeed = 1.75f;
	private const float DefaultAcceleration = 1.5f;
	private const float DefaultAirAcceleration = 0.4f;
	private const float DefaultGravity = 0.5f;
	private const float MaxTerminalVelocity = 50f;
	private const float DefaultJumpForce = 20f;
	private const int DefaultMaxLifePoint = 50;
	private const int DefaultAttackPoint = 10;
	private const int DefaultDefensePoint = 5;

	// Propriétés exportées pour être modifiables dans l'éditeur Godot
	[Export] public float speed { get; set; } = DefaultSpeed;
	[Export] public float extraSpeed { get; set; } = DefaultExtraSpeed;
	[Export] public float acceleration { get; set; } = DefaultAcceleration;
	[Export] public float airAcceleration { get; set; } = DefaultAirAcceleration;
	[Export] public float gravity { get; set; } = DefaultGravity;
	[Export] public float maxTerminalVelocity { get; set; } = MaxTerminalVelocity;
	[Export] public float jumpForce { get; set; } = DefaultJumpForce;

	[Export] public int maxLifePoint { get; set; } = DefaultMaxLifePoint;
	[Export] public int attackPoint { get; set; } = DefaultAttackPoint;
	[Export] public int defensePoint { get; set; } = DefaultDefensePoint;

 	[Export] public AnimationPlayer animationPlayer;

	// Champs privés pour la gestion de la vélocité
	protected Vector3 velocity;
	protected float yVelocity;

	// Variable booléenne pour indiquer si le joueur peut rebondir
	public bool bounce = false;  // Par défaut, le personnage ne peut pas rebondir

	// Propriétés pour les points de vie, d'attaque et de défense
	public int lifePoint { get; set; }
}
