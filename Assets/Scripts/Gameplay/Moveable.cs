using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public float movementCooldown = 0;
    [HideInInspector] public float movementTimer = 0;
    [HideInInspector] public bool movementTimerCountingDown = false;
    [HideInInspector] public bool canMove = true;

    MoveableVisuals visuals;

    void Start()
    {
        visuals = GetComponentInChildren<MoveableVisuals>();
    }

    void Update()
    {
        if (movementTimerCountingDown) {
            movementTimer -= Time.deltaTime;
            if (movementTimer <= 0) {
                movementTimerCountingDown = false;
                canMove = true;
            }
        }
    }

    public bool TryMove(Vector2 direction, bool first = true)
    {
        RaycastHit hit;

        if (!canMove || (direction.x == 0 && direction.y == 0))
            return false;
        if (Physics.Raycast(transform.position, new Vector3(direction.x, 0, direction.y), out hit, 1)) {
            if (hit.collider.GetComponent<Moveable>() == null
                || !hit.collider.GetComponent<Moveable>().TryMove(direction, false)) {
                if (first)
                    StartCoroutine(visuals.BeginMoveFailAnimation(direction));
                return false;
            }
        }
        StartCoroutine(visuals.BeginMoveAnimation(direction));
        return true;
    }

    // Can't assign a non-void method to a UnityEvent so here's a wrapper
    public void Move(Vector2 direction)
    {
        TryMove(direction);
    }
}
