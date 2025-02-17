using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Player : Character
{

	// Exporter la propriété mouseSensivity pour qu'elle soit modifiable dans l'éditeur Godot
	// Utiliser PropertyHint.Range pour définir une plage de valeurs de 0.1 à 1.0
	[Export(PropertyHint.Range, "0.1,1.0")]
	float mouseSensivity = 0.3f;  // Sensibilité de la souris, valeur par défaut 0.3

	// Exporter la propriété minPitch pour qu'elle soit modifiable dans l'éditeur Godot
	// Utiliser PropertyHint.Range pour définir une plage de valeurs de -90 à 0 avec un pas de 1
	[Export(PropertyHint.Range, "-90,0,1")]
	float minPitch = -90f;  // Inclinaison minimale (pitch), valeur par défaut -90

	// Exporter la propriété maxPitch pour qu'elle soit modifiable dans l'éditeur Godot
	// Utiliser PropertyHint.Range pour définir une plage de valeurs de 0 à 90 avec un pas de 1
	[Export(PropertyHint.Range, "0,90,1")]
	float maxPitch = 90f;  // Inclinaison maximale (pitch), valeur par défaut 90

	[Export(PropertyHint.Range, "0.5,50.0")]
	float animationSpeed = 1.0f;

	[Export]Node3D cameraPivot;
	[Export]Camera3D camera;

	private bool isAttacking = false;
	private bool isInDialogue = false; // Nouvelle propriété pour suivre l'état du dialogue
	
	private string currentAnimation = "";

	private new Vector3 self_position;

	private IInteractable _currentInteractable;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		/* Récupérer le noeud Camera3D et le noeud Node3D associé à la caméra au lancement du jeu */
		Input.MouseMode = Input.MouseModeEnum.Captured;
		animationPlayer.Play("Static_Animation");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel"))
		{
			/* Changer le mode de la souris entre Captured et Visible en appuyant sur echap */
			Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
		}
	}

	public override void _Input(InputEvent @event)
	{
		
		if(@event is InputEventMouseMotion motionEvent)
		{
			/* Récupérer les mouvements de la souris pour orienter la caméra */
			Vector3 rotDeg = RotationDegrees;
			rotDeg.Y -= motionEvent.Relative.X * mouseSensivity;
			RotationDegrees = rotDeg;
			
			rotDeg = cameraPivot.RotationDegrees;
			rotDeg.X -= motionEvent.Relative.Y * mouseSensivity;
			rotDeg.X = Mathf.Clamp(rotDeg.X, minPitch, maxPitch);
			cameraPivot.RotationDegrees = rotDeg;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		self_position = GlobalTransform.Origin;
		HandleMovement(delta);
	}

	private async void HandleMovement(double delta) {
		Vector3 direction = new Vector3(0, 0, 0);

		if(Input.IsActionPressed("move_up")) {
			direction += Transform.Basis.X;
		}
		if(Input.IsActionPressed("move_down")) {
			direction -= Transform.Basis.X;
		}
		if(Input.IsActionPressed("move_left")) {
			direction -= Transform.Basis.Z;
		}
		if(Input.IsActionPressed("move_right")) {
			direction += Transform.Basis.Z;
		}

		if(Input.IsActionJustPressed("attack")) {
			Attack();
		}

		if(Input.IsActionPressed("run")) {
			this.extraSpeed = 3f;
		}else{
			this.extraSpeed = 1f;
		}

		if(Input.IsActionJustPressed("interact")){
			InteractWithObject();
		}

		/********************************************************
		*		  												*
		*		TOUCHES DESTINÉES AU GODMOD ET AUX TESTS		*
		*														*
		********************************************************/

		if(Input.IsActionJustPressed("GMsave")) {
			// On fait appel à la méthode SaveGame de la classe Data pour sauvegarder les données du joueur
			GD.Print("Sauvegarde en cours...");
			Data.SaveGame(new GameData {
				PlayerPosition = self_position,
				Skills = new List<string> { "Boule de feu", "Gelure" },
				LifePoint = 100,
				MaxLifePoint = 100,
				AttackPoint = 10,
				DefensePoint = 5,
				Day = 1
			});
		}

		if(Input.IsActionJustPressed("GMload")) {
			// On fait appel à la méthode LoadGame de la classe Data pour charger les données du joueur
			GD.Print("Chargement en cours...");
			var gameData = Data.LoadGame();
			if(gameData != null) {
				self_position = gameData.PlayerPosition;
				// On téléporte le joueur à la position sauvegardée
				GlobalTransform = new Transform3D(Basis.Identity, self_position);
				GD.Print("Position du joueur : ", self_position);
			}
		}

		/* =================================================== */

		direction = direction.Normalized();

		float accel = IsOnFloor() ? this.acceleration : this.airAcceleration;
		this.velocity = direction * this.speed * accel * this.extraSpeed;

		if(this.bounce) {
			yVelocity = this.jumpForce;
			this.bounce = false;
		} else {
			if(IsOnFloor()) {
				yVelocity = -0.01f;
			} else {
				yVelocity = Mathf.Clamp(yVelocity - this.gravity, -this.maxTerminalVelocity, this.maxTerminalVelocity);
			}
		}

		velocity.Y = yVelocity;
		Velocity = velocity;
		MoveAndSlide();

		// for(int i = 0; i < GetSlideCollisionCount(); i++) {
		// 	var collision = GetSlideCollision(i);
		// 	//GD.Print("Collided with: ", (Node)collision.GetCollider());
		// }

		if(!isAttacking) {
			if(direction != new Vector3(0,0,0) && IsOnFloor()) {
				PlayerAnimation("Walk");
			}else if(direction == new Vector3(0, 0, 0) && IsOnFloor()) {

				animationPlayer.Play("Idle_Think");
			}
		}
	}

	private void Attack() {
		// Vérifie si le joueur n'est pas en dialogue avant de permettre l'attaque
		if (!isAttacking && !isInDialogue) {
			isAttacking = true;
			PlayerAttackAnimation();
		}
	}

	private void PlayerAttackAnimation() {
		currentAnimation = "Attaque";
		animationPlayer.Play("Attaque");
		animationPlayer.GetAnimation("Attaque").LoopMode = Animation.LoopModeEnum.Linear;
		
		// Attendre la fin de l'animation avant de permettre à nouveau les attaques
		var timer = GetTree().CreateTimer(animationPlayer.GetAnimation("Attaque").Length);
		timer.Timeout += () => {
			isAttacking = false;
		};
	}

	private void PlayerAnimation(string animationName) {
		if(currentAnimation == animationName && this.animationPlayer.IsPlaying()) {
			return;
		}

		currentAnimation = animationName;
		 // Joue directement l'animation en boucle
		animationPlayer.Play(animationName);
		// Force le mode de lecture en boucle
		animationPlayer.GetAnimation(animationName).LoopMode = Animation.LoopModeEnum.Linear;

		// Définit la vitesse de l'animation
		animationPlayer.SpeedScale = animationSpeed;

	}

	private void InteractWithObject() {
		_currentInteractable?.Interact();
	}

	public void SetCurrentInteractable(IInteractable interactable) {
		// Permet de définir l'objet actuellement en interaction
		// pour pouvoir interagir avec lui en appuyant sur une touche
		_currentInteractable = interactable;
	}

	public void ClearCurrentInteractable() {
		// Permet de supprimer l'objet actuellement en interaction
		// pour ne plus pouvoir interagir avec lui
		_currentInteractable = null;
	}

	// Méthodes publiques pour gérer l'état du dialogue
	public void StartDialogue() {
		isInDialogue = true;
	}

	public async void EndDialogue() {
		// On fait un await de 100ms 
		await Task.Delay(100);
		isInDialogue = false;
	}
}
