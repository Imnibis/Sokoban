using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[RequireComponent(typeof(Moveable))]
public class TouchHandler : MonoBehaviour
{
    [HideInInspector] public bool touching = false;
    [HideInInspector] public Vector2 stickPosition = Vector2.zero;
    [HideInInspector] public Vector2 stickDirection = Vector2.zero;

    Moveable player;
    TouchStickUI stickUI;
    TouchControl touch;
    Vector2 touchBeginPos;
    public float maxStickDistance = 200;

    void Start()
    {
        stickUI = FindObjectOfType<TouchStickUI>();
        player = GetComponent<Moveable>();
        touch = Touchscreen.current.primaryTouch;
    }

    void Update()
    {
        TouchPhase phase = touch.phase.ReadValue();

        if (phase == TouchPhase.Began && !touching) {
            touchBeginPos = touch.position.ReadValue();
            touching = true;
            if (stickUI != null) {
                stickUI.SetThumbPosition(Vector2.zero);
                stickUI.StartTouch(touchBeginPos);
            }
            Debug.Log("Began");
        }
        else if ((phase == TouchPhase.Ended || phase == TouchPhase.Canceled) && touching) {
            touching = false;
            stickPosition = Vector2.zero;
            stickDirection = Vector2.zero;
            if (stickUI != null) {
                stickUI.StopTouch();
            }
            Debug.Log("Ended");
        }
        else if ((phase == TouchPhase.Moved || phase == TouchPhase.Stationary) && touching) {
            Vector2 touchPos = touch.position.ReadValue();
            stickPosition = touchPos - touchBeginPos;
            stickPosition = Vector2.ClampMagnitude(stickPosition, maxStickDistance);
            stickDirection = GetStickDirection(stickPosition);
            player.TryMove(stickDirection);
            if (stickUI != null) {
                stickUI.SetThumbPosition(stickPosition);
            }
        }
    }

    Vector2 GetStickDirection(Vector2 stickPosition)
    {
        Vector2 absolutePos = new Vector2(
            Mathf.Abs(stickPosition.x),
            Mathf.Abs(stickPosition.y)
        );
        Vector2 normalizedAbsolutePos = absolutePos / maxStickDistance;

        if (normalizedAbsolutePos.x < 0.25f && normalizedAbsolutePos.y < 0.25f)
            return Vector2.zero;
        if (absolutePos.x > absolutePos.y) {
            return new Vector2(Mathf.Sign(stickPosition.x), 0);
        }
        else {
            return new Vector2(0, Mathf.Sign(stickPosition.y));
        }
    }
}
