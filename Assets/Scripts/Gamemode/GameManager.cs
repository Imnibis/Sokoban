using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum LevelTransitionPhase
{
    None,
    LevelFinishedAnimation,
    SceneTransitionAnimation,
    UnloadingScene,
    LoadingNextScene
}

public class GameManager : MonoBehaviour
{
    public List<Level> levels;
    [HideInInspector] public List<Button> buttons;

    public LevelUI levelUI;
    public SceneTransitionUI sceneTransitionUI;
    public LevelFinishedUI levelFinishedUI;
    public LevelFinishedUI theEndTitle;
    public LevelFinishedUI theEndSubtitle;

    public int currentLevelIndex;
    public Level currentLevel;

    LevelTransitionPhase levelTransitionPhase = LevelTransitionPhase.None;

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
        if (levelTransitionPhase != LevelTransitionPhase.None)
            return;
        foreach (Button button in buttons) {
            if (!button.triggered) {
                return;
            }
        }
        buttons.Clear();
        Debug.Log("All buttons were pressed");
        levelFinishedUI.onAfterHide += BeginSceneTransition;
        levelTransitionPhase = LevelTransitionPhase.LevelFinishedAnimation;
        StartCoroutine(levelFinishedUI.Show());
    }

    void BeginSceneTransition()
    {
        if (levelTransitionPhase != LevelTransitionPhase.LevelFinishedAnimation)
            return;
        Debug.Log("Beginning scene transition");
        levelFinishedUI.onAfterHide -= BeginSceneTransition;
        sceneTransitionUI.onAfterShow += EndLevel;
        levelTransitionPhase = LevelTransitionPhase.SceneTransitionAnimation;
        StartCoroutine(sceneTransitionUI.Show());
    }

    public void EndLevel()
    {
        if (levelTransitionPhase != LevelTransitionPhase.SceneTransitionAnimation)
            return;
        SceneManager.sceneUnloaded += OnLevelUnloaded;
        levelTransitionPhase = LevelTransitionPhase.UnloadingScene;
        SceneManager.UnloadSceneAsync(currentLevel.sceneName);
    }

    void OnLevelUnloaded(Scene scene)
    {
        if (levelTransitionPhase != LevelTransitionPhase.UnloadingScene)
            return;
        SceneManager.sceneUnloaded -= OnLevelUnloaded;
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        levelTransitionPhase = LevelTransitionPhase.LoadingNextScene;
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count) {
            theEndSubtitle.onAfterHide += LoadMainMenu;
            StartCoroutine(theEndTitle.Show());
            StartCoroutine(theEndSubtitle.Show());
            return;
        }
        PlayerPrefs.SetInt("Level", currentLevelIndex);
        PlayerPrefs.Save();
        currentLevel = levels[currentLevelIndex];
        LoadCurrentLevel();
    }

    void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
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
        levelTransitionPhase = LevelTransitionPhase.None;
        StartCoroutine(sceneTransitionUI.Hide());
    }
}
