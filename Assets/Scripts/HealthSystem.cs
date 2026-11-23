using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviour
{
    // Inspector Variables
    [Header("Health Configs")]
    public bool IsPlayer = false;
    public int MaxHealth = 100;
    [Space(2)]
    public float EnemyDestroyTime = 3f;
    public int EnemyDamage = 10;

    [Header("Objects Needed")]
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image DamageBar;
    [SerializeField] private Victory VictoryScript;
    [Space(2)]
    [SerializeField] private TextMeshProUGUI GameOverText;
    [Space(2)]
    [SerializeField] private Transform EnemyHealthCanvas;
    [SerializeField] private Animator EnemyModelAnimator;

    [Header("Script Data")]
    [ReadOnly] public int Health = 0;
    [ReadOnly]  public bool Dead = false;

    // Needed Components 
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    private void Awake()
    {
        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }
    //https://github.com/microsoft/Microsoft.Unity.Analyzers/blob/main/doc/UNT0041.md
    private static readonly int IsDead = Animator.StringToHash("Dead");
    public void Damage(int amount)
    {
        Health -= amount;

        if (Health <= 0 && !Dead) {
            Dead = true;
            if (IsPlayer) {
                GameOverText.SetText("Game Over");
                StarterAssetsInputs.Locked = true;
                Time.timeScale = 0f;
            } else
            {
                EnemyModelAnimator.SetBool(IsDead, true);
                VictoryScript.UpdateCounter();
                Destroy(gameObject, EnemyDestroyTime);
            }
        }
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Start()
    {
        Health = MaxHealth;
    }
    private void Update()
    {
        DamageBar.fillAmount = Mathf.Lerp(DamageBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime);
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (float) Health/MaxHealth, Time.deltaTime*5);
        if (!IsPlayer)
            EnemyHealthCanvas.rotation = Camera.main.transform.rotation;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            Damage(collision.gameObject.GetComponent<Projectile>().Damage);

        if (IsPlayer && collision.gameObject.CompareTag("Enemy"))
            Damage(EnemyDamage);
    }
}
