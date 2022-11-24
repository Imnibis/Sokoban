using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableVisuals : MonoBehaviour
{
    public float moveDuration = 0.15f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Moveable moveable;

    void Start()
    {
        moveable = GetComponentInParent<Moveable>();
    }

    public IEnumerator BeginMoveAnimation(Vector2 direction)
    {
        Vector3 endPosition = new Vector3(direction.x, 0, direction.y);
        float time = 0;
        moveable.canMove = false;
        while (time < moveDuration) {
            transform.localPosition = Vector3.Lerp(Vector3.zero, endPosition, moveCurve.Evaluate(time / moveDuration));
            time += Time.deltaTime;
            yield return null;
        }
        moveable.canMove = true;
        transform.localPosition = Vector3.zero;
        moveable.transform.position += endPosition;
    }

    public IEnumerator BeginMoveFailAnimation(Vector2 direction)
    {
        Vector3 endPosition = new Vector3(direction.x / 3, 0, direction.y / 3);
        float time = 0;
        moveable.canMove = false;
        while (time < moveDuration / 3) {
            transform.localPosition = Vector3.Lerp(Vector3.zero, endPosition, moveCurve.Evaluate(time / moveDuration));
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        while (time < moveDuration / 3) {
            transform.localPosition = Vector3.Lerp(endPosition, Vector3.zero, moveCurve.Evaluate(time / moveDuration));
            time += Time.deltaTime;
            yield return null;
        }
        moveable.canMove = true;
        transform.localPosition = Vector3.zero;
    }
}
