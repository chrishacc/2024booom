
using UnityEngine;

    /// <summary>
    /// ����ࣺ����
    /// 1�������ʾ��
    /// 2����ҿ����������Ŀ�������
    /// �������������ڲ����н���
    /// </summary>
    public class Player : MonoBehaviour
    {
        private PlayerRenderer playerRenderer;
        private PlayerController playerController;

        private IGameContext gameContext;

    public Player(IGameContext gameContext)
    {
        this.gameContext = gameContext;
    }

    //�������ʵ��
    public void Reload(Bounds bounds, Vector2 startPosition)
        {
            this.playerRenderer = Object.Instantiate(Resources.Load<PlayerRenderer>("PlayerRenderer"));
            //this.playerRenderer = AssetHelper.Create<PlayerRenderer>("Assets/ProPlatformer/_Prefabs/PlayerRenderer.prefab");
            //this.playerRenderer.Reload();

            //��ʼ��
            this.playerController = new PlayerController();
            this.playerController.Init(bounds, startPosition);

            PlayerParams playerParams = Resources.Load<PlayerParams>("PlayerParams");
            //PlayerParams playerParams = AssetHelper.LoadObject<PlayerParams>("Assets/ProPlatformer/PlayerParam.asset");
            playerParams.SetReloadCallback(() => this.playerController.RefreshAbility());
            playerParams.ReloadParams();
        }

        public void Update()
        {
            playerController.Update(Time.unscaledDeltaTime);
            Render();
        }

        private void Render()
        {
        playerRenderer.Render(Time.deltaTime);

        Vector2 scale = playerRenderer.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (int)playerController.Facing;
        playerRenderer.transform.localScale = scale;
        playerRenderer.transform.position = playerController.Position;

        //if (!lastFrameOnGround && this.playerController.OnGround)
        //{
        //    this.playerRenderer.PlayMoveEffect(true, this.playerController.GroundColor);
        //}
        //else if (lastFrameOnGround && !this.playerController.OnGround)
        //{
        //    this.playerRenderer.PlayMoveEffect(false, this.playerController.GroundColor);
        //}
        //this.playerRenderer.UpdateMoveEffect();

        this.lastFrameOnGround = this.playerController.OnGround;
        }

        private bool lastFrameOnGround;

        public Vector2 GetCameraPosition()
        {
            if (this.playerController == null)
            {
                return Vector3.zero;
            }
            return playerController.GetCameraPosition();
        }
    }
