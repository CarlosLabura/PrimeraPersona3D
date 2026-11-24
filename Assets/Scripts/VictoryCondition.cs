using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { Active, GameOver, Victory, Pause }
public class VictoryCondition : MonoBehaviour
{
    [Header("Objects Needed")]
    public GameState State = GameState.Active;
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private TextMeshProUGUI GameOver;

    [Header("Script Data")]
    [ReadOnly] public int EnemiesLeft = 0;

    // Internal Variables
    private int MaxEnemies = 0;

    // Needed Components 
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    private HealthSystem HealthSystem;
    private void Awake()
    {
        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        HealthSystem = GetComponent<HealthSystem>();
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void PlayerWin()
    {
        State = GameState.Victory;
        GameOver.SetText("Victory!");
        StarterAssetsInputs.Locked = true;
        //Time.timeScale = 0f;
    }
    private void PlayerDeath()
    {
        State = GameState.GameOver;
        GameOver.SetText("Game Over");
        // Movement is already locked in HealthSystem.cs
        Time.timeScale = 0f;
    }
    private void EnemyDeath()
    {
        EnemiesLeft -= 1;
        Counter.SetText(EnemiesLeft+"/"+MaxEnemies);

        if (EnemiesLeft <= 0)
            PlayerWin();
    }
    private void Start()
    {
        HealthSystem.OnEnemyDeath += EnemyDeath;
        HealthSystem.OnPlayerDeath += PlayerDeath;

        MaxEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemiesLeft = MaxEnemies;

        Counter.SetText(EnemiesLeft+"/"+MaxEnemies);
    }
}
