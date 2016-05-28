using UnityEngine;
using System.Collections;

public class TwoModeObject : MonoBehaviour
{
    public Material PlayMat;
    public Material PreMat;

    void Start()
    {
        SwitchToPre();
    }

    public void SwitchToPlay()
    {
        this.GetComponent<MeshRenderer>().material = PlayMat;
    }

    public void SwitchToPre()
    {
        this.GetComponent<MeshRenderer>().material = PreMat;
    }
}
