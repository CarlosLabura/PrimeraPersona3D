using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private AnimatedImage[] Images;
    public float AnimationSpeed = 10f;
    private bool Started = false;
    private void Start()
    {
        Started = true;

        for (int i = 0; i < Images.Length; i++)
        {
            AnimatedImage Image = Images[i];
            if (Image.ColorAnimated)
                Image.Image.color = new Color(1,1,1,0);
        }
    }
    private void Update()
    {
        if (!Started) 
            return;

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
    }
}
