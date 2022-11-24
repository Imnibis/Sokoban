using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Button> buttons;

    public void RegisterButton(Button button)
    {
        buttons.Add(button);
    }

    public void CheckButtons()
    {
        foreach (Button button in buttons) {
            if (!button.triggered) {
                return;
            }
        }
        Debug.Log("All buttons are triggered!");
    }
}
