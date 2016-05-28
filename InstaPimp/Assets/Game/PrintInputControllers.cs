using UnityEngine;
using System.Collections;

public class PrintInputControllers : MonoBehaviour
{
    void Start()
    {
        Debug.Log("= Active devices =");
        foreach(var device in InControl.InputManager.Devices)
        {
            Debug.Log(device.Name);
        }
    }
}
