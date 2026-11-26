using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MainMenuItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string LevelName;
    public float SelectionTime = 15f;

    [Header("Item Objects")]
    public RawImage BG;
    public RawImage Preview;
    public TextMeshProUGUI Record;

    // Internal Variables
    private float Alpha = 0.5f;
    private float Scale = 1;
    private Vector3 OriginalScale;
    [HideInInspector] public bool Selected = false;
    public static string SelectedLevel = "";

    // Components Needed
    [HideInInspector] public StartAnimation StartAnimation; //cuz im lazy this compoennt is asigned in MenuControl.cs
    public void Load()
    {
        OriginalScale = transform.localScale;

        Color Color = BG.color;
        BG.color = new Color(Color.r, Color.g, Color.b, Alpha);

        Preview.texture = Resources.Load<Texture2D>("Previews/"+LevelName);
    }
    private void Update()
    {
        if (!StartAnimation.Started)
            return;

        if (IsMouseOver(BG))
        {
            SelectedLevel = LevelName;
            Scale = 1.1f;
            Alpha = 1f;
        } else {
            if (SelectedLevel == LevelName)
                SelectedLevel = "";
            Scale = 1f;
            Alpha = 0.5f;
        }

        Color BGcolor = BG.color;
        BG.color = Color.Lerp(BGcolor, new Color(BGcolor.r,BGcolor.g,BGcolor.b,Alpha), Time.deltaTime * SelectionTime);
        transform.localScale = Vector3.Lerp(transform.localScale, OriginalScale * Scale, Time.deltaTime * SelectionTime);
    }
    private bool IsMouseOver(RawImage Image) // Super inteligent code made NOT by me
    {
        if (Mouse.current == null) 
            return false;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Canvas canvas = Image.canvas;
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        return RectTransformUtility.RectangleContainsScreenPoint(Image.rectTransform, mousePos, cam);
    }
}
