using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Outline))]
public class RayColor : MonoBehaviour
{
    [Min(0.1f)] public float DissappearTime = 2;

    // Components Needed
    private MeshRenderer MeshRenderer;
    private Outline Outline;
    
    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        Outline = GetComponent<Outline>();
    }
    private void Start()
    {
        SetAlpha(0);
    }
    public float GetAlpha()
    {
        return MeshRenderer.material.color.a;
    }
    public void SetAlpha(float Alpha)
    {
        Color ObjectColor = MeshRenderer.material.color;
        MeshRenderer.material.color = new Color(ObjectColor.r, ObjectColor.g, ObjectColor.b, Alpha);

        Color OutlineColor = Outline.OutlineColor;
        Outline.OutlineColor = new Color(OutlineColor.r, OutlineColor.g, OutlineColor.b, Alpha);
    }
    private void Update()
    {
        if (GetAlpha() > 0)
            SetAlpha(GetAlpha()-Time.deltaTime*DissappearTime);
        
        if (GetAlpha() < 0)
            SetAlpha(0);
    }
}
