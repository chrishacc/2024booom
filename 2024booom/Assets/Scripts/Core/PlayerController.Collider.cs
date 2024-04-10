using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UIElements;
using System;

/// <summary>
/// ��¼PlayerController������ײ��ع���
/// 
/// //ÿ���Ӵ���֮����0.01���ȵķ�϶.
/// </summary>
public partial class PlayerController
{
    const float STEP = 0.1f;  //��ײ��ⲽ������POINT�����
    const float DEVIATION = 0.02f;  //��ײ������

    private readonly Rect normalHitbox = new Rect(0, -0.25f, 0.8f, 1.1f);
    private readonly Rect duckHitbox = new Rect(0, -0.5f, 0.8f, 0.6f);
    private readonly Rect normalHurtbox = new Rect(0f, -0.15f, 0.8f, 0.9f);
    private readonly Rect duckHurtbox = new Rect(8f, 4f, 0.8f, 0.4f);

    private Rect collider;


    public void AdjustPosition(Vector2 adjust)
    {
        UpdateCollideX(adjust.x);
        UpdateCollideY(adjust.y);
    }

    //��ײ���
    public bool CollideCheck(Vector2 position, Vector2 dir, float dist = 0)
    {
        Vector2 origion = position + collider.position;
        return Physics2D.OverlapBox(origion + dir * (dist + DEVIATION), collider.size, 0, GroundMask);
    }

    public bool OverlapPoint(Vector2 position)
    {
        return Physics2D.OverlapPoint(position, GroundMask);
    }

    //�������
    public bool ClimbCheck(int dir, float yAdd = 0)
    {
        //��ȡ��ǰ����ײ��
        Vector2 origion = this.Position + collider.position;
        if (Physics2D.OverlapBox(origion + Vector2.up * (float)yAdd + Vector2.right * dir * (Constants.ClimbCheckDist * 0.1f + DEVIATION), collider.size, 0, GroundMask))
        {
            return true;
        }
        return false;
    }

    //������ײ����X���ϵ������ƶ�����
    protected void UpdateCollideX(float distX)
    {
        Vector2 targetPosition = this.Position;
        //ʹ��У��
        float distance = distX;
        int correctTimes = 1;
        while (true)
        {
            float moved = MoveXStepWithCollide(distance);
            //����ײ�˳�ѭ��
            this.Position += Vector2.right * moved;
            if (moved == distance || correctTimes == 0) //����ײ����У������Ϊ0
            {
                break;
            }
            float tempDist = distance - moved;
            correctTimes--;
            if (!CorrectX(tempDist))
            {
                this.Speed.x = 0;//δ���У�������ٶ�����

                //Speed retention
                if (wallSpeedRetentionTimer <= 0)
                {
                    wallSpeedRetained = this.Speed.x;
                    wallSpeedRetentionTimer = Constants.WallSpeedRetentionTime;
                }
                break;
            }
            distance = tempDist;
        }
    }

    protected void UpdateCollideY(float distY)
    {
        Vector2 targetPosition = this.Position;
        //ʹ��У��
        float distance = distY;
        int correctTimes = 1; //Ĭ�Ͽ��Ե���λ��10��
        bool collided = true;
        float speedY = Mathf.Abs(this.Speed.y);
        while (true)
        {
            float moved = MoveYStepWithCollide(distance);
            //����ײ�˳�ѭ��
            this.Position += Vector2.up * moved;
            if (moved == distance || correctTimes == 0) //����ײ����У������Ϊ0
            {
                collided = false;
                break;
            }
            float tempDist = distance - moved;
            correctTimes--;
            if (!CorrectY(tempDist))
            {
                this.Speed.y = 0;//δ���У�������ٶ�����
                break;
            }
            distance = tempDist;
        }

        ////���ʱ�򣬽�������
        //if (collided && distY < 0)
        //{
        //    if (this.stateMachine.State != (int)EActionState.Climb)
        //    {
        //        this.PlayLandEffect(this.SpritePosition, speedY);
        //    }
        //}
    }
    private bool CheckGround()
    {
        return CheckGround(Vector2.zero);
    }
    //��Ժ���,������ײ���.���������ײ,
    private bool CheckGround(Vector2 offset)
    {
        Vector2 origion = this.Position + collider.position + offset;
        RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, Vector2.down, DEVIATION, GroundMask);
        if (hit && hit.normal == Vector2.up)
        {
            return true;
        }
        return false;
    }

    //���������ؿ��ı�Ե����м��,ȷ�������ڹؿ��Ŀ���.
    public bool ClimbBoundsCheck(int dir)
    {
        return true;
        //return base.Left + (float)(dir * 2) >= (float)this.level.Bounds.Left && base.Right + (float)(dir * 2) < (float)this.level.Bounds.Right;
    }

    //ǽ���������
    public bool WallJumpCheck(int dir)
    {
        return ClimbBoundsCheck(dir) && this.CollideCheck(Position, Vector2.right * dir, Constants.WallJumpCheckDist);
    }

    public RaycastHit2D ClimbHopSolid { get; set; }
    public RaycastHit2D CollideClimbHop(int dir)
    {
        Vector2 origion = this.Position + collider.position;
        RaycastHit2D hit = Physics2D.BoxCast(Position, collider.size, 0, Vector2.right * dir, DEVIATION, GroundMask);
        return hit;
        //if (hit && hit.normal.x == -dir)
        //{

        //}
    }

    public bool SlipCheck(float addY = 0)
    {
        int direct = Facing == Facings.Right ? 1 : -1;
        Vector2 origin = this.Position + collider.position + Vector2.up * collider.size.y / 2f + Vector2.right * direct * (collider.size.x / 2f + STEP);
        Vector2 point1 = origin + Vector2.up * (-0.4f + addY);

        if (Physics2D.OverlapPoint(point1, GroundMask))
        {
            return false;
        }
        Vector2 point2 = origin + Vector2.up * (0.4f + addY);
        if (Physics2D.OverlapPoint(point2, GroundMask))
        {
            return false;
        }
        return true;
    }

    public bool ClimbHopBlockedCheck()
    {
        return false;
    }

    //�����ƶ��������ͷ���ֵ�������򣬱�ʾY��
    private float MoveYStepWithCollide(float distY)
    {
        Vector2 moved = Vector2.zero;
        Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
        Vector2 origion = this.Position + collider.position;
        RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
        if (hit && hit.normal == -direct)
        {
            //���������ײ,���ƶ�����
            moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
        }
        else
        {
            moved += Vector2.up * distY;
        }
        return moved.y;
    }

    private float MoveXStepWithCollide(float distX)
    {
        Vector2 moved = Vector2.zero;
        Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;
        Vector2 origion = this.Position + collider.position;
        RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distX) + DEVIATION, GroundMask);
        if (hit && hit.normal == -direct)
        {
            //���������ײ,���ƶ�����
            moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
        }
        else
        {
            moved += Vector2.right * distX;
        }
        return moved.x;
    }

    private bool CorrectX(float distX)
    {
        Vector2 origion = this.Position + collider.position;
        Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;

        if ((this.stateMachine.State == (int)EActionState.Dash))
        {
            if (onGround && DuckFreeAt(Position + Vector2.right * distX))
            {
                Ducking = true;
                return true;
            }
            else if (this.Speed.y == 0 && this.Speed.x != 0)
            {
                for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                {
                    for (int j = 1; j >= -1; j -= 2)
                    {
                        if (!CollideCheck(this.Position + new Vector2(0, j * i * 0.1f), direct, Mathf.Abs(distX)))
                        {
                            this.Position += new Vector2(distX, j * i * 0.1f);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CorrectY(float distY)
    {
        Vector2 origion = this.Position + collider.position;
        Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;

        if (this.Speed.y < 0)
        {
            if ((this.stateMachine.State == (int)EActionState.Dash) && !DashStartedOnGround)
            {
                if (this.Speed.x <= 0)
                {
                    for (int i = -1; i >= -Constants.DashCornerCorrection; i--)
                    {
                        float step = (Mathf.Abs(i * 0.1f) + DEVIATION);

                        if (!CheckGround(new Vector2(-step, 0)))
                        {
                            this.Position += new Vector2(-step, distY);
                            return true;
                        }
                    }
                }

                if (this.Speed.x >= 0)
                {
                    for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                    {
                        float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                        if (!CheckGround(new Vector2(step, 0)))
                        {
                            this.Position += new Vector2(step, distY);
                            return true;
                        }
                    }
                }
            }
        }
        //�����ƶ�
        else if (this.Speed.y > 0)
        {
            //Y�����Ϸ����Corner Correction
            {
                if (this.Speed.x <= 0)
                {
                    for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(-i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                        if (!hit)
                        {
                            this.Position += new Vector2(-i * 0.1f, 0);
                            return true;
                        }
                    }
                }

                if (this.Speed.x >= 0)
                {
                    for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                        if (!hit)
                        {
                            this.Position += new Vector2(i * 0.1f, 0);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    //����ʱ����������
    public bool ClimbUpSnap()
    {
        for (int i = 1; i <= Constants.ClimbUpCheckDist; i++)
        {
            //����Ϸ��Ƿ���ڿ���������ǽ�ڣ����������˲��i������
            float yOffset = i * 0.1f;
            if (!CollideCheck(this.Position, Vector2.up, yOffset) && ClimbCheck((int)Facing, yOffset + DEVIATION))
            {
                this.Position += Vector2.up * yOffset;
                Debug.Log($"======Climb Correct");
                return true;
            }
        }
        return false;
    }

    //����ˮƽ�����ϵ�����
    public void ClimbSnap()
    {
        Vector2 origion = this.Position + collider.position;
        Vector2 dir = Vector2.right * (int)this.Facing;
        RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, dir, Constants.ClimbCheckDist * 0.1f + DEVIATION, GroundMask);
        if (hit)
        {
            //���������ײ,���ƶ�����
            this.Position += dir * Mathf.Max((hit.distance - DEVIATION), 0);
        }
        //for (int i = 0; i < Constants.ClimbCheckDist; i++)
        //{
        //    Vector2 dir = Vector2.right * (int)ctx.Facing;
        //    if (!ctx.CollideCheck(ctx.Position, dir))
        //    {
        //        ctx.AdjustPosition(dir * 0.1f);
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}
    }

    //public void MoveExactY(float distY)
    //{
    //    CorrectY(distY);
    //}
}
