using UnityEngine;

public class PrintInputControllers : MonoBehaviour
{
    void Start()
    {
        Debug.Log("= Active devices =");
        foreach(var device in InControl.InputManager.Devices)
        {
            Debug.LogFormat("[{0}] {1}", device.GetHashCode(), device.Name);
        }
    }
}
