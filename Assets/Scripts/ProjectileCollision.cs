
using UnityEngine;
public class ProjectileCollision : MonoBehaviour
{ 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ray"))
            return;

        Debug.Log($"Projectile Hitted {collision.gameObject.name}");
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ray"))
            return;

        Debug.Log($"Projectile Hitted {other.name}");
        Destroy(gameObject);
    }
}