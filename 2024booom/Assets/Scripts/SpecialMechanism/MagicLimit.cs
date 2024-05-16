using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicLimit : MonoBehaviour
{
    public static MagicLimit Instance;

    public int magicCounts = 2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddMagic()
    {
        magicCounts++;
        if (magicCounts >= 3)
        {
            magicCounts = 3;
        }
    }

    public void DecreaseMagic()
    {
        magicCounts--;
        if (magicCounts <= 0)
        {
            magicCounts = 0;
        }
    }

    public int GetMagicCounts()
    {
        return magicCounts;
    }

    public void LoadSucessScene()
    {
        Invoke("LoadSucScene", 4.0f);
    }

    public void LoadSucScene()
    {
        SceneManager.LoadScene("SuccessScene");
    }

    }
