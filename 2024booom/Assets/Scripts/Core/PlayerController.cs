
using System;
using UnityEngine;

public struct Asix
{
    public float moveX;
    public float MoveY;
}

/// <summary>
/// ��Ҳ���������
/// </summary>
public partial class PlayerController
{
    private readonly int GroundMask;

    float varJumpTimer;
    float varJumpSpeed; //
    int moveX;
    private float maxFall;
    private float fastMaxFall;

    private float dashCooldownTimer;                //�����ȴʱ���������Ϊ0ʱ�������ٴγ��
    private float dashRefillCooldownTimer;          //
    public int dashes;
    public int lastDashes;
    private float wallSpeedRetentionTimer; // If you hit a wall, start this timer. If coast is clear within this timer, retain h-speed
    private float wallSpeedRetained;

    private bool onGround;
    private bool wasOnGround;

    public bool DashStartedOnGround { get; set; }

    public int ForceMoveX { get; set; }
    public float ForceMoveXTimer { get; set; }

    public int HopWaitX;   // If you climb hop onto a moving solid, snap to beside it until you get above it
    public float HopWaitXSpeed;

    public bool launched;
    public float launchedTimer;
    public float WallSlideTimer { get; set; } = Constants.WallSlideTime;
    public int WallSlideDir { get; set; }
    public JumpCheck JumpCheck { get; set; }    //����ʱ��
    public WallBoost WallBoost { get; set; }    //WallBoost
    private FiniteStateMachine<BaseActionState> stateMachine;

    //public ISpriteControl SpriteControl { get; private set; }
    //��Ч������
    //public IEffectControl EffectControl { get; private set; }
    //��Ч������
    //public ISoundControl SoundControl { get; private set; }
    public ICamera camera { get; private set; }
    public PlayerController()
    {
        //this.SpriteControl = spriteControl;
        //this.EffectControl = effectControl;

        this.stateMachine = new FiniteStateMachine<BaseActionState>((int)EActionState.Size);
        this.stateMachine.AddState(new NormalState(this));
        this.stateMachine.AddState(new DashState(this));
        this.stateMachine.AddState(new ClimbState(this));
        this.GroundMask = LayerMask.GetMask("Ground");

        this.Facing = Facings.Right;
        this.LastAim = Vector2.right;
    }

    public void RefreshAbility()
    {

        this.JumpCheck = new JumpCheck(this, Constants.EnableJumpGrace);

        if (!Constants.EnableWallBoost)
        {
            this.WallBoost = null;
        }
        else
        {
            this.WallBoost = this.WallBoost == null ? new WallBoost(this) : this.WallBoost;
        }
    }

    public void Init(Bounds bounds, Vector2 startPosition)
    {
        //���ݽ���ķ�ʽ,������ʼ״̬
        this.stateMachine.State = (int)EActionState.Normal;
        this.lastDashes = this.dashes = 1;
        this.Position = startPosition;
        this.collider = normalHitbox;

        //this.SpriteControl.SetSpriteScale(NORMAL_SPRITE_SCALE);

        this.bounds = bounds;
        this.cameraPosition = CameraTarget;
        //TODO ��ʼ��β����ɫ
        //Color color = NormalHairColor;
        //Gradient gradient = new Gradient();
        //gradient.SetKeys(
        //    new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
        //    new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 1.0f) }
        //);

        //this.player.SetTrailColor(gradient);

    }

    public void Update(float deltaTime)
    {
        //���¸�������б�����״̬
        {
            //Get ground
            wasOnGround = onGround;
            if (Speed.y <= 0)
            {
                this.onGround = CheckGround();//��ײ������
            }
            else
            {
                this.onGround = false;
            }

            //Wall Slide
            if (this.WallSlideDir != 0)
            {
                this.WallSlideTimer = Math.Max(this.WallSlideTimer - deltaTime, 0);
                this.WallSlideDir = 0;
            }
            if (this.onGround && this.stateMachine.State != (int)EActionState.Climb)
            {
                this.WallSlideTimer = Constants.WallSlideTime;
            }
            //Wall Boost, ����������WallJump
            this.WallBoost?.Update(deltaTime);

            //��Ծ���
            this.JumpCheck?.Update(deltaTime);

            //Dash
            {
                if (dashCooldownTimer > 0)
                    dashCooldownTimer -= deltaTime;
                if (dashRefillCooldownTimer > 0)
                {
                    dashRefillCooldownTimer -= deltaTime;
                }
                else if (onGround)
                {
                    RefillDash();
                }
            }

            //Var Jump
            if (varJumpTimer > 0)
            {
                varJumpTimer -= deltaTime;
            }

            //Force Move X
            if (ForceMoveXTimer > 0)
            {
                ForceMoveXTimer -= deltaTime;
                this.moveX = ForceMoveX;
            }
            else
            {
                //����
                this.moveX = Math.Sign(UnityEngine.Input.GetAxisRaw("Horizontal"));
            }

            //Facing
            if (moveX != 0 && this.stateMachine.State != (int)EActionState.Climb)
            {
                Facing = (Facings)moveX;
            }
            //Aiming
            LastAim = GameInput.GetAimVector(Facing);

            //ײǽ�Ժ���ٶȱ��֣�Wall Speed Retention������ײ��
            if (wallSpeedRetentionTimer > 0)
            {
                if (Math.Sign(Speed.x) == -Math.Sign(wallSpeedRetained))
                    wallSpeedRetentionTimer = 0;
                else if (!CollideCheck(Position, Vector2.right * Math.Sign(wallSpeedRetained)))
                {
                    Speed.x = wallSpeedRetained;
                    wallSpeedRetentionTimer = 0;
                }
                else
                    wallSpeedRetentionTimer -= deltaTime;
            }

            //Hop Wait X
            if (this.HopWaitX != 0)
            {
                if (Math.Sign(Speed.x) == -HopWaitX || Speed.y < 0)
                    this.HopWaitX = 0;
                else if (!CollideCheck(Position, Vector2.right * this.HopWaitX))
                {
                    Speed.x = this.HopWaitXSpeed;
                    this.HopWaitX = 0;
                }
            }

            //Launch Particles
            if (launched)
            {
                var sq = Speed.SqrMagnitude();
                if (sq < Constants.LaunchedMinSpeedSq)
                    launched = false;
                else
                {
                    var was = launchedTimer;
                    launchedTimer += deltaTime;

                    if (launchedTimer >= .5f)
                    {
                        launched = false;
                        launchedTimer = 0;
                    }
                    else if (Calc.OnInterval(launchedTimer, was, 0.15f))
                    {
                        //EffectControl.SpeedRing(this.Position, this.Speed.normalized);
                    }
                }
            }
            else
                launchedTimer = 0;

        }

        //״̬�������߼�
        stateMachine.Update(deltaTime);
        //����λ��
        UpdateCollideX(Speed.x * deltaTime);
        UpdateCollideY(Speed.y * deltaTime);

        //UpdateHair(deltaTime);

        UpdateCamera(deltaTime);
    }

    //������Ծ,��Ծʱ�򣬻����Ծǰ��һ��������ٶ�
    public void Jump()
    {
        GameInput.Jump.ConsumeBuffer();
        this.JumpCheck?.ResetTime();
        this.WallSlideTimer = Constants.WallSlideTime;
        this.WallBoost?.ResetTime();
        this.varJumpTimer = Constants.VarJumpTime;
        this.Speed.x += Constants.JumpHBoost * moveX;
        this.Speed.y = Constants.JumpSpeed;
        //Speed += LiftBoost;
        this.varJumpSpeed = this.Speed.y;

        //this.PlayJumpEffect(SpritePosition, Vector2.up);
    }

    //SuperJump����ʾ�ڵ����ϻ�������ʱ���ڣ�Dash����Ծ��
    //��ֵ�����Jump���ƣ���ֵ���
    //�׷�״̬��SuperJump��Ҫ���⴦����
    //Dash->Jump->Dush
    public void SuperJump()
    {
        GameInput.Jump.ConsumeBuffer();
        this.JumpCheck?.ResetTime();
        varJumpTimer = Constants.VarJumpTime;
        this.WallSlideTimer = Constants.WallSlideTime;
        this.WallBoost?.ResetTime();

        this.Speed.x = Constants.SuperJumpH * (int)Facing;
        this.Speed.y = Constants.JumpSpeed;
        //Speed += LiftBoost;
        if (Ducking)
        {
            Ducking = false;
            this.Speed.x *= Constants.DuckSuperJumpXMult;
            this.Speed.y *= Constants.DuckSuperJumpYMult;
        }

        varJumpSpeed = Speed.y;
        //TODO 
        launched = true;

        //this.PlayJumpEffect(this.SpritePosition, Vector2.up);
    }

    //��ǽ������µģ���Ծ����Ҫ��Ҫ���ǵ�ǰ��Ծ����
    public void WallJump(int dir)
    {
        GameInput.Jump.ConsumeBuffer();
        Ducking = false;
        this.JumpCheck?.ResetTime();
        varJumpTimer = Constants.VarJumpTime;
        this.WallSlideTimer = Constants.WallSlideTime;
        this.WallBoost?.ResetTime();
        if (moveX != 0)
        {
            this.ForceMoveX = dir;
            this.ForceMoveXTimer = Constants.WallJumpForceTime;
        }

        Speed.x = Constants.WallJumpHSpeed * dir;
        Speed.y = Constants.JumpSpeed;
        //TODO ���ǵ��ݶ��ٶȵļӳ�
        //Speed += LiftBoost;
        varJumpSpeed = Speed.y;

        ////ǽ������Ч����
        //if (dir == -1)
        //    this.PlayJumpEffect(this.RightPosition, Vector2.left);
        //else
        //    this.PlayJumpEffect(this.LeftPosition, Vector2.right);

    }

    public void ClimbJump()
    {
        if (!onGround)
        {
            //Stamina -= ClimbJumpCost;

            //sweatSprite.Play("jump", true);
            //Input.Rumble(RumbleStrength.Light, RumbleLength.Medium);
        }
        Jump();
        WallBoost?.Active();
    }

    //��ǽ��Dashʱ����ǰ��ס�ϣ���������ʱ��ִ��SuperWallJump
    public void SuperWallJump(int dir)
    {
        GameInput.Jump.ConsumeBuffer();
        Ducking = false;
        this.JumpCheck?.ResetTime();
        varJumpTimer = Constants.SuperWallJumpVarTime;
        this.WallSlideTimer = Constants.WallSlideTime;
        this.WallBoost?.ResetTime();

        Speed.x = Constants.SuperWallJumpH * dir;
        Speed.y = Constants.SuperWallJumpSpeed;
        //Speed += LiftBoost;
        varJumpSpeed = Speed.y;
        launched = true;

        //if (dir == -1)
        //    this.PlayJumpEffect(this.RightPosition, Vector2.left);
        //else
        //    this.PlayJumpEffect(this.LeftPosition, Vector2.right);
    }

    public bool RefillDash()
    {
        if (this.dashes < Constants.MaxDashes)
        {
            this.dashes = Constants.MaxDashes;
            return true;
        }
        else
            return false;
    }




    public bool CanDash
    {
        get
        {
            return GameInput.Dash.Pressed() && dashCooldownTimer <= 0 && this.dashes > 0;
        }
    }

    public float WallSpeedRetentionTimer
    {
        get { return this.wallSpeedRetentionTimer; }
        set { this.wallSpeedRetentionTimer = value; }
    }
    public Vector2 Speed;

    public object Holding => null;

    public bool OnGround => this.onGround;
    private Color groundColor = Color.white;
    public Color GroundColor => this.groundColor;
    public Vector2 Position { get; private set; }
    //��ʾ������ǽ״̬��0.1��ʱ��,�������ƶ���Ϊ������ҿ��巢������ǽ�Ķ���
    public float ClimbNoMoveTimer { get; set; }
    public float VarJumpSpeed => this.varJumpSpeed;

    public float VarJumpTimer
    {
        get
        {
            return this.varJumpTimer;
        }
        set
        {
            this.varJumpTimer = value;
        }
    }

    public int MoveX => moveX;
    public int MoveY => Math.Sign(UnityEngine.Input.GetAxisRaw("Vertical"));

    public float MaxFall { get => maxFall; set => maxFall = value; }
    public float DashCooldownTimer { get => dashCooldownTimer; set => dashCooldownTimer = value; }
    public float DashRefillCooldownTimer { get => dashRefillCooldownTimer; set => dashRefillCooldownTimer = value; }
    public Vector2 LastAim { get; set; }
    public Facings Facing { get; set; }  //��ǰ����
    public EActionState Dash()
    {
        //wasDashB = Dashes == 2;
        this.dashes = Math.Max(0, this.dashes - 1);
        GameInput.Dash.ConsumeBuffer();
        return EActionState.Dash;
    }
    public void SetState(int state)
    {
        this.stateMachine.State = state;
    }

    public bool Ducking
    {
        get
        {
            return this.collider == this.duckHitbox || this.collider == this.duckHurtbox;
        }
        set
        {
            if (value)
            {
                this.collider = this.duckHitbox;
                return;
            }
            else
            {
                this.collider = this.normalHitbox;
            }
            //PlayDuck(value);
        }
    }

    //��⵱ǰ�Ƿ����վ��
    public bool CanUnDuck
    {
        get
        {
            if (!Ducking)
                return true;
            Rect lastCollider = this.collider;
            this.collider = normalHitbox;
            bool noCollide = !CollideCheck(this.Position, Vector2.zero);
            this.collider = lastCollider;
            return noCollide;
        }
    }

    public bool DuckFreeAt(Vector2 at)
    {
        Vector2 oldP = Position;
        Rect oldC = this.collider;
        Position = at;
        this.collider = duckHitbox;

        bool ret = !CollideCheck(this.Position, Vector2.zero);

        this.Position = oldP;
        this.collider = oldC;

        return ret;
    }
    public bool IsFall
    {
        get
        {
            return !this.wasOnGround && this.OnGround;
        }
    }
}
