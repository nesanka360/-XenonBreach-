using System.Collections;
using UnityEngine;

public class AlarmZoneManager : MonoBehaviour
{
    public float escalationTime = 0.5f; // Shortened for quick demo video response
    public float exitBufferTime = 15f;  // Keeps them aggressive long enough to record

    private Coroutine escalationRoutine;
    private Coroutine exitRoutine;

    void OnTriggerEnter(Collider other)
    {
        // Safety check: Only trip if the main player walks in
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("[ALARM SYSTEM] Tripwire activated by Player at: " + gameObject.name);

        // GLOBAL BROADCAST: Find every single AlienAgent in the scene and engage them
        AlienAgent[] allAliens = FindObjectsOfType<AlienAgent>();
        
        if (allAliens.Length == 0)
        {
            Debug.LogWarning("[ALARM SYSTEM] No active objects with AlienAgent found in the scene!");
        }

        foreach (AlienAgent alien in allAliens)
        {
            if (alien != null)
            {
                alien.StartChase();
                Debug.Log("[ALARM SYSTEM] Sent A* Chase command to: " + alien.gameObject.name);
            }
        }

        if (exitRoutine != null) StopCoroutine(exitRoutine);
        escalationRoutine = StartCoroutine(EscalationTimer());
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("[ALARM SYSTEM] Player stepped out of: " + gameObject.name);
        
        if (exitRoutine != null) StopCoroutine(exitRoutine);
        exitRoutine = StartCoroutine(ExitBuffer());
    }

    IEnumerator EscalationTimer()
    {
        yield return new WaitForSeconds(escalationTime);
        Debug.Log("[ALARM SYSTEM] FULL MAP ALERT - All sectors engaged.");
    }

    IEnumerator ExitBuffer()
    {
        yield return new WaitForSeconds(exitBufferTime);
        Debug.Log("[ALARM SYSTEM] Cool down complete. Agents returning via UCS path.");
    }
}