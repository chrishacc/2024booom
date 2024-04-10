using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wall Boost���
/// ����ǽʱ��Ծ�����û��ָ��X�᷽����ϵͳ�Զ�����һ������ǽ��������Ҹ���һ���ĳ���ʱ��
/// If you climb jump and then do a sideways input within this timer, switch to wall jump
/// </summary>
public class WallBoost
{
    private float timer;
    private int dir;
    private PlayerController controller;
    public float Timer => timer;
    public WallBoost(PlayerController playerController)
    {
        this.controller = playerController;
        this.dir = 0;
        this.ResetTime();
    }

    public void ResetTime()
    {
        this.timer = 0;
    }

    public void Update(float deltaTime)
    {
        if (timer > 0)
        {
            timer -= deltaTime;
            if (controller.MoveX == dir)
            {
                controller.Speed.x = Constants.WallJumpHSpeed * controller.MoveX;
                timer = 0;
            }
        }
    }

    //��Ծʱ������
    public void Active()
    {
        if (controller.MoveX == 0)
        {
            Debug.Log("====WallBoost");
            this.dir = -(int)controller.Facing;
            this.timer = Constants.ClimbJumpBoostTime;
        }
    }
}