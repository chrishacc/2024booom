using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EGameState
{
    Load,   //������
    Play,   //��Ϸ��
    Pause,  //��Ϸ��ͣ
    Fail,   //��Ϸʧ��
}
public class Game : MonoBehaviour, IGameContext
{
    public static Game Instance;

    [SerializeField]
    public Level level;
    //������Ч������
    //[SerializeField]
    //private SceneEffectManager sceneEffectManager;
    [SerializeField]
    private SceneCamera gameCamera;
    //���
    Player player;

    EGameState gameState;

    void Awake()
    {
        Instance = this;

        gameState = EGameState.Load;

        player = new Player(this);
    }

    IEnumerator Start()
    {
        yield return null;

        //�������
        player.Reload(level.Bounds, level.StartPosition);
        this.gameState = EGameState.Play;
        yield return null;
    }

    public void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        if (UpdateTime(deltaTime))
        {
            if (this.gameState == EGameState.Play)
            {
                GameInput.Update(deltaTime);
                //��������߼�����
                player.Update(deltaTime);
                //���������
                gameCamera.SetCameraPosition(player.GetCameraPosition());
            }
        }
    }

    #region ��֡
    private float freezeTime;

    //���¶�֡���ݣ��������֡������true
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

    //��֡
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