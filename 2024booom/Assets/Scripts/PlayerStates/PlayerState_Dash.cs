using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState_Dash", menuName = "Data/StateMachine/PlayerState/Dash")]
public class PlayerState_Dash : PlayerState
{
    [SerializeField] float dashSpeed = 80f;
    [SerializeField] float dashTime = 0.15f;

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.StopSound("JumpSound");
        AudioManager.Instance.StopSound("LandSound");
        AudioManager.Instance.PlaySound("DashSound");

        player.dashCount--;
        player.dashStartTime = Time.time;
    }

    public override void Exit()
    {
        player.SetVelocity(new Vector2(0, 0));
    }

    public override void Update()
    {
        //ObjectPool.Instance.Get();
        if (input.JumpInputBuffer || input.Jump)
        {
            if (StateDuration > dashTime)
            {
                if (player.IsGround || player.jumpCount > 0)
                {
                    stateMachine.SwitchState(typeof(PlayerState_Jump));
                    return;
                }
            }
            else
                input.SetJumpInputBuffer();
        }

        if (StateDuration > dashTime)
        {
            if (player.IsGround) stateMachine.SwitchState(typeof(PlayerState_Idle));
            else stateMachine.SwitchState(typeof(PlayerState_Fall));
        }
    }

    public override void PhysicUpdate()
    {
        player.SetVelocity(new Vector2(dashSpeed * player.transform.localScale.x, 0));
    }
}