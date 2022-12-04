using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchStickUI : MonoBehaviour
{
    public Image touchCircle;
    public Image touchThumb;

    public void StartTouch(Vector2 position)
    {
        touchCircle.GetComponent<RectTransform>().anchoredPosition = position;
        touchCircle.enabled = true;
        touchThumb.enabled = true;
    }

    public void StopTouch()
    {
        touchCircle.enabled = false;
        touchThumb.enabled = false;
    }

    public void SetThumbPosition(Vector2 position)
    {
        touchThumb.GetComponent<RectTransform>().anchoredPosition = position;
    }
}
