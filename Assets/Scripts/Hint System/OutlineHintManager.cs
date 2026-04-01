using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;

public class OutlineHintManager : MonoBehaviour
{
    public static OutlineHintManager Instance { get; private set; }

    [Header("Configuración")]
    public CustomPassVolume customPassVolume;

    [Header("Grosor")]
    public float minWidth = 1f;
    public float maxWidth = 5f;

    [Header("Alpha")]
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;

    [Header("Velocidad")]
    public float pulseSpeed = 2f;

    OutlineFullScreenPass outlinePass;
    List<GameObject> activeObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        outlinePass = customPassVolume.customPasses[0] as OutlineFullScreenPass;
        outlinePass.enabled = true;
    }

    void Update()
    {
        if (activeObjects.Count == 0) return;

        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        // outlinePass.outlineWidth = Mathf.Lerp(minWidth, maxWidth, t);

        Color c = outlinePass.outlineColor;
        c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        outlinePass.outlineColor = c;
    }

    public void Activate(GameObject obj)
    {
        outlinePass.enabled = true;
        if (!activeObjects.Contains(obj))
        {
            activeObjects.Add(obj);
            SetLayerRecursively(obj, LayerMask.NameToLayer("Outline"));
        }
    }

    public void Deactivate(GameObject obj)
    {
        outlinePass.enabled = false;
        activeObjects.Remove(obj);
        SetLayerRecursively(obj, LayerMask.NameToLayer("Default"));
    }

    public void DeactivateAll()
    {
        outlinePass.enabled = false;
        foreach (var obj in activeObjects)
            SetLayerRecursively(obj, LayerMask.NameToLayer("Default"));
        activeObjects.Clear();
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
