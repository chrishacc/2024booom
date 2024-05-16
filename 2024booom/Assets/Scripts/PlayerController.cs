using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerGroundDetector groundDetector;
    PlayerWallDetector wallDetector;
    PlayerInput input;
    new Rigidbody2D rigidbody;

    // 二段跳
    [Header("默认二段跳次数，除了这个参数，其他的不要碰")]
    public int JumpCount = 1;
    public int jumpCount;

    // 冲刺
    public int dashCount = 1;
    public bool canDash => Time.time - dashStartTime > dashCD;
    public float dashStartTime = -100.0f;
    public float dashCD = 0.5f;

    public bool IsGround => groundDetector.IsGround;
    public bool IsFalling => rigidbody.velocity.y < 0 && !IsGround;
    public float moveSpeed => Mathf.Abs(rigidbody.velocity.x);

    public bool IsWall => wallDetector.IsWall;

    public List<StarGem> starGems = new List<StarGem>();

    void Awake()
    {
        groundDetector = GetComponentInChildren<PlayerGroundDetector>();
        wallDetector = GetComponentInChildren<PlayerWallDetector>();
        input = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody2D>();

        AudioManager.Instance.PlaySound("BGM");
    }

    void Start()
    {
        input.EnableGameplay();

        //rigidbody.gravityScale = 0;
        //rigidbody.simulated = false;
    }

    public void Move(float speed)
    {
        if (input.Move)
            transform.localScale = new Vector2(0.225f * input.AxesX, transform.localScale.y);
        SetVelocityX(speed * input.AxesX);
    }

    public void SetVelocity(Vector2 velocity)
    {
        rigidbody.velocity = velocity;
    }

    public void SetVelocityX(float velocityX)
    {
        rigidbody.velocity = new Vector2(velocityX, rigidbody.velocity.y);
    }

    public void SetVelocityY(float velocityY)
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, velocityY);
    }

    public void ReSetStarGem()
    {
        foreach (var gem in starGems)
        {
            gem.Reset();
        }
        starGems.Clear();
    }
}
