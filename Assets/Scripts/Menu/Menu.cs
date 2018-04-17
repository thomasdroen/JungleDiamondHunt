using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    public string menuName;
    public abstract void OpenMenu();
    public abstract void CloseMenu();

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(menuName))
        {
            Debug.LogWarning(gameObject.name + " does not have a menu name!");
        }
    }
}
