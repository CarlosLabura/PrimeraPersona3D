using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int MaxHealth = 100;
    public int Health = 0;
    public bool Player = false;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image DamageBar;
    [SerializeField] private TextMeshProUGUI GameOver;
    [SerializeField] private Victory Victory;
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    [SerializeField] private Animator Animator;
    public bool Dead = false;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            Damage(collision.gameObject.GetComponent<Projectile>().Damage);

        if (Player && collision.gameObject.CompareTag("Enemy"))
            Damage(10);
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Damage(int amount)
    {
        Health -= amount;

        if (Health <= 0 && !Dead) {
            Dead = true;
            if (Player) {
                GameOver.SetText("Game Over");
                Time.timeScale = 0f;
                StarterAssetsInputs.Locked = true;
            } else
            {
                Animator.SetBool("Dead", true);
                Destroy(gameObject, 5f);
                Victory.UpdateCounter();
            }
        }
    }
    void Start()
    {
        Health = MaxHealth;
        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }
    void Update()
    {
        DamageBar.fillAmount = Mathf.Lerp(DamageBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime);
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime*5);
    }
}
