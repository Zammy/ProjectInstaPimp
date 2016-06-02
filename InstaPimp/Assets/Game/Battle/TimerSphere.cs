using UnityEngine;
using System.Collections;

public class TimerSphere : MonoBehaviour
{
    public Transform SphereTrans;
    public Transform KeepAtPos;

    public Material SphereMaterial
    {
        set
        {
            SphereTrans.GetComponent<MeshRenderer>().material = value;
        }
    }

    Vector3 startScale;
    float startTime;

    void Start()
    {
        startScale = SphereTrans.transform.localScale;
        SphereTrans.transform.localScale = Vector3.zero;
    }

    void FixedUpdate()
    {
        SphereTrans.transform.position = KeepAtPos.transform.position;
    }

    public void ResetTimeLeft(float startTime)
    {
        this.startTime = startTime;
        SphereTrans.transform.localScale = startScale;
    }

    public void DisplayTimeLeft(float time)
    {
        var scaleVec = this.startScale;
        var scale = time / startTime;
        scaleVec *= scale;

        SphereTrans.transform.localScale = scaleVec;
    }
}
