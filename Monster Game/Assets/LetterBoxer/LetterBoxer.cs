using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBoxer : MonoBehaviour
{    
    public enum ReferenceMode { DesignedAspectRatio, OrginalResolution };

    public Color matteColor = new Color(0, 0, 0, 1);
    public ReferenceMode referenceMode; 
    public float x=16;
    public float y=9;  
    public float width = 960;
    public float height = 540;
    public bool onAwake = true;
    public bool onUpdate = true;

    // --- Added fields for texture background support ---
    public bool useTexture = false;
    public Texture2D matteTexture = null;
    public LayerMask backgroundLayer = 0; // choose the (single) layer that will hold the background sprite

    private Camera cam;
    private Camera letterBoxerCamera;
    private GameObject backgroundObject;
    private Sprite backgroundSprite;

    public void Awake()
    {
        // store reference to the camera
        cam = GetComponent<Camera>();

        // add the letterboxing camera
        AddLetterBoxingCamera();

        // perform sizing if onAwake is set
        if (onAwake)
        {
            PerformSizing();
        }
    }

    public void Update()
    {
        // perform sizing if onUpdate is set
        if (onUpdate)
        {
            PerformSizing();
        }
    }

    private void OnValidate()
    {
        x = Mathf.Max(1, x);
        y = Mathf.Max(1, y);
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
    }

    private void AddLetterBoxingCamera()
    {
        // check that we don't have a camera already at -100 (lowest depth) which will cause issues
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera camera in allCameras)
        {             
            if (camera.depth == -100)
            {
                Debug.LogError("Found " + camera.name + " with a depth of -100. Will cause letter boxing issues. Please increase it's depth.");
            }
        }

        // create a camera to render background used for matte bars
        letterBoxerCamera = new GameObject().AddComponent<Camera>();
        letterBoxerCamera.farClipPlane = 1;
        letterBoxerCamera.useOcclusionCulling = false;
        letterBoxerCamera.allowHDR = false;
        letterBoxerCamera.allowMSAA = false;
        letterBoxerCamera.depth = -100;
        letterBoxerCamera.name = "Letter Boxer Camera";

        // Move the camera to a far, non-playable position and hide it from the Hierarchy so players can't find it.
        // Large negative Y is unlikely to be reachable; hideFlags keeps it out of normal editing.
        letterBoxerCamera.transform.position = new Vector3(0f, -100000f, 0f);
        letterBoxerCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;

        // If using a texture, create a background sprite and configure culling only to the selected background layer.
        if (useTexture && matteTexture != null && backgroundLayer.value != 0)
        {
            // set clear flags (still clear color to avoid garbage if sprite has transparent regions)
            letterBoxerCamera.clearFlags = CameraClearFlags.Color;
            letterBoxerCamera.backgroundColor = Color.black;

            // set camera to render only the chosen background layer
            letterBoxerCamera.cullingMask = backgroundLayer.value;

            // create background GameObject (sprite) as child of the letterbox camera
            // pick the first selected layer index from the LayerMask
            int layerIndex = 0;
            int mask = backgroundLayer.value;
            for (int i = 0; i < 32; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    layerIndex = i;
                    break;
                }
            }

            // destroy previous background if exists
            if (backgroundObject != null)
            {
                DestroyImmediate(backgroundObject);
            }

            backgroundObject = new GameObject("LetterBox Background");
            backgroundObject.layer = layerIndex;
            backgroundObject.transform.SetParent(letterBoxerCamera.transform, false);

            // hide background GameObject from Hierarchy as well
            backgroundObject.hideFlags = HideFlags.HideAndDontSave;

            // position the sprite just in front of the camera near clip plane (local Z)
            float distance = letterBoxerCamera.nearClipPlane + 0.01f;
            backgroundObject.transform.localPosition = new Vector3(0f, 0f, distance);
            backgroundObject.transform.localRotation = Quaternion.identity;

            // create sprite and sprite renderer
            backgroundSprite = Sprite.Create(matteTexture, new Rect(0, 0, matteTexture.width, matteTexture.height), new Vector2(0.5f, 0.5f), 100f);
            var sr = backgroundObject.AddComponent<SpriteRenderer>();
            sr.sprite = backgroundSprite;

            // use an unlit material so the background isn't affected by scene lights
            Material unlit = new Material(Shader.Find("Unlit/Texture"));
            if (unlit != null)
                sr.sharedMaterial = unlit;

            // initial size update
            UpdateBackgroundScale();
        }
        else
        {
            // default (color matte) behavior
            letterBoxerCamera.cullingMask = 0;
            letterBoxerCamera.backgroundColor = matteColor;
            letterBoxerCamera.clearFlags = CameraClearFlags.Color;

            // remove any previous background object added for texture mode
            if (backgroundObject != null)
            {
                DestroyImmediate(backgroundObject);
                backgroundObject = null;
                backgroundSprite = null;
            }
        }
    }

    // Update the background sprite scale to match the current screen aspect
    private void UpdateBackgroundScale()
    {
        if (backgroundObject == null || backgroundSprite == null || letterBoxerCamera == null)
            return;

        // compute world size at the sprite distance
        float d = letterBoxerCamera.nearClipPlane + 0.01f;
        float fovRad = letterBoxerCamera.fieldOfView * Mathf.Deg2Rad;
        float worldHeight = 2.0f * d * Mathf.Tan(fovRad * 0.5f);

        // use current screen aspect to compute world width (this ensures horizontal stretching on wide screens)
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float worldWidth = worldHeight * windowaspect;

        float spriteWorldWidth = (backgroundSprite.rect.width / backgroundSprite.pixelsPerUnit);
        float spriteWorldHeight = (backgroundSprite.rect.height / backgroundSprite.pixelsPerUnit);

        float scaleX = worldWidth / spriteWorldWidth;
        float scaleY = worldHeight / spriteWorldHeight;

        backgroundObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    // based on logic here from http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
    private void PerformSizing()
    {
        // calc based on aspect ratio
        float targetRatio = x / y;

        // recalc if using resolution as reference
        if (referenceMode == LetterBoxer.ReferenceMode.OrginalResolution)
        {
            targetRatio = width / height;
        }

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetRatio;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            cam.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = cam.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }

        // ensure background sprite matches current screen aspect / camera parameters
        if (useTexture && matteTexture != null && backgroundObject != null)
        {
            UpdateBackgroundScale();
        }
    }
}
