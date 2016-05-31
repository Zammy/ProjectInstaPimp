using UnityEngine;

public class RailShot : MonoBehaviour
{
    public LineRenderer LineRenderer;

    public Material Material
    {
        set
        {
            LineRenderer.material = value;
        }
    }

    public void Mark(Transform nozzle)
    {
        LineRenderer.SetPosition(0, nozzle.position);
        LineRenderer.SetPosition(1, nozzle.position + nozzle.up);
        LineRenderer.SetWidth(0.5f, 0.01f);
    }

    public void Shoot(Transform nozzle)
    {
        LineRenderer.SetWidth(0.1f, 0.1f);

        RaycastHit hit;
        if (!Physics.Raycast(nozzle.position, nozzle.up, out hit))
        {
            Debug.LogError("Raycasted and hit nothing!");
        }

        LineRenderer.SetPosition(0, nozzle.position);
        LineRenderer.SetPosition(1, hit.point);
    }
}
