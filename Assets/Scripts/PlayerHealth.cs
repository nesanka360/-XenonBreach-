using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float drainRate = 15f;
    public float killRadius = 2f;
    public Slider healthBar;

    private float currentHealth;
    private AlienAgent[] aliens;

    void Start()
    {
        currentHealth = maxHealth;
        aliens = FindObjectsOfType<AlienAgent>();
    }

    void Update()
    {
        float nearestDist = GetNearestAlienDist();
        if (nearestDist < killRadius)
        {
            currentHealth -= drainRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            if (currentHealth <= 0f)
                Debug.Log("Player Dead!");
        }
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;
    }

    float GetNearestAlienDist()
    {
        float min = float.MaxValue;
        foreach (AlienAgent a in aliens)
        {
            float d = Vector3.Distance(transform.position, a.transform.position);
            if (d < min) min = d;
        }
        return min;
    }
}