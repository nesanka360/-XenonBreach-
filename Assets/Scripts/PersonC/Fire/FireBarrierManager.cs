using UnityEngine;

public class FireBarrierManager : MonoBehaviour
{
    public float barrierDuration = 5f;
    public float maxEdgeDetectDistance = 10f;

    private WaypointNode blockedNodeA;
    private WaypointNode blockedNodeB;

    private bool activated = false;

    void OnCollisionEnter(Collision collision)
    {
        if (activated) return;

        activated = true;

        Debug.Log("[C] FIRE BARRIER CREATED");

        bool foundEdge = FindNearestConnectedEdge();

        if (foundEdge)
        {
            GraphManager.Instance.RemoveEdge(blockedNodeA, blockedNodeB);

            Debug.Log("[C] Auto blocked edge: "
                + blockedNodeA.nodeName + " <-> " + blockedNodeB.nodeName);
        }
        else
        {
            Debug.LogWarning("[C] No nearby connected graph edge found for fire barrier");
        }

        Invoke(nameof(RestoreEdge), barrierDuration);
    }

    bool FindNearestConnectedEdge()
    {
        if (GraphManager.Instance == null)
        {
            Debug.LogWarning("[C] GraphManager missing");
            return false;
        }

        float bestDistance = float.MaxValue;

        foreach (WaypointNode node in GraphManager.Instance.allNodes)
        {
            if (node == null) continue;

            foreach (var neighborData in GraphManager.Instance.GetNeighbors(node))
            {
                WaypointNode neighbor = neighborData.Item1;

                if (neighbor == null) continue;

                float distance = DistancePointToLineSegment(
                    transform.position,
                    node.transform.position,
                    neighbor.transform.position
                );

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    blockedNodeA = node;
                    blockedNodeB = neighbor;
                }
            }
        }

        if (blockedNodeA == null || blockedNodeB == null)
            return false;

        Debug.Log("[C] Nearest edge distance: " + bestDistance);

        return bestDistance <= maxEdgeDetectDistance;
    }

    float DistancePointToLineSegment(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 line = lineEnd - lineStart;
        float lineLength = line.sqrMagnitude;

        if (lineLength == 0f)
            return Vector3.Distance(point, lineStart);

        float t = Vector3.Dot(point - lineStart, line) / lineLength;
        t = Mathf.Clamp01(t);

        Vector3 closestPoint = lineStart + t * line;

        return Vector3.Distance(point, closestPoint);
    }

    void RestoreEdge()
    {
        if (GraphManager.Instance != null && blockedNodeA != null && blockedNodeB != null)
        {
            GraphManager.Instance.RestoreEdge(blockedNodeA, blockedNodeB);

            Debug.Log("[C] Auto restored edge: "
                + blockedNodeA.nodeName + " <-> " + blockedNodeB.nodeName);
        }

        Destroy(gameObject);
    }
}