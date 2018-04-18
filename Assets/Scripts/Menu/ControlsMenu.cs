using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : Menu
{

    public GameObject controlsMenu;

    public override void OpenMenu()
    {
        controlsMenu.SetActive(true);
    }

    public override void CloseMenu()
    {
        controlsMenu.SetActive(false);
    }
}
