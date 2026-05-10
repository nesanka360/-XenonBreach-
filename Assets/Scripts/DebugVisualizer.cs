using System.Collections.Generic;
using UnityEngine;

public class DebugVisualizer : MonoBehaviour
{
    public bool showGraph = true;
    public bool showFrontier = false;
    private List<WaypointNode> ucsFrontier = new List<WaypointNode>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) showGraph = !showGraph;
        if (Input.GetKeyDown(KeyCode.F))
        {
            showFrontier = !showFrontier;
            if (showFrontier) RunUCSVisualization();
            else ucsFrontier.Clear();
        }
    }

    void OnDrawGizmos()
    {
        if (GraphManager.Instance == null) return;
        if (showGraph)
        {
            Gizmos.color = new Color(0.1f, 0.85f, 0.65f, 0.5f);
            foreach (var node in GraphManager.Instance.allNodes)
            {
                if (node == null) continue;
                foreach (var (nb, _) in GraphManager.Instance.GetNeighbors(node))
                    Gizmos.DrawLine(node.transform.position, nb.transform.position);
            }
            foreach (var node in GraphManager.Instance.allNodes)
            {
                if (node == null) continue;
                Gizmos.color = node.isComponentNode
                    ? new Color(1f, 0.35f, 0.2f, 0.9f)
                    : new Color(0.55f, 0.35f, 1f, 0.9f);
                Gizmos.DrawSphere(node.transform.position, 0.4f);
            }
        }
        if (showFrontier)
        {
            Gizmos.color = new Color(1f, 0.75f, 0.1f, 0.8f);
            foreach (var n in ucsFrontier)
                Gizmos.DrawWireSphere(n.transform.position, 0.6f);
        }
    }

    void RunUCSVisualization()
    {
        ucsFrontier.Clear();
        AlienAgent alien = FindObjectOfType<AlienAgent>();
        if (alien == null) return;
        var nodes = GraphManager.Instance.allNodes;
        var path = UCSPathfinder.FindPath(
            nodes[0], nodes[nodes.Count - 1]);
        if (path != null) ucsFrontier.AddRange(path);
    }
}