using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerParams", menuName = "Pro Platformer/Player Param", order = 1)]
public class PlayerParams : ScriptableObject
{
    [Header("���ù��ܡ�ǽ���»���")]
    public bool EnableWallSlide;
    [Header("���ù��ܡ�����ʱ�䡿")]
    public bool EnableJumpGrace;
    [Header("���ù��ܡ�WallBoost��")]
    [Tooltip("һ�����Բ����������ļ���")]
    public bool EnableWallBoost;

    [Header("ˮƽ�������")]
    [Tooltip("���ˮƽ�ٶ�")]
    public int MaxRun;
    [Tooltip("ˮƽ������ٶ�")]
    public int RunAccel;
    [Tooltip("ˮƽ������ٶ�")]
    public int RunReduce = 40;
    [Space]
    [Header("��ֱ�������")]
    [Tooltip("�������ٶ�")]
    public float Gravity = 90f; //����
    [Tooltip("���ٶ�С�ڸ���ֵʱ���������롣ֵԽС���Ϳ�ʱ��Խ����0��ʾ�ر�")]
    [Range(0, 9)]
    public float HalfGravThreshold = 4f;
    [Tooltip("���������ٶȣ�����������Ϊ����")]
    public float MaxFall = -16; //��ͨ��������ٶ�
    [Tooltip("�������������ٶȣ�����������Ϊ����")]
    public float FastMaxFall = -24f;  //������������ٶ�
    [Tooltip("����->������׹���ٶ�")]
    public float FastMaxAccel = 30f; //����������ٶ�

    [Space]
    [Header("��Ծ����")]
    [Tooltip("�����Ծ�ٶ�")]
    public float JumpSpeed = 10.5f;
    [Tooltip("��Ծ����ʱ�䣨����ʱ,�������Ӧ��Ծ����[VarJumpTime]��,Ӱ����Ծ����߸߶ȣ�")]
    public float VarJumpTime = 0.2f;
    [Tooltip("��Ծˮƽ����ˮƽ������ٶ�")]
    public float JumpHBoost = 4f;
    [Tooltip("����ʱ�䣨�뿪ƽ̨ʱ��������Ӧ��Ծ��ʱ�䣩")]
    public float JumpGraceTime = 0.1f;
    [Tooltip("�����˶������ϰ�������У������")]
    public int UpwardCornerCorrection = 4;

    [Header("Dash��̲���")]
    [Tooltip("��ʼ��̳��ٶ�")]
    public float DashSpeed = 24f;          //����ٶ�
    [Tooltip("������̺��ٶ�")]
    public float EndDashSpeed = 16f;        //��������ٶ�
    [Tooltip("Y�����ϳ�̵�˥��ϵ��")]
    public float EndDashUpMult = .75f;       //������ϳ�̣�������
    [Tooltip("���ʱ��")]
    public float DashTime = .15f;            //���ʱ��
    [Tooltip("�����ȴʱ��")]
    public float DashCooldown = .2f;         //�����ȴʱ�䣬
    [Tooltip("�������װ��ʱ��")]
    public float DashRefillCooldown = .1f;   //�������װ��ʱ��
    [Tooltip("Dashsˮƽ������ֱ����λ��У��������ֵ")]
    public int DashCornerCorrection = 4;     //Dashʱ�������赲��Ŀɾ������룬��λ0.1��
    [Tooltip("���Dash����")]
    public int MaxDashes = 1;    // ���Dash����


    [Header("��������")]
    [Tooltip("����ˮƽ�������߼������")]
    public int ClimbCheckDist = 2;           //�����������ֵ
    [Tooltip("������ֱ�������߼������")]
    public int ClimbUpCheckDist = 2;         //���������������ֵ
    [Tooltip("�����޷��ƶ�ʱ��")]
    public float ClimbNoMoveTime = .1f;
    [Tooltip("���������ٶ�")]
    public float ClimbUpSpeed = 4.5f;        //�����ٶ�
    [Tooltip("���������ٶ�")]
    public float ClimbDownSpeed = -8f;       //�����ٶ�
    [Tooltip("�����»��ٶ�")]
    public float ClimbSlipSpeed = -3f;       //�»��ٶ�
    [Tooltip("�����»����ٶ�")]
    public float ClimbAccel = 90f;          //�»����ٶ�
    [Tooltip("������ʼʱ����ԭY���ٶȵ�˥��")]
    public float ClimbGrabYMult = .2f;       //����ʱץȡ���µ�Y���ٶ�˥��

    [Header("Hop��������Ե��½��")]
    [Tooltip("Hop��Y���ٶ�")]
    public float ClimbHopY = 12f;          //Hop��Y���ٶ�
    [Tooltip("Hop��X���ٶ�")]
    public float ClimbHopX = 10f;           //Hop��X���ٶ�
    [Tooltip("Hopʱ��")]
    public float ClimbHopForceTime = .2f;    //Hopʱ��
    [Tooltip("WallBoostʱ��")]
    public float ClimbJumpBoostTime = .2f;   //WallBoostʱ��
    [Tooltip("Wind�����,Hop���޷�0.3��")]
    public float ClimbHopNoWindTime = .3f;   //Wind�����,Hop���޷�0.3��

    public float DuckFriction = 50f;
    public float DuckSuperJumpXMult = 1.25f;
    public float DuckSuperJumpYMult = 0.5f;

    private Action reloadCallback;
    public void SetReloadCallback(Action onReload)
    {
        this.reloadCallback = onReload;
    }

    public void OnValidate()
    {
        ReloadParams();
    }

    public void ReloadParams()
    {
        Debug.Log("=======��������Player���ò���");
        Constants.MaxRun = MaxRun;
        Constants.RunAccel = RunAccel;
        Constants.RunReduce = RunReduce;
        Constants.Gravity = Gravity; //����
        Constants.HalfGravThreshold = HalfGravThreshold;
        Constants.MaxFall = MaxFall; //��ͨ��������ٶ�
        Constants.FastMaxFall = FastMaxFall;  //������������ٶ�
        Constants.FastMaxAccel = FastMaxAccel; //����������ٶ�

        Constants.UpwardCornerCorrection = UpwardCornerCorrection;

        Constants.JumpSpeed = JumpSpeed;
        Constants.VarJumpTime = VarJumpTime;
        Constants.JumpHBoost = JumpHBoost;
        Constants.JumpGraceTime = JumpGraceTime;

        Constants.DashSpeed = DashSpeed;          //����ٶ�
        Constants.EndDashSpeed = EndDashSpeed;        //��������ٶ�
        Constants.EndDashUpMult = EndDashUpMult;       //������ϳ�̣�������
        Constants.DashTime = DashTime;            //���ʱ��
        Constants.DashCooldown = DashCooldown;         //�����ȴʱ�䣬
        Constants.DashRefillCooldown = DashRefillCooldown;   //�������װ��ʱ��
        Constants.DashCornerCorrection = DashCornerCorrection;     //ˮƽDashʱ�������赲��Ŀɾ�������ֵ
        Constants.MaxDashes = MaxDashes;    // ���Dash����

        Constants.ClimbCheckDist = ClimbCheckDist;           //�����������ֵ
        Constants.ClimbUpCheckDist = ClimbUpCheckDist;         //���������������ֵ
        Constants.ClimbNoMoveTime = ClimbNoMoveTime;
        Constants.ClimbUpSpeed = ClimbUpSpeed;        //�����ٶ�
        Constants.ClimbDownSpeed = ClimbDownSpeed;       //�����ٶ�
        Constants.ClimbSlipSpeed = ClimbSlipSpeed;       //�»��ٶ�
        Constants.ClimbAccel = ClimbAccel;          //�»����ٶ�
        Constants.ClimbGrabYMult = ClimbGrabYMult;       //����ʱץȡ���µ�Y���ٶ�˥��
        Constants.ClimbHopY = ClimbHopY;          //Hop��Y���ٶ� 
        Constants.ClimbHopX = ClimbHopX;           //Hop��X���ٶ�
        Constants.ClimbHopForceTime = ClimbHopForceTime;    //Hopʱ��
        Constants.ClimbJumpBoostTime = ClimbJumpBoostTime;   //WallBoostʱ��
        Constants.ClimbHopNoWindTime = ClimbHopNoWindTime;   //Wind�����,Hop���޷�0.3��

        Constants.WallJumpHSpeed = MaxRun + JumpHBoost;
        Constants.SuperJumpSpeed = JumpSpeed;
        Constants.SuperWallJumpH = MaxRun + JumpHBoost * 2;

        Constants.DashCornerCorrection = this.DashCornerCorrection;

        Constants.DuckFriction = DuckFriction;
        Constants.DuckSuperJumpXMult = DuckSuperJumpXMult;
        Constants.DuckSuperJumpYMult = DuckSuperJumpYMult;

        Constants.EnableWallSlide = this.EnableWallSlide; //����ǽ���»�����
        Constants.EnableJumpGrace = this.EnableJumpGrace; //����ʱ��
        Constants.EnableWallBoost = this.EnableWallBoost; //WallBoost

        reloadCallback?.Invoke();
    }
}