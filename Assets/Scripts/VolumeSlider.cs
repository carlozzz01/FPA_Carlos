using UnityEngine;
using UnityEngine.UI;
using Managers;

public class VolumeSlider : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string audioKey = "musicVolume";
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        if (_slider ==  null) _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        UpdateSliderValue();
    }

    /// <summary>
    /// Updates the slider value to match the value from the player prefs key
    /// </summary>
    public void UpdateSliderValue()
    {
        Debug.Log($"This slider value \"{audioKey}\" has value of {DataManager.Instance.GetFloat(audioKey)}");

        _slider.SetValueWithoutNotify(DataManager.Instance.GetFloat(audioKey));
    }
}
