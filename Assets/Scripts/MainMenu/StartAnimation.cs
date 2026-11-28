using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[System.Serializable]
public struct AnimatedImage
{
    public RawImage Image;
    public bool PositionAnimated;
    public bool RotationAnimated;
    public bool ColorAnimated;
}
public class StartAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Min(1f)] public float AnimationSpeed = 7.5f;

    [Header("Objects Needed")]
    public RawImage StartText;
    public RawImage BlackScreen;
    public AnimatedImage[] Images;

    //Internal Variables
    public static StartAnimation Instance;
    [HideInInspector] public bool Started = false;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {   
            Debug.Log($"Destoying Script on {name}");
            Destroy(gameObject);
        }
    }
    public void OnShoot(InputValue value)
    {
        if (!Started)
        {
            Started = true;
            StartText.color = new Color(1,1,1,0);
            BlackScreen.color = Color.white;
            Debug.Log("Started Menu");
        }
    }
    private void Start()
    {
        StartText.color = Color.black;
        for (int i = 0; i < Images.Length; i++)
        {
            AnimatedImage Image = Images[i];
            if (Image.ColorAnimated)
                Image.Image.color = new Color(1,1,1,0);
        }
    }
    private void Update()
    {
        if (MenuControl.Instance.Selected != "")
            return;

        if (!Started) {
            StartText.color = Color.Lerp(StartText.color, Color.white, Time.deltaTime * AnimationSpeed);
            return;
        }
        BlackScreen.color = Color.Lerp(BlackScreen.color, new Color(1,1,1,0), Time.deltaTime * AnimationSpeed);
        for (int i = 0; i < Images.Length; i++)
        {
            AnimatedImage Image = Images[i];
            Animate(Image.Image, Image.PositionAnimated, Image.RotationAnimated, Image.ColorAnimated);
        }
    }
    private void Animate(RawImage image, bool position = true, bool rotation = true, bool color = true)
    {
        if (!position && !rotation && !color)
            return;

        float time = Time.deltaTime * AnimationSpeed;

        RectTransform RectTransform = image.rectTransform;
        if (position)
            RectTransform.anchoredPosition = Vector2.Lerp(RectTransform.anchoredPosition, Vector2.zero, time);

        if (rotation)
            RectTransform.localRotation = Quaternion.Lerp(RectTransform.localRotation, Quaternion.identity, time);

        if (color)
            image.color = Color.Lerp(image.color, Color.white, time);

        //Debug.Log($"Animating {image.name}, POS: {position} ROT: {rotation} COL: {color}");
    }
}
