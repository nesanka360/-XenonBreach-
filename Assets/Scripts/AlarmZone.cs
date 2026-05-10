using UnityEngine;

public class AlarmZone : MonoBehaviour
{
    public float radius = 15f;
    public Vector3[] alarmPositions = {
        new Vector3(-80, 0, -20),
        new Vector3(80, 0, -20),
        new Vector3(-80, 0, -147),
        new Vector3(80, 0, -147)
    };

    private AlienAgent[] aliens;
    private bool triggered = false;

    void Start()
    {
        aliens = FindObjectsOfType<AlienAgent>();
        Debug.Log("Aliens found: " + aliens.Length);
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        bool inZone = false;
        foreach (Vector3 zone in alarmPositions)
        {
            if (Vector3.Distance(player.transform.position, zone) < radius)
            {
                inZone = true;
                break;
            }
        }

        if (inZone && !triggered)
        {
            triggered = true;
            TriggerChase();
        }

        if (!inZone)
        {
            triggered = false;
        }
    }

    void TriggerChase()
    {
        Debug.Log("ALARM TRIGGERED!");
        foreach (AlienAgent alien in aliens)
            alien.StartChase();
    }
}