using UnityEngine;
using System.Collections.Generic;

public class AlienAgent : MonoBehaviour
{
    public float speed = 2f;
    public float reachDist = 1f;
    public List<Vector3> patrolPath = new List<Vector3>();

    private int currentIndex = 0;

    void Start()
    {
        if (patrolPath.Count > 0)
            transform.position = patrolPath[0];
    }

    void Update()
    {
        MoveAlien();
    }

    void MoveAlien()
    {
        if (patrolPath.Count == 0) return;
        Vector3 target = patrolPath[currentIndex];
        target.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime);

        Vector3 dir = target - transform.position;
        if (dir.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                8f * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < reachDist)
            currentIndex = (currentIndex + 1) % patrolPath.Count;
    }
}