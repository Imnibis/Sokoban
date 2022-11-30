using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public ParticleSystem teleportParticles;
    public Teleporter otherTeleporter;
    public float teleportDelay = 0.3f;
    [HideInInspector] public Moveable overlappingMoveable;

    bool isTeleporting = false;

    void OnTriggerEnter(Collider other)
    {
        Moveable moveable = other.GetComponent<Moveable>();

        if (moveable != null)
            overlappingMoveable = moveable;
        if (moveable != null && !isTeleporting) {
            StartCoroutine(PlayTeleportAnimation());
            StartCoroutine(otherTeleporter.PlayTeleportAnimation());
            StartCoroutine(Teleport(moveable));
            if (otherTeleporter.overlappingMoveable != null)
                StartCoroutine(otherTeleporter.Teleport(otherTeleporter.overlappingMoveable));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Moveable>() != null)
            overlappingMoveable = null;
    }

    public IEnumerator Teleport(Moveable moveable)
    {
        yield return new WaitForSeconds(teleportDelay);
        Vector3 relativePosition = moveable.transform.position - transform.position;
        moveable.transform.position = otherTeleporter.transform.position + relativePosition;
    }

    public IEnumerator PlayTeleportAnimation()
    {
        isTeleporting = true;
        teleportParticles.Play();
        yield return new WaitForSeconds(teleportParticles.main.duration);
        isTeleporting = false;
    }
}
