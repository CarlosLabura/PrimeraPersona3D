using TMPro;
using UnityEngine;
public class Victory : MonoBehaviour
{
    [Header("Objects Needed")]
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private TextMeshProUGUI GameOver;

    [Header("Script Data")]
    [ReadOnly] public int EnemiesLeft = 0;

    // Internal Variables
    private int MaxEnemies = 0;

    // Needed Components 
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    private void Awake()
    {
        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }
    private void Start()
    {
        MaxEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        EnemiesLeft = MaxEnemies;
        Counter.SetText(EnemiesLeft+"/"+MaxEnemies);
    }
    public void UpdateCounter()
    {
        EnemiesLeft -= 1;
        Counter.SetText(EnemiesLeft+"/"+MaxEnemies);

        if (EnemiesLeft <= 0)
        {
            GameOver.SetText("Victory!");
            Time.timeScale = 0f;
            StarterAssetsInputs.Locked = true;
        }
    }
}
