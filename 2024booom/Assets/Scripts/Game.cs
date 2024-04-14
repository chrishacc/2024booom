using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EGameState
{
    Load,   //加载中
    Play,   //游戏中
    Pause,  //游戏暂停
    Fail,   //游戏失败
}
public class Game : MonoBehaviour, IGameContext
{
    public static Game Instance;

    [SerializeField]
    public Level level;
    //场景特效管理器
    //[SerializeField]
    //private SceneEffectManager sceneEffectManager;
    [SerializeField]
    private SceneCamera gameCamera;
    //玩家
    Player player;

    EGameState gameState;

    void Awake()
    {
        Debug.Log("Game Awake");
        Instance = this;

        gameState = EGameState.Load;

        player = new Player(this);
        Debug.Log("Game AwakeDowm");
    }

    IEnumerator Start()
    {
        Debug.Log("Game StartLoad");
        yield return null;//再多延迟一帧加载
        Debug.Log("Game Start");
        //加载玩家
        player.Reload(level.Bounds, level.StartPosition);
        this.gameState = EGameState.Play;
        Debug.Log("Game LoadDown");
        yield return null;
    }

    public void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        //Debug.Log("Game Update1");
        if (UpdateTime(deltaTime))
        {
            //Debug.Log("Game Update2");
            if (this.gameState == EGameState.Play)
            {
                //Debug.Log("Game Update3");
                GameInput.Update(deltaTime);
                //更新玩家逻辑数据
                player.Update();
                //更新摄像机
                gameCamera.SetCameraPosition(player.GetCameraPosition());
            }
        }
    }

    #region 冻帧
    private float freezeTime;

    //更新顿帧数据，如果不顿帧，返回true
    public bool UpdateTime(float deltaTime)
    {
        if (freezeTime > 0f)
        {
            freezeTime = Mathf.Max(freezeTime - deltaTime, 0f);
            return false;
        }
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        return true;
    }

    //冻帧
    public void Freeze(float freezeTime)
    {
        this.freezeTime = Mathf.Max(this.freezeTime, freezeTime);
        if (this.freezeTime > 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion
    public void CameraShake(Vector2 dir, float duration)
    {
        this.gameCamera.Shake(dir, duration);
    }

    //public IEffectControl EffectControl { get => this.sceneEffectManager; }

    //public ISoundControl SoundControl { get; }

}