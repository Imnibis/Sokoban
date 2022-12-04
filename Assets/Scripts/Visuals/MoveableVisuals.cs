using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableVisuals : MonoBehaviour
{
    public float moveDuration = 0.15f;
    public ParticleSystem moveParticles;
    public ParticleSystem moveFailParticles;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve resizeCurve;

    Moveable moveable;

    void Start()
    {
        moveable = GetComponentInParent<Moveable>();
    }

    public IEnumerator BeginMoveAnimation(Vector2 direction)
    {
        Vector3 endPosition = new Vector3(direction.x, 0, direction.y);
        Vector3 smearScale = new Vector3(Mathf.Abs(direction.x), 0, Mathf.Abs(direction.y));
        float time = 0;
        moveable.canMove = false;
        if (moveParticles != null) {
            moveParticles.transform.localRotation = Quaternion.LookRotation(-endPosition);
            moveParticles.Play();
        }
        while (time < moveDuration) {
            transform.localPosition = Vector3.Lerp(Vector3.zero, endPosition, moveCurve.Evaluate(time / moveDuration));
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one + smearScale,
                0.5f - Mathf.Abs(moveCurve.Evaluate(time / moveDuration) - 0.5f));
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        moveable.transform.position += endPosition;
        moveable.movementTimer = moveable.movementCooldown;
        moveable.movementTimerCountingDown = true;
    }

    public IEnumerator BeginMoveFailAnimation(Vector2 direction)
    {
        Vector3 endPosition = new Vector3(direction.x / 3, 0, direction.y / 3);
        float time = 0;
        moveable.canMove = false;
        if (moveParticles != null) {
            moveParticles.transform.localRotation = Quaternion.LookRotation(-endPosition);
            moveParticles.Play();
        }
        while (time < moveDuration / 3) {
            transform.localPosition = Vector3.Lerp(Vector3.zero, endPosition, moveCurve.Evaluate(time / moveDuration));
            time += Time.deltaTime;
            yield return null;
        }
        if (moveFailParticles != null) {
            moveFailParticles.transform.localRotation = Quaternion.LookRotation(endPosition);
            moveFailParticles.Play();
        }
        time = 0;
        while (time < moveDuration / 3) {
            transform.localPosition = Vector3.Lerp(endPosition, Vector3.zero, moveCurve.Evaluate(time / moveDuration));
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        moveable.movementTimer = moveable.movementCooldown;
        moveable.movementTimerCountingDown = true;
    }
}
