using System.Collections;
using UnityEngine;

public class AlarmZoneManager : MonoBehaviour
{
    public float escalationTime = 8f;
    public float exitBufferTime = 3f;

    private Coroutine escalationRoutine;
    private Coroutine exitRoutine;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("[C] Player entered alarm zone");

        Debug.Log("[C] Guardian alien starts chasing");

        if (exitRoutine != null)
        {
            StopCoroutine(exitRoutine);
        }

        escalationRoutine = StartCoroutine(EscalationTimer());
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("[C] Player exited alarm zone");

        if (escalationRoutine != null)
        {
            StopCoroutine(escalationRoutine);
        }

        exitRoutine = StartCoroutine(ExitBuffer());
    }

    IEnumerator EscalationTimer()
    {
        yield return new WaitForSeconds(escalationTime);

        Debug.Log("[C] FULL ALERT - All aliens chasing player");
    }

    IEnumerator ExitBuffer()
    {
        yield return new WaitForSeconds(exitBufferTime);

        Debug.Log("[C] Alarm ended - aliens returning home");
    }
}