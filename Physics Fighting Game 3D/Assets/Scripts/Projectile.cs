using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            rb.isKinematic = true; // Disable physics interactions
        }

        Destroy(gameObject);
    }
}
