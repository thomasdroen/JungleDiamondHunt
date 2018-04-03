using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{

    public GameObject[] openGates;

    public void toggleGate(bool unlocked)
    {
        foreach (var openGate in openGates)
        {
            openGate.SetActive(unlocked);
        }
        gameObject.SetActive(!unlocked);
    }
}
