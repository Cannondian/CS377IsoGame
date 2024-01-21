// To switch your project to using the new InputSystem:
// Edit>Project Settings>Player>Active Input Handling change to "Input System Package (New)".

using UnityEngine;
using RPGCharacterAnims.Actions;
using RPGCharacterAnims.Extensions;
using RPGCharacterAnims.Lookups;
// Requires installing the InputSystem Package from the Package Manager: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/Installation.html
using UnityEngine.InputSystem;

namespace RPGCharacterAnims
{
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/index.html")]

	public class RPGCharacterInputSystemController : MonoBehaviour
    {
        RPGCharacterController rpgCharacterController;

		//InputSystem
		public @RPGInputs rpgInputs;

		// Inputs.
		private bool inputJump;
        private bool inputLightHit;
        private bool inputDeath;
        private bool inputAttackL;
        private bool inputAttackR;
        private bool inputCastL;
        private bool inputCastR;
		private bool inputBlock;
		private bool inputRoll;
		private bool inputShield;
		private bool inputRelax;
		private bool inputAim;
		private Vector2 inputMovement;
		private bool inputFace;
		private Vector2 inputFacing;
		private bool inputSwitchUp;
		private bool inputSwitchDown;
		private bool inputSwitchLeft;
		private bool inputSwitchRight;

		// Variables.
		private Vector3 moveInput;
		private Vector3 currentAim;
		private float bowPull;
		private bool blockToggle;
		private float inputPauseTimeout = 0;
		private bool inputPaused = false;

		private void Awake()
        {
            rpgCharacterController = GetComponent<RPGCharacterController>();
			rpgInputs = new @RPGInputs();
			currentAim = Vector3.zero;
        }

		private void OnEnable()
		{ rpgInputs.Enable(); }

		private void OnDisable()
		{ rpgInputs.Disable(); }

		public bool HasMoveInput() => moveInput.magnitude > 0.1f;

		public bool HasAimInput() => inputAim;

		public bool HasFacingInput() => inputFacing != Vector2.zero || inputFace;

		public bool HasBlockInput() => inputBlock;

		private void Update()
		{
			// Pause input for other external input.
			if (inputPaused) {
				if (Time.time > inputPauseTimeout) { inputPaused = false; }
				else { return; }
			}

			if (!inputPaused) { Inputs(); }

			Blocking();
			Moving();
			Jumping();
			Damage();
			SwitchWeapons();

			if (!rpgCharacterController.IsActive("Relax")) {
				Strafing();
				Facing();
				Aiming();
				Rolling();
				Attacking();
			}
		}

		/// <summary>
		/// Pause input for a number of seconds.
		/// </summary>
		/// <param name="timeout">The amount of time in seconds to ignore input.</param>
		public void PauseInput(float timeout)
		{
			inputPaused = true;
			inputPauseTimeout = Time.time + timeout;
		}

		/// <summary>
		/// Input abstraction for easier asset updates using outside control schemes.
		/// </summary>
		private void Inputs()
        {
            try {
				inputAttackL = rpgInputs.RPGCharacter.AttackL.WasPressedThisFrame();
				inputAttackR = rpgInputs.RPGCharacter.AttackR.WasPressedThisFrame();
				inputBlock = rpgInputs.RPGCharacter.Block.IsPressed();
				inputCastL = rpgInputs.RPGCharacter.CastL.WasPressedThisFrame();
				inputCastR = rpgInputs.RPGCharacter.CastR.WasPressedThisFrame();
				inputDeath = rpgInputs.RPGCharacter.Death.WasPressedThisFrame();
				inputFace = rpgInputs.RPGCharacter.Face.IsPressed();
				inputFacing = rpgInputs.RPGCharacter.Facing.ReadValue<Vector2>();
				inputJump = rpgInputs.RPGCharacter.Jump.IsPressed();
				inputLightHit = rpgInputs.RPGCharacter.LightHit.WasPressedThisFrame();
				inputMovement = rpgInputs.RPGCharacter.Move.ReadValue<Vector2>();
				inputRelax = rpgInputs.RPGCharacter.Relax.WasPressedThisFrame();
				inputRoll = rpgInputs.RPGCharacter.Roll.WasPressedThisFrame();
				inputShield = rpgInputs.RPGCharacter.Shield.WasPressedThisFrame();
				inputAim = rpgInputs.RPGCharacter.Aim.IsPressed();
				inputSwitchDown = rpgInputs.RPGCharacter.WeaponDown.WasPressedThisFrame();
				inputSwitchLeft = rpgInputs.RPGCharacter.WeaponLeft.WasPressedThisFrame();
				inputSwitchRight = rpgInputs.RPGCharacter.WeaponRight.WasPressedThisFrame();
				inputSwitchUp = rpgInputs.RPGCharacter.WeaponUp.WasPressedThisFrame();

                // Headlook toggle.
                if (rpgInputs.RPGCharacter.ToggleHeadLook.IsPressed())
				{ rpgCharacterController.ToggleHeadlook(); }

				// Injury toggle.
				if (rpgInputs.RPGCharacter.ToggleInjury.IsPressed()) {
                    if (rpgCharacterController.CanStartAction("Injure"))
					{ rpgCharacterController.StartAction("Injure"); }
					else if (rpgCharacterController.CanEndAction("Injure"))
					{ rpgCharacterController.EndAction("Injure"); }
                }
                // Pause toggle.
                if (rpgInputs.RPGCharacter.TogglePause.IsPressed()) {
                    if (rpgCharacterController.CanStartAction("SlowTime"))
					{ rpgCharacterController.StartAction("SlowTime", 0f); }
					else if (rpgCharacterController.CanEndAction("SlowTime"))
					{ rpgCharacterController.EndAction("SlowTime"); }
                }
                // Slow time toggle.
                if (rpgInputs.RPGCharacter.ToggleSlowTime.IsPressed()) {
                    if (rpgCharacterController.CanStartAction("SlowTime"))
					{ rpgCharacterController.StartAction("SlowTime", 0.125f); }
					else if (rpgCharacterController.CanEndAction("SlowTime"))
					{ rpgCharacterController.EndAction("SlowTime"); }
                }
            }
			catch (System.Exception) { Debug.LogError("Inputs not found!  Character must have Player Input component."); }
        }

		public void Blocking()
        {
            bool blocking = HasBlockInput();
            if (blocking && rpgCharacterController.CanStartAction("Block")) {
                rpgCharacterController.StartAction("Block");
				blockToggle = true;
            }
			else if (!blocking && blockToggle && rpgCharacterController.CanEndAction("Block")) {
                rpgCharacterController.EndAction("Block");
				blockToggle = false;
            }
        }

        public void Moving()
		{
			moveInput = new Vector3(inputMovement.x, inputMovement.y, 0f);

			// Filter the 0.1 threshold of HasMoveInput.
			if (HasMoveInput()) { rpgCharacterController.SetMoveInput(moveInput); }
			else { rpgCharacterController.SetMoveInput(Vector3.zero); }
		}

		private void Jumping()
		{
			// Set the input on the jump axis every frame.
			Vector3 jumpInput = inputJump ? Vector3.up : Vector3.zero;
			rpgCharacterController.SetJumpInput(jumpInput);

			// If we pressed jump button this frame, jump.
			if (inputJump && rpgCharacterController.CanStartAction("Jump")) { rpgCharacterController.StartAction("Jump"); }
			else if (inputJump && rpgCharacterController.CanStartAction("DoubleJump")) { rpgCharacterController.StartAction("DoubleJump"); }
		}

		public void Rolling()
		{
			if (!inputRoll) { return; }
			if (!rpgCharacterController.CanStartAction("DiveRoll")) { return; }

			rpgCharacterController.StartAction("DiveRoll", 1);
		}

		private void Aiming()
		{
			if (rpgCharacterController.hasAimedWeapon) {
				if (rpgCharacterController.HandlerExists(HandlerTypes.Aim)) {
					if (HasAimInput()) { rpgCharacterController.TryStartAction(HandlerTypes.Aim); }
					else { rpgCharacterController.TryEndAction(HandlerTypes.Aim); }
				}
				if (rpgCharacterController.rightWeapon == Weapon.TwoHandBow) {

					// If using the bow, we want to pull back slowly on the bow string while the
					// Left Mouse button is down, and shoot when it is released.
					if (Mouse.current.leftButton.isPressed) { bowPull += 0.05f; }
					else if (Mouse.current.leftButton.wasReleasedThisFrame) {
						if (rpgCharacterController.HandlerExists(HandlerTypes.Shoot))
						{ rpgCharacterController.TryStartAction(HandlerTypes.Shoot); }
					}
					else { bowPull = 0f; }
					bowPull = Mathf.Clamp(bowPull, 0f, 1f);
				}
				else {
					// If using a gun or a crossbow, we want to fire when the left mouse button is pressed.
					if (rpgCharacterController.HandlerExists(HandlerTypes.Shoot)) {
						if (Mouse.current.leftButton.isPressed) { rpgCharacterController.TryStartAction(HandlerTypes.Shoot); }
					}
				}
				// Reload.
				if (rpgCharacterController.HandlerExists(HandlerTypes.Reload)) {
					if (Mouse.current.rightButton.isPressed) { rpgCharacterController.TryStartAction(HandlerTypes.Reload); }
				}
				// Finally, set aim location and bow pull.
				rpgCharacterController.SetAimInput(rpgCharacterController.target.position);
				rpgCharacterController.SetBowPull(bowPull);
			}
			else { Strafing(); }
		}

		private void Strafing()
		{
			if (rpgCharacterController.canStrafe) {
				if (!rpgCharacterController.hasAimedWeapon) {
					if (inputAim) {
						if (rpgCharacterController.CanStartAction("Strafe")) { rpgCharacterController.StartAction("Strafe"); }
					}
					else {
						if (rpgCharacterController.CanEndAction("Strafe")) { rpgCharacterController.EndAction("Strafe"); }
					}
				}
			}
		}

		private void Facing()
		{
			if (rpgCharacterController.canFace) {
				if (HasFacingInput()) {
					if (inputFace) {

						// Get world position from mouse position on screen and convert to direction from character.
						Plane playerPlane = new Plane(Vector3.up, transform.position);
						Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
						float hitdist = 0.0f;
						if (playerPlane.Raycast(ray, out hitdist)) {
							Vector3 targetPoint = ray.GetPoint(hitdist);
							Vector3 lookTarget = new Vector3(targetPoint.x - transform.position.x, transform.position.z - targetPoint.z, 0);
							rpgCharacterController.SetFaceInput(lookTarget);
						}
					}
					else { rpgCharacterController.SetFaceInput(new Vector3(inputFacing.x, inputFacing.y, 0)); }

					if (rpgCharacterController.CanStartAction("Face")) { rpgCharacterController.StartAction("Face"); }
				}
				else {
					if (rpgCharacterController.CanEndAction("Face")) { rpgCharacterController.EndAction("Face"); }
				}
			}
		}

		private void Attacking()
		{
			// Check to make sure Attack and Cast Actions exist.
			if (!rpgCharacterController.HandlerExists(HandlerTypes.Attack)
				&& rpgCharacterController.HandlerExists(HandlerTypes.AttackCast)) { return; }

			// If already casting, stop casting.
			if ((inputCastL || inputCastR) && rpgCharacterController.IsActive(HandlerTypes.AttackCast)) {
				rpgCharacterController.EndAction(HandlerTypes.AttackCast);
				return;
			}

			// Check to make character can Attack.
			if (!rpgCharacterController.CanStartAction(HandlerTypes.Attack)) { return; }

			if (inputAttackL)
			{ rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext(HandlerTypes.Attack, Side.Left)); }
			else if (inputAttackR)
			{ rpgCharacterController.StartAction(HandlerTypes.Attack, new AttackContext(HandlerTypes.Attack, Side.Right)); }
			else if (inputCastL)
			{ rpgCharacterController.StartAction(HandlerTypes.AttackCast, new AttackCastContext(AnimationVariations.AttackCast.TakeRandom(), Side.Left)); }
			else if (inputCastR)
			{ rpgCharacterController.StartAction(HandlerTypes.AttackCast, new AttackCastContext(AnimationVariations.AttackCast.TakeRandom(), Side.Right)); }
		}

		private void Damage()
		{
			// Hit.
			if (inputLightHit) { rpgCharacterController.StartAction("GetHit", new HitContext()); }

			// Death.
			if (inputDeath) {
				if (rpgCharacterController.CanStartAction("Death")) { rpgCharacterController.StartAction("Death"); }
				else if (rpgCharacterController.CanEndAction("Death")) { rpgCharacterController.EndAction("Death"); }
			}
		}

		/// <summary>
		/// Cycle weapons using directional pad input. Up and Down cycle forward and backward through
		/// the list of two handed weapons. Left cycles through the left hand weapons. Right cycles through
		/// the right hand weapons.
		/// </summary>
		private void SwitchWeapons()
		{
			// Check to make sure SwitchWeapon Action exists.
			if (!rpgCharacterController.HandlerExists(HandlerTypes.SwitchWeapon)) { return; }

			// Bail out if we can't switch weapons.
			if (!rpgCharacterController.CanStartAction(HandlerTypes.SwitchWeapon)) { return; }

			// Switch to Relaxed.
			if (inputRelax) {
				rpgCharacterController.StartAction(HandlerTypes.Relax);
				return;
			}

			var doSwitch = false;
			var context = new SwitchWeaponContext();
			var weaponNumber = Weapon.Unarmed;

			// Switch to Shield.
			if (inputShield) {
				doSwitch = true;
				context.side = "Left";
				context.type = "Switch";
				context.leftWeapon = Weapon.Shield;
				context.rightWeapon = Weapon.Relax;
				rpgCharacterController.StartAction(HandlerTypes.SwitchWeapon, context);
				return;
			}

			// Cycle through 2Handed weapons if any input happens on the up-down axis.
			if (inputSwitchUp || inputSwitchDown) {
				var twoHandedWeapons = new Weapon[] {
					Weapon.TwoHandSword,
					 Weapon.TwoHandSpear,
					 Weapon.TwoHandAxe,
					 Weapon.TwoHandBow,
					 Weapon.TwoHandCrossbow,
					 Weapon.TwoHandStaff,
					 Weapon.Rifle,
				};
				// If we're not wielding 2Handed weapon already, just switch to the first one in the list.
				if (System.Array.IndexOf(twoHandedWeapons, rpgCharacterController.rightWeapon) == -1)
				{ weaponNumber = twoHandedWeapons[0]; }

				// Otherwise, we should loop through them.
				else {
					var index = System.Array.IndexOf(twoHandedWeapons, rpgCharacterController.rightWeapon);
					if (inputSwitchUp) { index = (index - 1 + twoHandedWeapons.Length) % twoHandedWeapons.Length; }
					else if (inputSwitchDown) { index = (index + 1) % twoHandedWeapons.Length; }
					weaponNumber = twoHandedWeapons[index];
				}
				// Set up the context and flag that we actually want to perform the switch.
				doSwitch = true;
				context.type = HandlerTypes.Switch;
				context.side = "None";
				context.leftWeapon = Weapon.Relax;
				context.rightWeapon = weaponNumber;
			}

			// Cycle through 1Handed weapons if any input happens on the left-right axis.
			if (inputSwitchLeft  || inputSwitchRight) {
				doSwitch = true;
				context.type = HandlerTypes.Switch;

				// Left-handed weapons.
				if (inputSwitchLeft) {
					var leftWeaponType = rpgCharacterController.leftWeapon;

					// If we are not wielding a left-handed weapon, switch to Left Sword.
					if (System.Array.IndexOf(WeaponGroupings.LeftHandedWeapons, leftWeaponType) == -1)
					{ weaponNumber = Weapon.LeftSword; }

					// Otherwise, cycle through the list.
					else {
						var currentIndex = System.Array.IndexOf(WeaponGroupings.LeftHandedWeapons, leftWeaponType);
						weaponNumber = WeaponGroupings.LeftHandedWeapons[(currentIndex + 1) % WeaponGroupings.LeftHandedWeapons.Length];
					}

					context.side = "Left";
					context.leftWeapon = weaponNumber;
					context.rightWeapon = Weapon.Relax;
				}
				// Right-handed weapons.
				else if (inputSwitchRight) {
					var rightWeaponType = rpgCharacterController.rightWeapon;

					// If we are not wielding a right-handed weapon, switch to Unarmed.
					if (System.Array.IndexOf(WeaponGroupings.RightHandedWeapons, rightWeaponType) == -1)
					{ weaponNumber = Weapon.Unarmed; }

					// Otherwise, cycle through the list.
					else {
						var currentIndex = System.Array.IndexOf(WeaponGroupings.RightHandedWeapons, rightWeaponType);
						weaponNumber = WeaponGroupings.RightHandedWeapons[(currentIndex + 1) % WeaponGroupings.RightHandedWeapons.Length];
					}
					context.side = "Right";
					context.leftWeapon = Weapon.Relax;
					context.rightWeapon = weaponNumber;
				}
			}
			// If we've received input, then "doSwitch" is true, and the context is filled out,
			// so start the SwitchWeapon action.
			if (doSwitch) { rpgCharacterController.StartAction(HandlerTypes.SwitchWeapon, context); }
		}
	}

	/// <summary>
	/// Extension Method to allow checking InputSystem without Action Callbacks.
	/// </summary>
	public static class InputActionExtensions
	{
		public static bool IsPressed(this InputAction inputAction) => inputAction.ReadValue<float>() > 0f;

		public static bool WasPressedThisFrame(this InputAction inputAction) => inputAction.triggered && inputAction.ReadValue<float>() > 0f;

		public static bool WasReleasedThisFrame(this InputAction inputAction) => inputAction.triggered && inputAction.ReadValue<float>() == 0f;
	}
}