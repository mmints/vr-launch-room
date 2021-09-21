using UnityEngine;

public class Path : MonoBehaviour
{
    private const float wayPointGizmoRadius = 0.05f;
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(transform.GetChild(i).position, wayPointGizmoRadius);
            Gizmos.DrawLine(GetWayPoint(i).transform.position, GetWayPoint(j).transform.position);
        }
    }

    private int GetNextIndex(int i)
    {
        if (i + 1 == transform.childCount)
            return 0;

        return i+1;
    }
    
    public GameObject GetWayPoint(int i)
    {
        if (i >= transform.childCount) return  GetWayPoint(0);
        
        return transform.GetChild(i).gameObject;
    }

    public GameObject GetWayPoint(string name)
    {
        foreach (Transform waypoint in transform)
        {
            if (waypoint.gameObject.name == name)
                return waypoint.gameObject;
        }

        return null;
    }

    public GameObject GetStartPoint()
    {
        return transform.GetChild(0).gameObject;
    }
}
