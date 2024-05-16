using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform mainCameraTrans; // ���������Transform���
    private Vector3 lastCameraPosition; // ��һ֡�������λ��
    private float textureUnitSizeX; // ����ͼ��λ�ߴ�

    public Vector2 followWeight; // �����������Ȩ��
                                 // ����ԽԶ������Ȩ��Խ�ߣ��� ��ա��ơ�̫���� ����0.8-1��ΧЧ���п�
                                 // ����Խ��������Ȩ��Խ�ͣ��� ��ߵ���ľ�����ݡ����ӵȵ�

    void Start()
    {
        mainCameraTrans = Camera.main.transform; // ��ȡ���������Transform���
        lastCameraPosition = mainCameraTrans.position; // ��ʼ����һ֡�������λ��Ϊ��ǰ�������λ��

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture; // ��ȡSprite������
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit; // ���㱳��ͼ����Ϸ������ĵ�λ�ߴ�
    }
    void Update()
    {

    }
    private void LateUpdate()
    {
        ImageFollowCamera();
        ResetImageX();

        lastCameraPosition = mainCameraTrans.position; // ������һ֡�������λ��
    }

    private void ResetImageX()
    {
        // ����Ƿ���Ҫ�ƶ�����
        if (Mathf.Abs(mainCameraTrans.position.x - transform.position.x) >= textureUnitSizeX)
        {
            // ���ñ���λ��
            transform.position = new Vector3(mainCameraTrans.position.x, transform.position.y, transform.position.z);
        }
    }

    private void ImageFollowCamera()
    {
        // ���������λ�õ�ƫ����
        Vector3 offsetPosition = mainCameraTrans.position - lastCameraPosition;

        // ����Ȩ�ص�������ͼƬ��λ��
        transform.position += new Vector3(offsetPosition.x * followWeight.x, offsetPosition.y * followWeight.y, 0);
    }
}