using TMPro;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private TextMeshProUGUI GameOver;
    public int EnemiesLeft = 0;
    void Start()
    {
        EnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    // Update is called once per frame
    void Update()
    {
        Counter.SetText("0/"+EnemiesLeft);

        if (EnemiesLeft <= 0)
        {
            Time.timeScale = 0f;
            GameOver.SetText("Victory!");
        }
    }
}
