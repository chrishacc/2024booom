using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicBarManager : MonoBehaviour
{
    public TextMeshProUGUI magicCountText;

    private void Update()
    {
        magicCountText.text = MagicLimit.Instance.magicCounts.ToString();
    }
}
