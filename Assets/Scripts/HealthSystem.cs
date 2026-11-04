using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int MaxHealth = 100;
    public int Health = 0;
    public bool Player = false;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image DamageBar;
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
        Health = MaxHealth;
    }

    void Update()
    {
        DamageBar.fillAmount = Mathf.Lerp(DamageBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime);
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime*5);
    }
}
