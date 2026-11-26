
using UnityEngine;
public class ProjectileCollision : MonoBehaviour
{ 
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Projectile Hitted {collision.gameObject.name}");
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Projectile Hitted {other.name}");
        Destroy(gameObject);
    }
}