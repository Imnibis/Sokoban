using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class CrateTriggerable : MonoBehaviour
{
    public UnityEvent onTrigger;
    public UnityEvent onUntrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Crate") {
            onTrigger.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Crate") {
            onUntrigger.Invoke();
        }
    }
}
