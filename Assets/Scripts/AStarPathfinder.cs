using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinder
{
    public static List<WaypointNode> FindPath(WaypointNode start, WaypointNode goal)
    {
        if (start == null || goal == null) return null;
        var openSet = new List<WaypointNode> { start };
        var cameFrom = new Dictionary<WaypointNode, WaypointNode>();
        var gScore = new Dictionary<WaypointNode, float>();
        var fScore = new Dictionary<WaypointNode, float>();

        foreach (var node in GraphManager.Instance.allNodes)
        {
            if (node == null) continue;
            gScore[node] = float.MaxValue;
            fScore[node] = float.MaxValue;
        }

        gScore[start] = 0f;
        fScore[start] = Vector3.Distance(
            start.transform.position, goal.transform.position);

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(n => fScore[n]).First();
            if (current == goal) return Reconstruct(cameFrom, current);
            openSet.Remove(current);

            foreach (var (nb, cost) in GraphManager.Instance.GetNeighbors(current))
            {
                if (nb == null) continue;
                float tg = gScore[current] + cost;
                if (tg < gScore[nb])
                {
                    cameFrom[nb] = current;
                    gScore[nb] = tg;
                    fScore[nb] = tg + Vector3.Distance(
                        nb.transform.position, goal.transform.position);
                    if (!openSet.Contains(nb)) openSet.Add(nb);
                }
            }
        }
        return null;
    }

    static List<WaypointNode> Reconstruct(
        Dictionary<WaypointNode, WaypointNode> cf, WaypointNode cur)
    {
        var path = new List<WaypointNode> { cur };
        while (cf.ContainsKey(cur)) { cur = cf[cur]; path.Insert(0, cur); }
        return path;
    }
}