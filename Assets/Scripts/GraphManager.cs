using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager Instance;
    public List<WaypointNode> allNodes = new List<WaypointNode>();

    private Dictionary<WaypointNode, List<(WaypointNode n, float w)>> adj;

    void Awake()
    {
        Instance = this;
        BuildGraph();
    }

    public void BuildGraph()
    {
        adj = new Dictionary<WaypointNode, List<(WaypointNode, float)>>();
        foreach (var node in allNodes)
        {
            adj[node] = new List<(WaypointNode, float)>();
            foreach (var neighbor in node.neighbors)
                adj[node].Add((neighbor, node.CostTo(neighbor)));
        }
        Debug.Log("[GraphManager] Graph built: " + allNodes.Count + " nodes");
    }

    public void RemoveEdge(WaypointNode a, WaypointNode b)
    {
        if (adj.ContainsKey(a)) adj[a].RemoveAll(p => p.n == b);
        if (adj.ContainsKey(b)) adj[b].RemoveAll(p => p.n == a);
        Debug.Log("[GraphManager] Edge removed: " + a.nodeName + " <-> " + b.nodeName);
    }

    public void RestoreEdge(WaypointNode a, WaypointNode b)
    {
        if (adj.ContainsKey(a) && !adj[a].Exists(p => p.n == b))
            adj[a].Add((b, a.CostTo(b)));
        if (adj.ContainsKey(b) && !adj[b].Exists(p => p.n == a))
            adj[b].Add((a, b.CostTo(a)));
        Debug.Log("[GraphManager] Edge restored: " + a.nodeName + " <-> " + b.nodeName);
    }

    public List<(WaypointNode, float)> GetNeighbors(WaypointNode n)
    {
        return adj.ContainsKey(n) ? adj[n] : new List<(WaypointNode, float)>();
    }
}