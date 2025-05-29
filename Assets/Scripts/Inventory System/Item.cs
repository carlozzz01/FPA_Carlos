using UnityEngine;

public class Item
{
    [SerializeField] private string _id;
    [TextArea][SerializeField] private string _description;

    public string ID => _id;
}
