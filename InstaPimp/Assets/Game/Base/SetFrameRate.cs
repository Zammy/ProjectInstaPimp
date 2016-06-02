using UnityEngine;
using System.Collections;

public class SetFrameRate : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
