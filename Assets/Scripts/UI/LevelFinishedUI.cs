using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishedUI : MonoBehaviour
{
    RectTransform rectTransform;
    public float transitionTime = 0.5f;
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float waitTime = 1f;
    public AfterLevelFinishedHideCallback onAfterHide;
    Vector2 originalScale;

    public delegate void AfterLevelFinishedHideCallback();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        rectTransform.localScale = Vector2.zero;
    }

    public IEnumerator Show()
    {
        float time = 0;
        while (time < transitionTime) {
            time += Time.deltaTime;
            rectTransform.localScale = Vector2.Lerp(Vector2.zero, originalScale,
                transitionCurve.Evaluate(time / transitionTime));
            yield return null;
        }
        rectTransform.localScale = originalScale;
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(Hide());
    }

    public IEnumerator Hide()
    {
        float time = 0;
        while (time < transitionTime) {
            time += Time.deltaTime;
            rectTransform.localScale = Vector2.Lerp(originalScale, Vector2.zero,
                transitionCurve.Evaluate(time / transitionTime));
            yield return null;
        }
        rectTransform.localScale = Vector2.zero;
        if (onAfterHide != null) {
            onAfterHide();
        }
    }
}
