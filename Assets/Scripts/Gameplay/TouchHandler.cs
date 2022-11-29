using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchHandler : MonoBehaviour
{
    public UnityEvent<Vector2> onSwipe;

    TouchControl touch;
    Vector2 touchBeginPos;
    float swipeDistance;
    bool swiping = false;

    void Start()
    {
        swipeDistance = Mathf.Min(15, Screen.height / 0.01f);
        touch = Touchscreen.current.primaryTouch;
    }

    void Update()
    {
        TouchPhase phase = touch.phase.ReadValue();

        if (phase == TouchPhase.Began) {
            touchBeginPos = touch.position.ReadValue();
            swiping = true;
        }
        else if (phase == TouchPhase.Ended && swiping) {
            Vector2 touchEndPos = touch.position.ReadValue();
            Vector2 swipe = touchEndPos - touchBeginPos;
            swiping = false;

            if (swipe.magnitude > swipeDistance) {
                onSwipe.Invoke(GetSwipeDirection(swipe));
            }
        }
    }

    Vector2 GetSwipeDirection(Vector2 swipe)
    {
        Vector2 absoluteSwipe = new Vector2(
            Mathf.Abs(swipe.x),
            Mathf.Abs(swipe.y)
        );

        if (absoluteSwipe.x > absoluteSwipe.y) {
            return new Vector2(Mathf.Sign(swipe.x), 0);
        }
        else {
            return new Vector2(0, Mathf.Sign(swipe.y));
        }
    }
}
