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
        _hits = new Collider[10];

        if (Physics.OverlapSphereNonAlloc(_checkPoint, _checkRadius, _hits, _checkLayers) >= _amountNeeded)
        {
            int amountFound = 0;

            foreach (Collider hit in _hits)
            {
                if (hit == null) continue;

                if (hit.TryGetComponent(out PoolEntity entity))
                {
                    if (entity.ID == _itemID)
                    {
                        Debug.Log(_itemID);
                        amountFound++;
                    }
                }
            }

            Debug.Log($"{_itemID} found?: {amountFound >= _amountNeeded}");
            return amountFound >= _amountNeeded;
        }
        else
        {
            return false;
        }
    }
}
