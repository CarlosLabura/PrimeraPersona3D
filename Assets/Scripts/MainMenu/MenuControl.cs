using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuControl : MonoBehaviour
{
    [Header("Menu Data")]
    [SerializeField] private TextAsset LevelInfo;
    public float TransitionSpeed = 0.75f;
    public Vector3 ItemStartPosition;
    public Vector3 ItemStartOffset;

    [Header("Objects Needed")]
    [SerializeField] private RawImage Background;
    [SerializeField] private GameObject MenuPrefab;
    [SerializeField] private Transform MenuContainer;
    [SerializeField] private RawImage BlackScreen;

    // Internal Variables
    private const string LevelPath = "Scenes/";
    private string[] Levels;
    public static string Selected = "";
    public static bool Loaded = false;

    // Components Needed
    private void Start()
    {
        Levels = LevelInfo.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < Levels.Length; i++)
        {
            GameObject Object = Instantiate(MenuPrefab, MenuContainer, false);
            Object.GetComponent<RectTransform>().anchoredPosition3D = ItemStartPosition + ItemStartOffset * i;

            MainMenuItem MainMenuItem = Object.GetComponent<MainMenuItem>();
            MainMenuItem.LevelName = Levels[i];

            MainMenuItem.Load();
        }
    }
    public void OnShoot(InputValue value)
    {
        if (Selected != "")
            return;

        if (MainMenuItem.SelectedLevel != "") {
            BlackScreen.color = new Color(0,0,0,0);
            Selected = MainMenuItem.SelectedLevel;
        }
    }
    private void Update()
    {
        if (!StartAnimation.Started)
            return;
        
        if (Selected == "")
            return;

        if (!Loaded)
        {
            BlackScreen.color = Color.Lerp(BlackScreen.color, Color.black, Time.deltaTime * TransitionSpeed);
            if (BlackScreen.color.a >= 0.99){
                Loaded = true;
                SceneManager.LoadScene(LevelPath+Selected+"/Scene");
            }
        }
        
    }
}
