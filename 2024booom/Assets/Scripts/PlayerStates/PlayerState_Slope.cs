using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState_Slope", menuName = "Data/StateMachine/PlayerState/Slope")]
public class PlayerState_Slope : PlayerState
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float acceleration = 5f;

    public override void Enter()
    {
        base.Enter();

        currentSpeed = player.moveSpeed;
    }

    public override void Update()
    {
        if (!input.Move)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }

        if (input.Dash && player.canDash)
        {
            if (input.UpInputBuffer)
            {
                stateMachine.SwitchState(typeof(PlayerState_UpDash));
                return;
            }
            stateMachine.SwitchState(typeof(PlayerState_Dash));
        }

        if (input.Jump)
        {
            stateMachine.SwitchState(typeof(PlayerState_Jump));
        }

        if (!player.IsGround)
        {
            stateMachine.SwitchState(typeof(PlayerState_CoyoteTime));
        }

        if (input.shoot)
        {
            stateMachine.SwitchState(typeof(PlayerState_Shoot));
        }
    }

    public override void PhysicUpdate()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeed, acceleration * Time.fixedDeltaTime);

        player.Move(currentSpeed);
    }
}
