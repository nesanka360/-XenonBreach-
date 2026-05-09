using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AlienState { Patrol, HeightenedPatrol, Chase, ReturnHome }
public class AlienAgent : MonoBehaviour
{
    public WaypointNode homeNode;
    public WaypointNode playerTargetNode;
    public float reachDist = 0.8f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    private NavMeshAgent agent;
    private Animator anim;
    private List<WaypointNode> path;
    private int pathIdx = 0;
    private bool needsRepath = false;
    private AlienState state = AlienState.Patrol;
    void Start() { agent = GetComponent<NavMeshAgent>(); anim = GetComponent<Animator>(); CalculatePath(); }
    void Update()
    {
        if (needsRepath) { needsRepath = false; CalculatePath(); }
        FollowPath();
        if (anim != null) anim.SetFloat("Speed", agent.velocity.magnitude);
    }
    public void SetState(AlienState newState)
    {
        state = newState;
        agent.speed = (state == AlienState.Chase || state == AlienState.ReturnHome)
            ? chaseSpeed : patrolSpeed;
        CalculatePath();
    }
    public void RequestRepath() => needsRepath = true;
    void CalculatePath()
    {
        WaypointNode nearest = GetNearestNode();
        WaypointNode target;
        switch (state)
        {
            case AlienState.Chase:
                target = playerTargetNode ?? homeNode;
                path = AStarPathfinder.FindPath(nearest, target);
                break;
            case AlienState.ReturnHome:
                path = AStarPathfinder.FindPath(nearest, homeNode);
                break;
            default:
                WaypointNode patrolTarget = GetPatrolTarget();
                path = UCSPathfinder.FindPath(nearest, patrolTarget);
                break;
        }
        pathIdx = 0;
    }
    void FollowPath()
    {
        if (path == null || pathIdx >= path.Count) { CalculatePath(); return; }
        Vector3 tgt = path[pathIdx].transform.position;
        agent.SetDestination(tgt);
        if (Vector3.Distance(transform.position, tgt) < reachDist) pathIdx++;
        if (agent.velocity.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(agent.velocity.normalized), 8f * Time.deltaTime);
    }
    WaypointNode GetNearestNode()
    {
        WaypointNode best = null; float minD = float.MaxValue;
        foreach (var n in GraphManager.Instance.allNodes)
        { float d = Vector3.Distance(transform.position, n.transform.position);
            if (d < minD) { minD = d; best = n; } }
        return best;
    }
    WaypointNode GetPatrolTarget()
    {
        var nb = homeNode.neighbors;
        return nb.Count > 0 ? nb[Random.Range(0, nb.Count)] : homeNode;
    }
}