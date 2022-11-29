using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelNbText;
    public TextMeshProUGUI levelNameText;

    public void SetLevel(int nb, string name)
    {
        levelNbText.text = nb.ToString();
        levelNameText.text = name;
    }
}
