using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����漰�������ֵ��Ҫ/10, ��ʱ������
public static class Constants
{

    public static bool EnableWallSlide = true;
    public static bool EnableJumpGrace = true;
    public static bool EnableWallBoost = true;

    public static float Gravity = 90f; //����

    public static float HalfGravThreshold = 4f; //�Ϳ�ʱ����ֵ
    public static float MaxFall = -16; //��ͨ��������ٶ�
    public static float FastMaxFall = -24f;  //������������ٶ�
    public static float FastMaxAccel = 30f; //����������ٶ�
                                            //����ƶ��ٶ�
    public static float MaxRun = 9f;
    //Hold����µ�����ƶ��ٶ�
    public static float HoldingMaxRun = 7f;
    //��������
    public static float AirMult = 0.65f;
    //�ƶ����ٶ�
    public static float RunAccel = 100f;
    //�ƶ����ٶ�
    public static float RunReduce = 40f;
    //
    public static float JumpSpeed = 10.5f;  //�����Ծ�ٶ�
    public static float VarJumpTime = 0.2f; //��Ծ����ʱ��(����ʱ,�������Ӧ��Ծ����[VarJumpTime]��,Ӱ����Ծ����߸߶�);
    public static float JumpHBoost = 4f; //����ǽ�ڵ���
    public static float JumpGraceTime = 0.1f;//����ʱ��

    #region WallJump
    public static float WallJumpCheckDist = 0.3f;
    public static float WallJumpForceTime = .16f; //ǽ����Ծǿ��ʱ��
    public static float WallJumpHSpeed = MaxRun + JumpHBoost;

    #endregion

    #region SuperWallJump
    public static float SuperJumpSpeed = JumpSpeed;
    public static float SuperJumpH = 26f;
    public static float SuperWallJumpSpeed = 16f;
    public static float SuperWallJumpVarTime = .25f;
    public static float SuperWallJumpForceTime = .2f;
    public static float SuperWallJumpH = MaxRun + JumpHBoost * 2;
    #endregion
    #region WallSlide
    public static float WallSpeedRetentionTime = .06f; //ײǽ�Ժ��������ı����ٶȵ�ʱ��
    public static float WallSlideTime = 1.2f; //ǽ�ڻ���ʱ��
    public static float WallSlideStartMax = -2f;


    #endregion

    #region Dash��ز���
    public static float DashSpeed = 24f;           //����ٶ�
    public static float EndDashSpeed = 16f;        //��������ٶ�
    public static float EndDashUpMult = .75f;       //������ϳ�̣�������
    public static float DashTime = .15f;            //���ʱ��
    public static float DashCooldown = .2f;         //�����ȴʱ�䣬
    public static float DashRefillCooldown = .1f;   //�������װ��ʱ��
    public static int DashHJumpThruNudge = 6;       //
    public static int DashCornerCorrection = 4;     //ˮƽDashʱ�������赲��Ŀɾ�������ֵ
    public static int DashVFloorSnapDist = 3;       //DashAttacking�µĵ�����������ֵ
    public static float DashAttackTime = .3f;       //
    public static int MaxDashes = 1;
    #endregion

    #region Climb����
    public static float ClimbMaxStamina = 110;       //�������
    public static float ClimbUpCost = 100 / 2.2f;   //����������������
    public static float ClimbStillCost = 100 / 10f; //���Ų�����������
    public static float ClimbJumpCost = 110 / 4f;   //������Ծ��������
    public static int ClimbCheckDist = 2;           //�����������ֵ
    public static int ClimbUpCheckDist = 2;         //���������������ֵ
    public static float ClimbNoMoveTime = .1f;
    public static float ClimbTiredThreshold = 20f;  //����ƣ������ֵ
    public static float ClimbUpSpeed = 4.5f;        //�����ٶ�
    public static float ClimbDownSpeed = -8f;       //�����ٶ�
    public static float ClimbSlipSpeed = -3f;       //�»��ٶ�
    public static float ClimbAccel = 90f;           //�»����ٶ�
    public static float ClimbGrabYMult = .2f;       //����ʱץȡ���µ�Y���ٶ�˥��
    public static float ClimbHopY = 12f;            //Hop��Y���ٶ�
    public static float ClimbHopX = 10f;            //Hop��X���ٶ�
    public static float ClimbHopForceTime = .2f;    //Hopʱ��
    public static float ClimbJumpBoostTime = .2f;   //WallBoostʱ��
    public static float ClimbHopNoWindTime = .3f;   //Wind�����,Hop���޷�0.3��
    #endregion

    #region Duck����
    public static float DuckFriction = 50f;
    public static float DuckSuperJumpXMult = 1.25f;
    public static float DuckSuperJumpYMult = .5f;
    #endregion

    #region Corner Correct
    public static int UpwardCornerCorrection = 4; //�����ƶ���X���ϱ�ԵУ����������
    #endregion

    public static float LaunchedMinSpeedSq = 196;
}
