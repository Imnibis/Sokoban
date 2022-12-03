using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class TiledFloor : MonoBehaviour
{
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }


    void Update()
    {
        if (renderer) {
            renderer.material.mainTextureScale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
        }
    }
}
