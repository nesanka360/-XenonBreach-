using UnityEngine;

public class FireShooter : MonoBehaviour
{
    public GameObject fireProjectilePrefab;

    public Transform firePoint;

    public float shootForce = 15f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (fireProjectilePrefab == null || firePoint == null)
            return;

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
    }
}