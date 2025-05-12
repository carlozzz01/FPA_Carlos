using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIKController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private ChainIKConstraint _rHandIK;
    [SerializeField] private float _ikAnimationTime = 1f;
    [SerializeField] private AnimationCurve _ikAnimationCurve;
    [SerializeField] private Transform _rHandIKTarget;
    private float _ikCoroutineTimer;
    private Coroutine _ikAnimationCoroutine;

    /// <summary>
    /// Matches the Right Hand's IK Target to given Transform for a brief period of time
    /// </summary>
    /// <param name="ikTarget"></param>
    public void StartIKAnimation(Transform ikTarget)
    {
        _rHandIKTarget.SetParent(ikTarget);
        _rHandIKTarget.localPosition = Vector3.zero;
        _rHandIKTarget.rotation = Quaternion.Euler(Vector3.zero);

        if (_ikAnimationCoroutine != null)
        {
            StopCoroutine(_ikAnimationCoroutine);
        }

        _ikAnimationCoroutine = StartCoroutine(LerpIKWeight());
    }

    /// <summary>
    /// Coroutine that interpolates the IK's weight following an Animation Curve
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpIKWeight()
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
