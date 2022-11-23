using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public bool TryMove(Vector2 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(direction.x, 0, direction.y), out hit, 1)) {
            if (hit.collider.GetComponent<Moveable>() != null) {
                if (!hit.collider.GetComponent<Moveable>().TryMove(direction)) {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        transform.position += new Vector3(direction.x, 0, direction.y);
        return true;
    }

    // Can't assign a non-void method to a UnityEvent so here's a wrapper
    public void Move(Vector2 direction)
    {
        TryMove(direction);
    }
}
