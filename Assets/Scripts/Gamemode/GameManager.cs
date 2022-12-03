using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Level> levels;
    [HideInInspector] public List<Button> buttons;

    public LevelUI levelUI;
    public SceneTransitionUI sceneTransitionUI;
    public LevelFinishedUI levelFinishedUI;

    public int currentLevelIndex;
    public Level currentLevel;

    void Start()
    {
        if (levels.Count != 0) {
            currentLevelIndex = PlayerPrefs.GetInt("Level", 0);
            currentLevel = levels[currentLevelIndex];
            LoadCurrentLevel();
        }
    }

    public void RegisterButton(Button button)
    {
        buttons.Add(button);
    }

    public void CheckButtons()
    {
        foreach (Button button in buttons) {
            if (!button.triggered) {
                return;
            }
        }
        buttons.Clear();
        Debug.Log("All buttons were pressed");
        levelFinishedUI.onAfterHide += BeginSceneTransition;
        StartCoroutine(levelFinishedUI.Show());
    }

    void BeginSceneTransition()
    {
        Debug.Log("Beginning scene transition");
        levelFinishedUI.onAfterHide -= BeginSceneTransition;
        sceneTransitionUI.onAfterShow += EndLevel;
        StartCoroutine(sceneTransitionUI.Show());
    }

    public void EndLevel()
    {
        SceneManager.sceneUnloaded += OnLevelUnloaded;
        SceneManager.UnloadSceneAsync(currentLevel.sceneName);
    }

    void OnLevelUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnLevelUnloaded;
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count) {
            Debug.Log("No levels remaining");
            return;
        }
        PlayerPrefs.SetInt("Level", currentLevelIndex);
        PlayerPrefs.Save();
        currentLevel = levels[currentLevelIndex];
        LoadCurrentLevel();
    }

    void LoadCurrentLevel()
    {
        Debug.Log("Loading level: " + currentLevel.name);
        levelUI.SetLevel(currentLevelIndex + 1, currentLevel.name);
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.LoadSceneAsync(currentLevel.sceneName, LoadSceneMode.Additive);
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        StartCoroutine(sceneTransitionUI.Hide());
    }
}
