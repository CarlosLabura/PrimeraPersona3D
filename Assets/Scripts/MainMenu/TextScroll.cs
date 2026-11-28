using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(RawImage))]
public class TextScroll : MonoBehaviour
{
    public Vector2 Scroll;

    // Components Needed
    private RawImage RawImage;

    private void Awake()
    {
        RawImage = GetComponent<RawImage>();
    }
    private void Update()
    {
        Rect uv = RawImage.uvRect;
        uv.position += Scroll * Time.deltaTime;
        RawImage.uvRect = uv;
    }
}
