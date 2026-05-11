using UnityEngine;
using System.Collections.Generic;

public class AlienAgent : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float reachDist = 3f;
    public float chaseDuration = 12f;
    public float recalcInterval = 2f;
    public WaypointNode homeNode;
    public List<Vector3> patrolPath = new List<Vector3>();

    private int currentIndex = 0;
    private bool isChasing = false;
    private float chaseTimer = 0f;
    private Transform player;

    private List<WaypointNode> astarPath = new List<WaypointNode>();
    private int astarIndex = 0;

    private List<WaypointNode> patrolNodePath = new List<WaypointNode>();
    private int patrolNodeIndex = 0;
    private bool usingNodePatrol = false;

    private float recalcTimer = 0f;

    void Start()
    {
        if (patrolPath.Count > 0)
            transform.position = patrolPath[0];
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (GraphManager.Instance == null) return;

        if (isChasing)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0f)
            {
                isChasing = false;
                astarPath.Clear();
                ReturnToPatrolViaNodes();
            }
            else
            {
                ChaseWithAStar();
            }
        }
        else if (usingNodePatrol)
        {
            FollowNodePatrol();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPath.Count == 0) return;
        Vector3 target = patrolPath[currentIndex];
        target.y = transform.position.y;
        MoveToward(target, patrolSpeed);
        if (Vector3.Distance(transform.position, target) < reachDist)
            currentIndex = (currentIndex + 1) % patrolPath.Count;
    }

    void ReturnToPatrolViaNodes()
    {
        // Safety check if the path is empty
        if (patrolPath == null || patrolPath.Count == 0)
        {
            Debug.LogWarning(gameObject.name + " has an empty patrolPath list!");
            return;
        }

        WaypointNode startNode = GetNearestNode(transform.position);
        int index = GetNearestPatrolIndex();
        
        // Force the index to stay safe between 0 and the final element
        index = Mathf.Clamp(index, 0, patrolPath.Count - 1);
        Vector3 patrolTargetPosition = patrolPath[index];

        WaypointNode nearestPatrolNode = GetNearestNode(patrolTargetPosition);

        if (startNode != null && nearestPatrolNode != null)
        {
            patrolNodePath = UCSPathfinder.FindPath(startNode, nearestPatrolNode);
            patrolNodeIndex = 0;
            usingNodePatrol = true;
            Debug.Log(gameObject.name + " returning via UCS path!");
        }
    }

    void FollowNodePatrol()
    {
        if (patrolNodePath == null || patrolNodeIndex >= patrolNodePath.Count)
        {
            usingNodePatrol = false;
            
            if (patrolPath != null && patrolPath.Count > 0)
            {
                currentIndex = Mathf.Clamp(GetNearestPatrolIndex(), 0, patrolPath.Count - 1);
            }
            else
            {
                currentIndex = 0;
            }
            
            Debug.Log(gameObject.name + " resumed normal patrol!");
            return;
        }

        Vector3 target = patrolNodePath[patrolNodeIndex].transform.position;
        target.y = transform.position.y;
        MoveToward(target, patrolSpeed);

        if (Vector3.Distance(transform.position, target) < reachDist)
            patrolNodeIndex++;
    }
    
    void ChaseWithAStar()
    {
        if (player == null) return;

        recalcTimer -= Time.deltaTime;

        WaypointNode startNode = GetNearestNode(transform.position);
        WaypointNode goalNode = GetNearestNode(player.position);

        // Move directly to player when close
        if (startNode == goalNode ||
            Vector3.Distance(transform.position, player.position) < 5f)
        {
            MoveToward(player.position, chaseSpeed);
            return;
        }

        // Recalculate path periodically
        if (recalcTimer <= 0f || astarPath == null ||
            astarPath.Count == 0 || astarIndex >= astarPath.Count)
        {
            if (startNode != null && goalNode != null)
            {
                astarPath = AStarPathfinder.FindPath(startNode, goalNode);
                recalcTimer = recalcInterval;
                astarIndex = 0;
                Debug.Log(gameObject.name + " A* path: " + astarPath?.Count);
            }
            // NO return — continue to follow path
        }

        // Follow A* path
        if (astarPath == null || astarIndex >= astarPath.Count) return;
        Vector3 target = astarPath[astarIndex].transform.position;
        target.y = transform.position.y;
        MoveToward(target, chaseSpeed);
        if (Vector3.Distance(transform.position, target) < reachDist)
            astarIndex++;
    }

    WaypointNode GetNearestNode(Vector3 position)
    {
        if (GraphManager.Instance == null) return null;
        WaypointNode best = null;
        float minD = float.MaxValue;
        foreach (var n in GraphManager.Instance.allNodes)
        {
            if (n == null) continue;
            float d = Vector3.Distance(position, n.transform.position);
            if (d < minD) { minD = d; best = n; }
        }
        return best;
    }

    void MoveToward(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime);
        Vector3 dir = target - transform.position;
        if (dir.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                8f * Time.deltaTime);
    }

    int GetNearestPatrolIndex()
    {
        if (patrolPath == null || patrolPath.Count == 0) return 0; 

        int best = 0;
        float minD = float.MaxValue;
        for (int i = 0; i < patrolPath.Count; i++)
        {
            float d = Vector3.Distance(transform.position, patrolPath[i]);
            if (d < minD) { minD = d; best = i; }
        }
        return best;
    }

    public void StartChase()
    {
        if (isChasing) return;
        Debug.Log(gameObject.name + " StartChase called!");
        isChasing = true;
        chaseTimer = chaseDuration;
        astarPath = null;
        astarIndex = 0;
        recalcTimer = 0f;
        usingNodePatrol = false;
    }
}