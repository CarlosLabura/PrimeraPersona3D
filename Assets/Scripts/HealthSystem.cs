using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int Health = 100;
    public bool Player = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) {
            Health -= collision.gameObject.GetComponent<Projectile>().Damage;

            if (Health <= 0)
                Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
