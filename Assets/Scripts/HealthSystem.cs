using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviour
{
    // Inspector Variables
    [Header("Health Configs")]
    public bool IsPlayer = false;
    public int MaxHealth = 100;
    [ShowIf("IsPlayer")] public int EnemyDamage = 10;
    [ShowIf("IsPlayer")] public int ProjectileDamage = 20;
    [ShowIf("IsPlayer", true)] public float EnemyDestroyTime = 3f;

    [Header("Objects Needed")]
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image DamageBar;
    [ShowIf("IsPlayer", true)] [SerializeField] private Transform EnemyHealthCanvas;

    [Header("Script Data")]
    [ReadOnly] public int Health = 0;
    [ReadOnly] public bool IsDead = false;

    // Internal Variables
    public Action OnEnemyDeath;
    public Action OnPlayerDeath;
    //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide1005

    // Needed Components 
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    private void Awake()
    {
        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }
    public void Damage(int amount)
    {
        Health -= amount;

        if (Health <= 0 && !IsDead) {
            IsDead = true;
            if (IsPlayer) {
                OnPlayerDeath?.Invoke();
                StarterAssetsInputs.Locked = true;
                // Locked / Not destoyed to keep the scripts running
            } else
            {
                OnEnemyDeath?.Invoke();
                Destroy(gameObject, EnemyDestroyTime);
            }
        }
    }
    private void Start()
    {
        Health = MaxHealth;
    }
    private void Update()
    {
        DamageBar.fillAmount = Mathf.Lerp(DamageBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime*2.5f);
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime*5);

        if (!IsPlayer)
            EnemyHealthCanvas.rotation = Camera.main.transform.rotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            Damage(ProjectileDamage);

        if (IsPlayer && collision.gameObject.CompareTag("Enemy"))
            Damage(EnemyDamage);
    }
}
