using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class SceneTransitionUI : MonoBehaviour
{
    RectTransform rectTransform;
    public float transitionTime = 0.5f;
    public float hiddenOffset = 3500f;
    public AfterSceneTransitionShowCallback onAfterShow;
    public AfterSceneTransitionHideCallback onAfterHide;
    public bool autoHideOnStart = false;

    public delegate void AfterSceneTransitionShowCallback();
    public delegate void AfterSceneTransitionHideCallback();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (autoHideOnStart) {
            StartCoroutine(Hide());
        }
    }

    public IEnumerator Show()
    {
        float time = 0f;
        while (time < transitionTime) {
            time += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(hiddenOffset, 0f), Vector2.zero, time / transitionTime);
            yield return null;
        }
        rectTransform.anchoredPosition = Vector2.zero;
        if (onAfterShow != null) {
            onAfterShow();
        }
    }

    public IEnumerator Hide()
    {
        float time = 0f;
        while (time < transitionTime) {
            time += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(-hiddenOffset, 0f), time / transitionTime);
            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(hiddenOffset, 0f);
        if (onAfterHide != null) {
            onAfterHide();
        }
    }
}
