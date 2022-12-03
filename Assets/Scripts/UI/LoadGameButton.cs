using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UIButton = UnityEngine.UI.Button;

[RequireComponent(typeof(UIButton))]
public class LoadGameButton : MonoBehaviour
{
    public SceneTransitionUI transitionUI;
    public bool isContinueButton = false;

    void Start()
    {
        if (isContinueButton && PlayerPrefs.HasKey("Level")) {
            GetComponent<UIButton>().interactable = true;
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.Save();
        ContinueGame();
    }

    public void ContinueGame()
    {
        transitionUI.onAfterShow += LoadScene;
        StartCoroutine(transitionUI.Show());
    }

    void LoadScene()
    {
        transitionUI.onAfterShow -= LoadScene;
        SceneManager.LoadScene("BaseScene");
    }
}
