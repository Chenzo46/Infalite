using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGUI : MonoBehaviour
{
    [SerializeField] private LevelManager lvlManager;
    [SerializeField]private TMP_Text levelText;

    public void setText()
    {
        levelText.text = lvlManager.getLevelsBeat().ToString();
    }
}
