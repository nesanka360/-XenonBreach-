// WaypointNode.cs — Person A (IS Student 1: Custom Graph Formulation)
// Place on empty GameObjects at canyon junctions in the Unity scene.
// Set 'neighbors' by dragging other WaypointNode objects in the Inspector.
// 'isHomeNode' marks alien patrol base positions.
// 'isComponentNode' marks machine component locations.
using System.Collections.Generic;
using UnityEngine;
public class WaypointNode : MonoBehaviour
{
public List<WaypointNode> neighbors = new List<WaypointNode>();
public bool isHomeNode = false;
public bool isComponentNode = false;
public string nodeName = "";
// Real-world 3D distance — used as edge weight in adjacency list
public float CostTo(WaypointNode other)
=> Vector3.Distance(transform.position, other.transform.position);
void OnDrawGizmos()
{
Gizmos.color = isComponentNode
? new Color(1f, 0.35f, 0.2f, 0.9f)
: isHomeNode
? new Color(0.2f, 0.85f, 0.65f, 0.9f)
: new Color(0.55f, 0.35f, 1f, 0.8f);
Gizmos.DrawSphere(transform.position, 0.35f);
Gizmos.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
foreach (var n in neighbors)
if (n != null) Gizmos.DrawLine(transform.position, n.transform.position);
}
}