using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Transform WarpTarget;
    /// <summary>
    /// Ŀǰ���Ϊ���Ǵ�����������ѡ��ʱ�ҷ�����ײʱ������ȷ�����ǻὫ��ײ������Ϊ�������ͽ���,�Ҳ������ͱ�����Collider2D��
    /// </summary>
    /// <param name="other"></param>

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.transform.position = WarpTarget.transform.position;
    }
}
