
using UnityEngine;
public class ProjectileCollision : MonoBehaviour
{ 
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Projectile Hitted {collision.gameObject.name}");
        Destroy(gameObject);
    }
}