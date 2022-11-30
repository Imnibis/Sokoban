using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterVisuals : MonoBehaviour
{
    public float rotationSpeed = 1f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }
}
