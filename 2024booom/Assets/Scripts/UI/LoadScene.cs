using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void LoadScene01()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene01");
    }

    public void LoadSceneDecompression()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DecompressionScene");
    }

    public void ExitGame()
    {
        // �������д�˳���Ϸ�Ĵ�
        Debug.Log("�˳���Ϸ"); // ����һ����ѡ�ĵ�����־

        // �ڱ༭��������ʱ������ͨ�������Play����ť�˳���Ϸ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڹ���Ӧ�ó���ʱ������ʹ�����´����˳���Ϸ
        Application.Quit();
#endif
    }
}
