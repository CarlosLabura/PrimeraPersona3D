using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuItem : MonoBehaviour
{
    public string LevelName;
    public string LevelPath;
    public RawImage BG;
    public RawImage Preview;
    public TextMeshProUGUI Record;

    public void Load()
    {
        Color Color = BG.color;
        BG.color = new Color(Color.r, Color.g, Color.b, 0.5f);

        Preview.texture = Resources.Load<Texture2D>("Previews/"+LevelName);
    }
    private void Update()
    {
        
    }
}
