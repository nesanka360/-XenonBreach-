using UnityEngine;

public class FireShooter : MonoBehaviour
{
    public GameObject fireProjectilePrefab;

    public Transform firePoint;

    public float shootForce = 15f;

    public float cooldownTime = 10f;

    private bool canShoot = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        canShoot = false;

        if (fireProjectilePrefab == null || firePoint == null)
        {
            Debug.LogWarning("[C] FireShooter missing prefab or fire point");

            canShoot = true;

            return;
        }

        GameObject projectile =
            Instantiate(
                fireProjectilePrefab,
                firePoint.position,
                firePoint.rotation
            );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(
                firePoint.forward * shootForce,
                ForceMode.Impulse
            );
        }

        Destroy(projectile, 6f);

        Debug.Log("[C] Flamethrower fired");

        Invoke(nameof(ResetShoot), cooldownTime);
    }

    void ResetShoot()
    {
        canShoot = true;

        Debug.Log("[C] Flamethrower ready again");
    }
}