using UnityEngine;

public class InspectorItem : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private bool _hasText;

    public string ID => _id;
    public bool HasText => _hasText;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
