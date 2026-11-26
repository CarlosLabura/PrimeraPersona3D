using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(StartAnimation))]
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
    [HideInInspector] public string Selected = "";
    [HideInInspector] public bool Loaded = false;
    private static readonly string LevelPath = "Scenes/";
    private string[] Levels;

    // Components Needed
    private StartAnimation StartAnimation;
    private void Awake()
    {
        StartAnimation = GetComponent<StartAnimation>();
    }
    private void Start()
    {
        Levels = LevelInfo.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < Levels.Length; i++) //Load Menu Items
        {
            GameObject Object = Instantiate(MenuPrefab, MenuContainer, false);
            Object.GetComponent<RectTransform>().anchoredPosition3D = ItemStartPosition + ItemStartOffset * i;

            MainMenuItem MainMenuItem = Object.GetComponent<MainMenuItem>();
            MainMenuItem.LevelName = Levels[i];
            MainMenuItem.StartAnimation = StartAnimation; // why did yall let him cook

            MainMenuItem.Load();
        }
        Background.texture = Resources.Load<Texture2D>("Previews/"+Levels[0]);
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
        if (!StartAnimation.Started) //Below: Menu Started
            return;

        if (Background.texture is Texture2D Texture2D)
            if (Background.texture.name != MainMenuItem.SelectedLevel && MainMenuItem.SelectedLevel != "") {
                Texture2D = Resources.Load<Texture2D>("Previews/"+MainMenuItem.SelectedLevel);
                if (Texture2D != null)
                    Background.texture = Texture2D;
            }
        
        if (Selected == "") //Below: Selected Something
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
