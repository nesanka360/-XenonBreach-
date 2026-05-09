using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float killingZoneRadius = 20f;
    public float drainRate = 15f;
    public float recoveryRate = 5f;
    public float safeDistance = 8f;

    public Slider healthBar;
    public UnityEngine.UI.Image geigerImage;
    public AudioSource geigerAudio;

    public Color safeColor = Color.green;
    public Color dangerColor = Color.red;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI(false);

        Debug.Log("[C] Health system started");
    }

    void Update()
    {
        float nearestDist = GetNearestAlienDistance();

        bool inKillZone = nearestDist < killingZoneRadius;
        bool isSafe = nearestDist > safeDistance;

        if (inKillZone)
        {
            currentHealth -= drainRate * Time.deltaTime;
        }
        else if (isSafe)
        {
            currentHealth += recoveryRate * Time.deltaTime;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI(inKillZone);

        if (inKillZone)
        {
            Debug.Log("[C] Radiation danger! Health: " + currentHealth);
        }

        if (currentHealth <= 0f)
        {
            Debug.Log("[C] Player died from radiation exposure");
        }
    }

    void UpdateUI(bool danger)
    {
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;

        if (geigerImage != null)
            geigerImage.color = danger ? dangerColor : safeColor;

        if (geigerAudio != null)
        {
            if (danger && !geigerAudio.isPlaying)
                geigerAudio.Play();

            if (!danger && geigerAudio.isPlaying)
                geigerAudio.Stop();
        }
    }

    float GetNearestAlienDistance()
    {
        float minDistance = float.MaxValue;

        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien");

        foreach (GameObject alien in aliens)
        {
            float distance = Vector3.Distance(transform.position, alien.transform.position);

            if (distance < minDistance)
                minDistance = distance;
        }

        return minDistance;
    }
}