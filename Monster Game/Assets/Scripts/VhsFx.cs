using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class VhsFx : MonoBehaviour
{
    public Shader shader;
    [Range(0f, 1f)] public float intensity = 1f;

    private Material _material;
    private Material Material
    {
        get
        {
            if (_material == null && shader != null)
            {
                _material = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
            }
            return _material;
        }
    }

    void OnDestroy()
    {
        if (_material != null)
            DestroyImmediate(_material);
    }

    // Viewport-agnostic OnRenderImage: use src dimensions/format (avoids Screen.* or Camera.rect)
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        var mat = Material;
        if (mat == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        // Use the incoming source RT size/format so LetterBoxer camera.rect does not break things
        int width = src.width;
        int height = src.height;
        RenderTextureFormat format = src.format;

        // Acquire temporary RT(s) with exact dimensions taken from src
        RenderTexture tmp = RenderTexture.GetTemporary(width, height, 0, format);
        tmp.filterMode = FilterMode.Bilinear;

        // Set shader properties (example: intensity). Adjust to match your shader property names.
        if (mat.HasProperty("_Intensity"))
            mat.SetFloat("_Intensity", intensity);

        // Perform effect pass(es). Use src -> tmp -> dest to avoid writing into the source.
        Graphics.Blit(src, tmp, mat);
        Graphics.Blit(tmp, dest);

        RenderTexture.ReleaseTemporary(tmp);
    }
}