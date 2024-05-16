using UnityEngine;

public class DashGhost : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float activeTime = 0.1f;
    private float startTime;
    new MeshRenderer renderer;
    [SerializeField] AnimationCurve curve;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = player.localScale;
        startTime = Time.time;

        renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        if (Time.time > (startTime + activeTime))
        {
            ObjectPool.Instance.Push(gameObject);
        }
        // Í¸Ã÷¶ÈµÝ¼õ
        Color color = renderer.material.color;
        color.a = curve.Evaluate(Time.time - startTime);
        renderer.material.color = color;
    }
}