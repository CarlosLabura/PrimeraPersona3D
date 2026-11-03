
using UnityEngine;


public class Projectile : MonoBehaviour
{ 
    public int Damage = 20;
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}