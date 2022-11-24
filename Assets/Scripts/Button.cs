using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrateTriggerable))]
public class Button : MonoBehaviour
{
    public bool triggered = false;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Trigger()
    {
        triggered = true;
        gameManager.CheckButtons();
    }

    public void Untrigger()
    {
        triggered = false;
    }
}
