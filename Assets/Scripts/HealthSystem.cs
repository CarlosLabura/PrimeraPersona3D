using System;
using UnityEngine;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviour
{
    // Inspector Variables
    [Header("Health Configs")]
    public bool IsPlayer = false;
    [Min(0)] public int MaxHealth = 100;
    [ShowIf("IsPlayer")] [Min(0)] public int EnemyDamage = 10;
    [ShowIf("IsPlayer")] [Min(0)] public int ProjectileDamage = 20;
    [ShowIf("IsPlayer")] [Min(0)] public int RayDamage = 100;
    [ShowIf("IsPlayer", true)] [Min(0.1f)] public float EnemyDestroyTime = 3f;

    [Header("Objects Needed")]
    public Image HealthBar;
    public Image DamageBar;
    [ShowIf("IsPlayer", true)] public Transform EnemyHealthCanvas;

    [Header("Script Data")]
    [ReadOnly] public int Health = 0;

    // Internal Variables
    [HideInInspector] public bool IsDead = false;
    public Action OnEnemyDeath;     // Runs only if IsPlayer = false
    public Action OnPlayerDeath;    // Runs only if IsPlayer = true
    public static Action OnAnyEnemyDeath; //Runs everytime some enemy die
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
        Debug.Log($"{name}: Damaged by: {amount}");

        if (Health <= 0 && !IsDead) {
            IsDead = true;
            if (IsPlayer) {
                OnPlayerDeath?.Invoke();
                StarterAssetsInputs.Locked = true;
                // Locked / Not destoyed to keep the scripts running
            } else {
                OnEnemyDeath?.Invoke();
                OnAnyEnemyDeath?.Invoke();
                Destroy(gameObject, EnemyDestroyTime);
            }
            Debug.Log($"{name} died at Position: X: {transform.position.x} Y: {transform.position.y} Z: {transform.position.z}");
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
    private void OnTriggerStay(Collider other)
    {
        if (!IsPlayer && other.CompareTag("Ray") && other.GetComponent<RayColor>().GetAlpha() > 0.5)
            Damage(RayDamage);
    }
}
