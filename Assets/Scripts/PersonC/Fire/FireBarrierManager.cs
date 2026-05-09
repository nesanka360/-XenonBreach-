using UnityEngine;

public class FireBarrierManager : MonoBehaviour
{
    public float barrierDuration = 5f;

    private bool activated = false;

    void OnCollisionEnter(Collision collision)
    {
        if (activated) return;

        activated = true;

        Debug.Log("[C] FIRE BARRIER CREATED");

        SimulateEdgeRemoval();

        Invoke(nameof(RestoreEdge), barrierDuration);
    }

    void SimulateEdgeRemoval()
    {
        Debug.Log("[C] Graph edge REMOVED");
        Debug.Log("[C] Aliens recalculating path...");
    }

    void RestoreEdge()
    {
        Debug.Log("[C] Graph edge RESTORED");
    }
}