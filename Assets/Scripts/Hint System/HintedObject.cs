using UnityEngine;

public class HintedObject : MonoBehaviour
{
    public void Activate()
    {
        OutlineHintManager.Instance.Activate(gameObject);
    }

    public void Deactivate()
    {
        OutlineHintManager.Instance.Deactivate(gameObject);
    }
}
