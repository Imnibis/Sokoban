using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[RequireComponent(typeof(TrailRenderer)), RequireComponent(typeof(RectTransform))]
public class TouchTrail : MonoBehaviour
{
    public float movementThreshhold = 0.1f;

    TouchControl touch;
    TrailRenderer trailRenderer;
    RectTransform rectTransform;

    void Start()
    {
        touch = Touchscreen.current.primaryTouch;
        trailRenderer = GetComponent<TrailRenderer>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 touchPos = touch.position.ReadValue();
        Vector2 screenHeight = Screen.height * Vector2.up;
        rectTransform.anchoredPosition = touch.position.ReadValue() - screenHeight;
        trailRenderer.enabled = touch.phase.ReadValue() == TouchPhase.Moved
            && touch.delta.ReadValue().magnitude > movementThreshhold;
    }
}
