using System.Collections;
using UnityEngine;

public class Projectial : MonoBehaviour
{
    [SerializeField]float moveSpeed = 30f;

    [SerializeField]int moveDirection;

    

    void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        while(gameObject.activeSelf)
        {
            Vector2 dir = new Vector2(moveDirection, 0);
            transform.Translate(dir * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void SetDir(Vector2 dir)
    {
        if(dir.x > 0)
        {
            moveDirection = 1;
            //gameObject.transform.rotation = Quaternion.Euler(180, 0, 0);
            //gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(1, 1, 1);
            //gameObject.transform.localScale = new Vector3(1, 1, 1);
        }   
        else
        {
            moveDirection = -1;
            //gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(180, 0, 0);
            //gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(-1, 1, 1);
            //gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enter Collision");
        if(collision.gameObject.TryGetComponent<Explodable>(out Explodable explodable))
        {
            Debug.Log("Begin Explode");
            VirtualCamera.Instance.CameraShake();
            explodable.explode();
            ExplosionForce.instance.doExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
