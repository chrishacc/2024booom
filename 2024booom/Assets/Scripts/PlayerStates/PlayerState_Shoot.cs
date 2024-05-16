using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState_Shoot", menuName = "Data/StateMachine/PlayerState/Shoot")]
public class PlayerState_Shoot : PlayerState
{
    [SerializeField] float dashSpeed = -5f;
    [SerializeField] float shootTime = 0.1f;

    public override void Enter()
    {
        base.Enter();
        //player.dashCount--;
        //player.dashStartTime = Time.time; 

        //AudioManager.Instance.PlaySound("ShootSound");
    }

    public override void Exit()
    {
        player.SetVelocity(new Vector2(0, 0));
    }

    public override void Update()
    {
        //ObjectPool.Instance.Get();
        if (input.ShootInputBuffer || input.shoot)
        {
            //if (StateDuration > shootTime)
            //{
            //    if (player.IsGround)
            //    {

            //        return;
            //    }
            //}
            //else
                input.SetShootInputBuffer();
        }

        if (StateDuration > shootTime)
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