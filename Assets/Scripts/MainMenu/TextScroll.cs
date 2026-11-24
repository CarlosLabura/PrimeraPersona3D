using UnityEngine;
using UnityEngine.UI;
public class TextScroll : MonoBehaviour
{
    public Vector2 Scroll;
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
