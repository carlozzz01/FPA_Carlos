using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIKController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player _player;

    [Header("Configuration")]
    [SerializeField] private ChainIKConstraint _rHandIK;
    [SerializeField] private float _ikAnimationTime = 1f;
    [SerializeField] private AnimationCurve _ikAnimationCurve;
    [SerializeField] private Transform _rHandIKTarget;
    private float _ikCoroutineTimer;
    private Coroutine _ikAnimationCoroutine;

    private void OnEnable()
    {
        _player.OnInteractionStarted += StartIKAnimation;
    }

    private void OnDisable()
    {
        _player.OnInteractionStarted -= StartIKAnimation;
    }

    /// <summary>
    /// Matches the Right Hand's IK Target to given Transform for a brief period of time
    /// </summary>
    /// <param name="ikTarget"></param>
    public void StartIKAnimation(Interactable interactable)
    {
        ReactiveInteractable reactiveInteractable = interactable as ReactiveInteractable;

        // try
        // {
        //     reactiveInteractable = interactable as ReactiveInteractable;
        // }
        // catch (System.Exception)
        // {
        //     return;
        // }

        if (reactiveInteractable == null) return;

        if (reactiveInteractable.Handle == null || reactiveInteractable.InteractOnTriggerEnter) return;

        _rHandIKTarget.SetParent(reactiveInteractable.Handle);
        _rHandIKTarget.localPosition = Vector3.zero;
        _rHandIKTarget.rotation = Quaternion.Euler(Vector3.zero);

        if (_ikAnimationCoroutine != null)
        {
            StopCoroutine(_ikAnimationCoroutine);
        }

        _ikAnimationCoroutine = StartCoroutine(AnimateIKWeight());
    }

    /// <summary>
    /// Coroutine that interpolates the IK's weight following an Animation Curve
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateIKWeight()
    {
        _ikCoroutineTimer = 0f;

        while (_ikCoroutineTimer < _ikAnimationTime)
        {
            _rHandIK.weight = _ikAnimationCurve.Evaluate(_ikCoroutineTimer / _ikAnimationTime);

            _ikCoroutineTimer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

}
