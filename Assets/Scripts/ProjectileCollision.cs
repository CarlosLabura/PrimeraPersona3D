
using UnityEngine;
public class ProjectileCollision : MonoBehaviour
{ 
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}