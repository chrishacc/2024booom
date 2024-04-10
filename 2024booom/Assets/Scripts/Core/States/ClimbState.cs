using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClimbState : BaseActionState
{
    public ClimbState(PlayerController context) : base(EActionState.Climb, context)
    {
    }

    public override IEnumerator Coroutine()
    {
        yield return null;
    }

    public override bool IsCoroutine()
    {
        return false;
    }

    public override void OnBegin()
    {
        ctx.Speed.x = 0;
        ctx.Speed.y *= Constants.ClimbGrabYMult;
        //TODO ��������
        ctx.WallSlideTimer = Constants.WallSlideTime;
        ctx.WallBoost?.ResetTime();
        ctx.ClimbNoMoveTimer = Constants.ClimbNoMoveTime;

        //�������ص���������
        ctx.ClimbSnap();
        //TODO ����
    }

    public override void OnEnd()
    {
        //TODO 
    }

    public override EActionState Update(float deltaTime)
    {
        ctx.ClimbNoMoveTimer -= deltaTime;
        //������Ծ
        if (GameInput.Jump.Pressed() && (!ctx.Ducking || ctx.CanUnDuck))
        {
            if (ctx.MoveX == -(int)ctx.Facing)
                ctx.WallJump(-(int)ctx.Facing);
            else
                ctx.ClimbJump();

            return EActionState.Normal;
        }
        if (ctx.CanDash)
        {
            return this.ctx.Dash();
        }
        //�ſ�ץȡ��,��ص�Normal״̬
        if (!GameInput.Grab.Checked())
        {
            //Speed += LiftBoost;
            //Play(Sfxs.char_mad_grab_letgo);
            return EActionState.Normal;
        }

        //���ǰ���ǽ���Ƿ����
        if (!ctx.CollideCheck(ctx.Position, Vector2.right * (int)ctx.Facing))
        {
            //Climbed over ledge?
            if (ctx.Speed.y < 0)
            {
                //if (ctx.WallBoosting)
                //{
                //    Speed += LiftBoost;
                //    Play(Sfxs.char_mad_grab_letgo);
                //}
                //else
                {
                    ClimbHop(); //�Զ���Խǽ��
                }
            }

            return EActionState.Normal;
        }

        {
            //Climbing
            float target = 0;
            bool trySlip = false;
            if (ctx.ClimbNoMoveTimer <= 0)
            {
                if (false)//(ClimbBlocker.Check(Scene, this, Position + Vector2.UnitX * (int)Facing))  
                {
                    //trySlip = true;
                }
                else if (ctx.MoveY == 1)
                {
                    //������
                    target = Constants.ClimbUpSpeed;
                    //�����������ƶ�����,��������ײ����SlipCheck
                    if (ctx.CollideCheck(ctx.Position, Vector2.up))
                    {
                        Debug.Log("=======ClimbSlip_Type1");
                        ctx.Speed.y = Mathf.Min(ctx.Speed.y, 0);
                        target = 0;
                        trySlip = true;
                    }
                    //���������0.6�״������ϰ�����ǰ�Ϸ�0.1�״�û���谭����Ȼ����������
                    else if (ctx.ClimbHopBlockedCheck() && ctx.SlipCheck(0.1f))
                    {
                        Debug.Log("=======ClimbSlip_Type2");
                        ctx.Speed.y = Mathf.Min(ctx.Speed.y, 0);
                        target = 0;
                        trySlip = true;
                    }
                    //���ǰ�Ϸ�û���谭, �����ClimbHop
                    else if (ctx.SlipCheck())
                    {
                        //Hopping
                        ClimbHop();
                        return EActionState.Normal;
                    }
                }
                else if (ctx.MoveY == -1)
                {
                    //������
                    target = Constants.ClimbDownSpeed;

                    if (ctx.OnGround)
                    {
                        ctx.Speed.y = Mathf.Max(ctx.Speed.y, 0);    //���ʱ,Y���ٶ�>=0
                        target = 0;
                    }
                    else
                    {
                        //����WallSlide����Ч��
                        //ctx.PlayWallSlideEffect(Vector2.right * (int)ctx.Facing);
                    }
                }
                else
                {
                    trySlip = true;
                }
            }
            else
            {
                trySlip = true;
            }

            //����
            if (trySlip && ctx.SlipCheck())
            {
                Debug.Log("=======ClimbSlip_Type4");
                target = Constants.ClimbSlipSpeed;
            }
            ctx.Speed.y = Mathf.MoveTowards(ctx.Speed.y, target, Constants.ClimbAccel * deltaTime);
        }
        //TrySlip���µ��»��������ײ���ʱ��,ֹͣ�»�
        if (ctx.MoveY != -1 && ctx.Speed.y < 0 && !ctx.CollideCheck(ctx.Position, new Vector2((int)ctx.Facing, -1)))
        {
            ctx.Speed.y = 0;
        }
        //TODO Stamina
        return state;
    }

    private void ClimbHop()
    {
        Debug.Log("=====ClimbHop");
        //����Hop�ľ��鶯��
        //playFootstepOnLand = 0.5f;

        //��ȡĿ�����ŵ�
        bool hit = ctx.CollideCheck(ctx.Position, Vector2.right * (int)ctx.Facing);
        if (hit)
        {
            ctx.HopWaitX = (int)ctx.Facing;
            ctx.HopWaitXSpeed = (int)ctx.Facing * Constants.ClimbHopX;
        }
        //ctx.ClimbHopSolid = ctx.CollideClimbHop((int)ctx.Facing);
        //if (ctx.ClimbHopSolid)
        //{
        //    //climbHopSolidPosition = climbHopSolid.Position;
        //    ctx.HopWaitX = (int)ctx.Facing;
        //    ctx.HopWaitXSpeed = (int)ctx.Facing * Constants.ClimbHopX;
        //}
        else
        {
            ctx.HopWaitX = 0;
            ctx.Speed.x = (int)ctx.Facing * Constants.ClimbHopX;
        }

        ctx.Speed.y = Math.Max(ctx.Speed.y, Constants.ClimbHopY);
        ctx.ForceMoveX = 0;
        ctx.ForceMoveXTimer = Constants.ClimbHopForceTime;
    }
}
