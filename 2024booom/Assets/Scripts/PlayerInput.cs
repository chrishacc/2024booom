using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions inputActions;

    [Header("跳跃预输入保留时间")]
    [SerializeField] float jumpInputBufferTime = 0.2f;
    WaitForSeconds waitJumpInputBufferTime;
    public bool JumpInputBuffer { get; set; }

    [Header("向上冲刺时上键预输入保留时间")]
    [SerializeField] float upInputBufferTime = 0.1f;
    WaitForSeconds waitUpInputBufferTime;
    public bool Up => inputActions.Gameplay.Up.IsPressed();
    public bool UpInputBuffer { get; private set; }

    public bool Dash => inputActions.Gameplay.Dash.WasPressedThisFrame();
    public bool Jump => inputActions.Gameplay.Jump.WasPressedThisFrame();
    public bool StopJump => inputActions.Gameplay.Jump.WasReleasedThisFrame();
    public bool Move => AxesX != 0;

    public bool explo => inputActions.Gameplay.G.WasPressedThisFrame();

    public bool Dialog => inputActions.Gameplay.Dialog.WasPressedThisFrame();

    //UI相关处理
    public bool Esc => inputActions.Gameplay.Esc.WasPressedThisFrame();
    public GameObject Panel;

    //发射子弹时发射键预输入保留时间
    public bool ShootInputBuffer { get; set; }
    WaitForSeconds waitShotInputBufferTime;
    public bool shoot => inputActions.Gameplay.Fire.WasPressedThisFrame();
    [SerializeField] GameObject projectial;
    [SerializeField] Transform muzzle;

    public float AxesX;

    float LeftTime;
    float RightTime;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        waitJumpInputBufferTime = new WaitForSeconds(jumpInputBufferTime);
        waitUpInputBufferTime = new WaitForSeconds(upInputBufferTime);
        waitShotInputBufferTime = new WaitForSeconds(0.1f);
    }

    void Update()
    {
        CheckMoveDirection();
        if (Up) SetUPInputBuffer();
        if (shoot && MagicLimit.Instance.magicCounts > 0)
        {
            MagicLimit.Instance.DecreaseMagic();
            AudioManager.Instance.PlaySound("ShootSound");
            SetShootInputBuffer();

            GameObject newProjectial = Instantiate(projectial, muzzle.position, Quaternion.identity);
            newProjectial.GetComponent<Playerprojectial>().SetDir(new Vector2(transform.localScale.x, transform.localScale.y));

        }

        if (Esc && Panel.activeSelf == false)
        {
            Panel.SetActive(true);
        }
        else if(Esc && Panel.activeSelf == true)
        {
            Panel.SetActive(false);
        }
    }

    void OnEnable()
    {
        inputActions.Gameplay.Jump.canceled += delegate
        {
            JumpInputBuffer = false;
        };
    }

    void CheckMoveDirection()
    {
        if (inputActions.Gameplay.LeftMove.WasPressedThisFrame())
        {
            LeftTime = Time.time;
        }
        if (inputActions.Gameplay.RightMove.WasPressedThisFrame())
        {
            RightTime = Time.time;
        }
        if (inputActions.Gameplay.LeftMove.IsPressed() && inputActions.Gameplay.RightMove.IsPressed())
        {
            AxesX = (LeftTime > RightTime) ? -1 : 1;
        }
        else if (inputActions.Gameplay.LeftMove.IsPressed())
        {
            AxesX = -1;
        }
        else if (inputActions.Gameplay.RightMove.IsPressed())
        {
            AxesX = 1;
        }
        else AxesX = 0;
    }

    public void EnableGameplay()
    {
        inputActions.Gameplay.Enable();
        //锁定鼠标
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetJumpInputBuffer()
    {
        //防止同一个协程重复开启
        StopCoroutine(nameof(JumpInputBufferCoroutine));
        StartCoroutine(nameof(JumpInputBufferCoroutine));
    }

    IEnumerator JumpInputBufferCoroutine()
    {
        JumpInputBuffer = true;

        yield return waitJumpInputBufferTime;

        JumpInputBuffer = false;
    }

    public void SetUPInputBuffer()
    {
        //防止同一个协程重复开启
        StopCoroutine(nameof(UpInputBufferCoroutine));
        StartCoroutine(nameof(UpInputBufferCoroutine));
    }

    IEnumerator UpInputBufferCoroutine()
    {
        UpInputBuffer = true;

        yield return waitUpInputBufferTime;

        UpInputBuffer = false;
    }

    public void SetShootInputBuffer()
    {
        //防止同一个协程重复开启
        StopCoroutine(nameof(ShootInputBufferCoroutine));
        StartCoroutine(nameof(ShootInputBufferCoroutine));
    }

    IEnumerator ShootInputBufferCoroutine()
    {
        ShootInputBuffer = true;

        //Instantiate(projectial, muzzle.position, Quaternion.identity);

        yield return waitShotInputBufferTime;

        ShootInputBuffer = false;
    }

    public void DisableGameplayInputs()
    {
        inputActions.Disable();
    }
}
