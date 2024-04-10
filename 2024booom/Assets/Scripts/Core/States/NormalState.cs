using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : BaseActionState
{
    public NormalState(PlayerController controller) : base(EActionState.Normal, controller)
    {
    }

    public override IEnumerator Coroutine()
    {
        throw new NotImplementedException();
    }

    public override bool IsCoroutine()
    {
        return false;
    }

    public override void OnBegin()
    {
        this.ctx.MaxFall = Constants.MaxFall;
    }

    public override void OnEnd()
    {
        this.ctx.WallBoost?.ResetTime();
        this.ctx.WallSpeedRetentionTimer = 0;
        this.ctx.HopWaitX = 0;
    }

    public override EActionState Update(float deltaTime)
    {
        //Climb
        if (GameInput.Grab.Checked() && !ctx.Ducking)
        {
            //Climbing
            if (ctx.Speed.y <= 0 && Math.Sign(ctx.Speed.x) != -(int)ctx.Facing)
            {
                if (ctx.ClimbCheck((int)ctx.Facing))
                {
                    ctx.Ducking = false;
                    return EActionState.Climb;
                }
                //����׹�������Ҫ����������������
                if (ctx.MoveY > -1)
                {
                    bool snapped = ctx.ClimbUpSnap();
                    if (snapped)
                    {
                        ctx.Ducking = false;
                        return EActionState.Climb;
                    }
                }
            }
        }

        //Dashing
        if (this.ctx.CanDash)
        {
            return this.ctx.Dash();
        }

        //Ducking
        if (ctx.Ducking)
        {
            if (ctx.OnGround && ctx.MoveY != -1)
            {
                if (ctx.CanUnDuck)
                {
                    ctx.Ducking = false;
                }
                else if (ctx.Speed.x == 0)
                {
                    //���ݽ���λ�ã����м�������
                }
            }
        }
        else if (ctx.OnGround && ctx.MoveY == -1 && ctx.Speed.y <= 0)
        {
            ctx.Ducking = true;
            //ctx.PlayDuck(true);
        }

        //ˮƽ�����ƶ�,��������
        if (ctx.Ducking && ctx.OnGround)
        {
            ctx.Speed.x = Mathf.MoveTowards(ctx.Speed.x, 0, Constants.DuckFriction * deltaTime);
        }
        else
        {
            float mult = ctx.OnGround ? 1 : Constants.AirMult;
            //����ˮƽ�ٶ�
            float max = ctx.Holding == null ? Constants.MaxRun : Constants.HoldingMaxRun;
            if (Math.Abs(ctx.Speed.x) > max && Math.Sign(ctx.Speed.x) == this.ctx.MoveX)
            {
                //ͬ�������
                ctx.Speed.x = Mathf.MoveTowards(ctx.Speed.x, max * this.ctx.MoveX, Constants.RunReduce * mult * Time.deltaTime);
            }
            else
            {
                //���������
                ctx.Speed.x = Mathf.MoveTowards(ctx.Speed.x, max * this.ctx.MoveX, Constants.RunAccel * mult * Time.deltaTime);
            }
        }
        //������ֱ�ٶ�
        {
            //������������ٶ�
            {
                float maxFallSpeed = Constants.MaxFall;
                float fastMaxFallSpeed = Constants.FastMaxFall;

                if (this.ctx.MoveY == -1 && this.ctx.Speed.y <= maxFallSpeed)
                {
                    this.ctx.MaxFall = Mathf.MoveTowards(this.ctx.MaxFall, fastMaxFallSpeed, Constants.FastMaxAccel * deltaTime);

                    //�������
                    //this.ctx.PlayFallEffect(ctx.Speed.y);
                }
                else
                {
                    this.ctx.MaxFall = Mathf.MoveTowards(this.ctx.MaxFall, maxFallSpeed, Constants.FastMaxAccel * deltaTime);
                }
            }

            if (!ctx.OnGround)
            {
                float max = this.ctx.MaxFall;//��������ٶ�
                                             //Wall Slide
                if ((ctx.MoveX == (int)ctx.Facing || (ctx.MoveX == 0 && GameInput.Grab.Checked())) && ctx.MoveY != -1)
                {
                    //�ж��Ƿ�������Wall����
                    if (ctx.Speed.y <= 0 && ctx.WallSlideTimer > 0 && ctx.ClimbBoundsCheck((int)ctx.Facing) && ctx.CollideCheck(ctx.Position, Vector2.right * (int)ctx.Facing) && ctx.CanUnDuck)
                    {
                        ctx.Ducking = false;
                        ctx.WallSlideDir = (int)ctx.Facing;
                    }

                    if (ctx.WallSlideDir != 0)
                    {
                        //if (ctx.WallSlideTimer > Constants.WallSlideTime * 0.5f && ClimbBlocker.Check(level, this, Position + Vector2.UnitX * wallSlideDir))
                        //    ctx.WallSlideTimer = Constants.WallSlideTime * .5f;

                        max = Mathf.Lerp(Constants.MaxFall, Constants.WallSlideStartMax, ctx.WallSlideTimer / Constants.WallSlideTime);
                        if ((ctx.WallSlideTimer / Constants.WallSlideTime) > .65f)
                        {
                            //���Ż�����Ч
                            //ctx.PlayWallSlideEffect(Vector2.right * ctx.WallSlideDir);
                        }
                    }
                }

                float mult = (Math.Abs(ctx.Speed.y) < Constants.HalfGravThreshold && (GameInput.Jump.Checked())) ? .5f : 1f;
                //���е����,��Ҫ����Y���ٶ�
                ctx.Speed.y = Mathf.MoveTowards(ctx.Speed.y, max, Constants.Gravity * mult * deltaTime);
            }

            //������Ծ
            if (ctx.VarJumpTimer > 0)
            {
                if (GameInput.Jump.Checked())
                {
                    //�����ס��Ծ������Ծ�ٶȲ�������Ӱ�졣
                    ctx.Speed.y = Math.Max(ctx.Speed.y, ctx.VarJumpSpeed);
                }
                else
                    ctx.VarJumpTimer = 0;
            }
        }

        if (GameInput.Jump.Pressed())
        {
            //����ʱ�䷶Χ��,������Ծ
            if (this.ctx.JumpCheck.AllowJump())
            {
                this.ctx.Jump();
            }
            else if (ctx.CanUnDuck)
            {
                //����Ҳ���ǽ
                if (ctx.WallJumpCheck(1))
                {
                    if (ctx.Facing == Facings.Right && GameInput.Grab.Checked())
                        ctx.ClimbJump();
                    else
                        ctx.WallJump(-1);
                }
                //��������ǽ
                else if (ctx.WallJumpCheck(-1))
                {
                    if (ctx.Facing == Facings.Left && GameInput.Grab.Checked())
                        ctx.ClimbJump();
                    else
                        ctx.WallJump(1);
                }
            }
        }

        return state;
    }
}