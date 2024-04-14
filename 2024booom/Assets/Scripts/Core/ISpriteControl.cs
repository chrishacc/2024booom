using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������������ں��ⲿʵ�ֽ���
/// </summary>
public interface ISpriteControl
{
    void Trail(int face);

    void Scale(Vector2 localScale);

    void SetSpriteScale(Vector2 localScale);

    Vector3 SpritePosition { get; }

    void Slash(bool enable);

    void DashFlux(Vector2 dir, bool enable);
    //����ͷ����ɫ
    void SetHairColor(Color color);

    void WallSlide(Color color, Vector2 dir);
}