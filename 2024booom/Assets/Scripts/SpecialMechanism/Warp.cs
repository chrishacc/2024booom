using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Transform WarpTarget;
    /// <summary>
    /// 目前理解为“是触发器”被勾选上时且发生碰撞时触发，确定的是会将碰撞对象作为参数传送进来,且参数类型必须是Collider2D。
    /// </summary>
    /// <param name="other"></param>

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.transform.position = WarpTarget.transform.position;
    }
}
