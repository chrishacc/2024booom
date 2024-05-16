using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ཫ����ص���ҽ�ɫ������
/// </summary>
public class PlayerStateMachine : StateMachine
{
    //public PlayerState_Idle idleState;
    //public PlayerState_Run runState;

    Animator animator;

    [SerializeField] PlayerState[] states;

    PlayerInput input;

    PlayerController player;


    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        input = GetComponent<PlayerInput>();

        player = GetComponent<PlayerController>();

        stateTable = new Dictionary<System.Type, IState>(states.Length);

        //����ҵ�״̬���г�ʼ��
        foreach (var state in states)
        {
            state.Initialize(animator, player, input, this);
            stateTable.Add(state.GetType(), state);
        }
    }

    void Start()
    {
        SwitchOn(stateTable[typeof(PlayerState_Idle)]);
    }
}