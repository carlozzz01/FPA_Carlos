using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Check Area", menuName = "Reaction System/Decisions/Check Area")]
public class CheckAreaReactionDecision : ReactionDecision
{
    [SerializeField] private Vector3 _checkPoint;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _checkLayers;
    [SerializeField] private string _itemID;
    [SerializeField] private int _amountNeeded;

    private Collider[] _hits;

    public override bool CheckDecision()
    {
        if (Physics.OverlapSphereNonAlloc(_checkPoint, _checkRadius, _hits, _checkLayers) >= _amountNeeded)
        {
            Debug.Log(_hits.Length);

            bool matchingIDs = true;

            foreach (Collider hit in _hits)
            {
                if (hit.TryGetComponent(out PoolEntity entity))
                {
                    if (entity.ID != _itemID)
                    {
                        Debug.Log("ID missmatch");

                        matchingIDs = false;
                    }
                }
                else
                {
                    Debug.Log("Type missmatch");

                    matchingIDs = false;
                }
            }

            Debug.Log($"result {matchingIDs}");

            return matchingIDs;
        }
        else
        {
            Debug.Log("No sufficient amount found");

            return false;
        }
    }
}
