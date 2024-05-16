using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个类将会挂载到玩家角色对象上
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

        //对玩家的状态进行初始化
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