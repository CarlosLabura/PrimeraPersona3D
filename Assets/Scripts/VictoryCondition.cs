using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState { Active, GameOver, Victory, Pause, Counting }
public class VictoryCondition : MonoBehaviour
{
    public GameState State = GameState.Counting;

    [Header("Objects Needed")]
    [SerializeField] private TextMeshProUGUI Timer;
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private TextMeshProUGUI GameOver;

    [Header("Script Data")]
    [ReadOnly] public int EnemiesLeft = 0;
    [ReadOnly] public float TimerTime = 0;

    // Internal Variables
    public static VictoryCondition Instance;
    private int MaxEnemies = 0;

    // Needed Components 
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    private HealthSystem HealthSystem;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {   
            Debug.Log($"Destoying Script on {name}");
            Destroy(gameObject);
        }

        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        HealthSystem = GetComponent<HealthSystem>();
    }
    public void Restart()
    {
        Debug.Log("Restarting Scene");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void StartCount()
    {
        Debug.Log("Starting Count");
        State = GameState.Counting;
        StarterAssetsInputs.Locked = true;
        StartCoroutine(Count());
    }
    private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);
    private IEnumerator Count()
    {
        yield return _waitForSeconds1;
        GameOver.SetText("3");
        yield return _waitForSeconds1;
        GameOver.SetText("2");
        yield return _waitForSeconds1;
        GameOver.SetText("1");
        yield return _waitForSeconds1;
        CountFinished();
    }
    private void CountFinished()
    {
        Debug.Log("Counting finished");
        GameOver.SetText("");
        State = GameState.Active;
        StarterAssetsInputs.Locked = false;
    }
    private void PlayerWin()
    {
        if (TimerTime > PlayerPrefs.GetFloat(GameData.CurrentLevel, 0f)){
            PlayerPrefs.SetFloat(GameData.CurrentLevel, TimerTime);
            PlayerPrefs.Save();
        }

        State = GameState.Victory;
        GameOver.SetText("Victory!");
        StarterAssetsInputs.Locked = true;
        //Time.timeScale = 0f;
        Debug.Log("Player Won");
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
        EnemiesLeft--;
        UpdateCounter();

        if (EnemiesLeft <= 0)
            PlayerWin();
    }
    public void UpdateCounter()
    {
        Counter.SetText(EnemiesLeft+"/"+MaxEnemies);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        HealthSystem.OnAnyEnemyDeath += EnemyDeath; // this only runs with player health logic, so u must use the static method
        HealthSystem.OnPlayerDeath += PlayerDeath;

        MaxEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemiesLeft = MaxEnemies;

        UpdateCounter();

        StartCount();
    }
    public static string ParseTime(float Time)
    {
        int m = Mathf.FloorToInt(Time / 60);
        int s = Mathf.FloorToInt(Time % 60);
        int ms = Mathf.FloorToInt((Time * 1000) % 1000);

        return $"{m:00}:{s:00}:{ms:000}";
    }
    private void Update()
    {
        if (State != GameState.Active)
            return;

        TimerTime += Time.deltaTime;
        Timer.text = ParseTime(TimerTime);
    }
    private void OnDestroy()
    {
        HealthSystem.OnAnyEnemyDeath -= EnemyDeath;
        HealthSystem.OnPlayerDeath -= PlayerDeath;
    }
}
