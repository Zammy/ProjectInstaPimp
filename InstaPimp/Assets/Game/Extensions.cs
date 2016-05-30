using UnityEngine;

public static class Extensions
{
    public static float xAngleSigned(this Vector3 v1, Vector3 v2, Vector3 rotationAxis)
    {
        return Mathf.Atan2(
            Vector3.Dot(rotationAxis, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static void xLookAt(this Transform trans, Vector3 point)
    {
        Vector3 fromPosToPoint = point - trans.position;
        Vector3 dir = fromPosToPoint.normalized;
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public static RectTransform xToRect(this Transform trans)
    {
        return trans as RectTransform;
    }
}

