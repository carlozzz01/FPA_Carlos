using UnityEngine;

public class ReactionAnimation : Reaction
{
    [Header("Animation Configuration")]
    [SerializeField] private Animator _targetAnimator;
    [HideInInspector][SerializeField] private string _triggerKey;
    [HideInInspector][SerializeField] private string _boolKey;
    [HideInInspector][SerializeField] private bool _boolValue;
    [HideInInspector][SerializeField] private string _floatKey;
    [HideInInspector][SerializeField] private float _floatValue;
    [SerializeField] private AnimationReactionType _reactionType;

    public string TriggerKey => _triggerKey;
    public string BoolKey => _boolKey;
    public bool BoolValue => _boolValue;
    public string FloatKey => _floatKey;
    public float FloatValue => _floatValue;
    public AnimationReactionType ReactionType => _reactionType;

    public enum AnimationReactionType
    {
        Trigger,
        Bool,
        Float
    }

    // void OnValidate()
    // {
    //     if (_reactionType == AnimationReactionType.Bool)
    //     {
    //         _floatValue = Mathf.FloorToInt(_floatValue);
    //     }
    // }

    protected override void React()
    {
        switch (_reactionType)
        {
            case AnimationReactionType.Trigger:

                _targetAnimator.SetTrigger(_triggerKey);

                break;

            case AnimationReactionType.Bool:

                _targetAnimator.SetBool(_boolKey, _boolValue);

                break;

            case AnimationReactionType.Float:

                _targetAnimator.SetFloat(_floatKey, _floatValue);

                break;

            default:
                break;
        }
    }

    public void SetTriggerKey(string key)
    {
        _triggerKey = key;
    }

    public void SetBoolKey(string key)
    {
        _boolKey = key;
    }

    public void SetBoolValue(bool value)
    {
        _boolValue = value;
    }

    public void SetFloatKey(string key)
    {
        _floatKey = key;
    }

    public void SetFloatValue(float value)
    {
        _floatValue = value;
    }
}
