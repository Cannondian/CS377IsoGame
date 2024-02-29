using RPGCharacterAnims.Lookups;

namespace RPGCharacterAnims.Actions
{
    public class DiveRoll : MovementActionHandler<DiveRollType>
    {
        public DiveRoll(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        { return (controller.canAction || controller.isAttacking) && !controller.isRelaxed && RollManager.Instance.CanRoll(); }

        protected override void _StartAction(RPGCharacterController controller, DiveRollType rollType)
        {
            controller.DiveRoll(rollType);
            movement.currentState = CharacterState.DiveRoll;
            RollManager.Instance.Roll();
		}

        public override bool IsActive()
        { return movement.currentState != null && (CharacterState)movement.currentState == CharacterState.DiveRoll; }
    }
}