using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public float killingZoneRadius = 5f;
    public float drainRate = 15f;
    public float recoveryRate = 5f;
    public float safeDistance = 10f;

    public Slider healthBar;

    public GameObject gameOverPanel;

    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateHealthBar();

        Debug.Log("[C] Health system started");
    }

    void Update()
    {
        if (isDead) return;

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

        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            GameOver();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    float GetNearestAlienDistance()
    {
        float minDistance = float.MaxValue;

        GameObject[] aliens = GameObject.FindGameObjectsWithTag("Alien");

        foreach (GameObject alien in aliens)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    alien.transform.position
                );

            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }

    void GameOver()
    {
        isDead = true;

        Debug.Log("[C] GAME OVER");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}