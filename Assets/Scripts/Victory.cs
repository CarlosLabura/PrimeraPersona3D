using TMPro;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private TextMeshProUGUI GameOver;
    public int EnemiesLeft = 0;
    private int MaxEnemies = 0;
    private StarterAssets.StarterAssetsInputs StarterAssetsInputs;
    void Start()
    {
        StarterAssetsInputs = GetComponent<StarterAssets.StarterAssetsInputs>();
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
