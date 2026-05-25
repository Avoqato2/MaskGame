using UnityEngine;
using System;
[Serializable]
public class GroundController
{
    // I stole this shit i am to lazy to explain it
    // actually i can not explain it pls watch YouTube Video on Ground detection.
    public float _groundDistanceTolerance = 0.1f;
    
    public LayerMask _groundLayerMask;
    
    private CapsuleCollider _capsuleCollider;
    
    private Transform _transform;
    
    public bool IsGrounded { get; private set; }

    public float? DistanceToGround { get; private set; }

    public void Init(CapsuleCollider capsuleCollider, Transform transform)
    {
        _capsuleCollider = capsuleCollider;
        _transform = transform;
    }

    public void CheckGround()
    {
        float sphereCastRadius = _capsuleCollider.radius - 0.1f;
        Vector3 sphereCastOrigin = _transform.position +  new Vector3(0, _capsuleCollider.radius, 0);

        bool isGroundBelow = Physics.SphereCast(
            sphereCastOrigin,
            sphereCastRadius, 
            Vector3.down, 
            out RaycastHit hitinfo, 
            1000, 
            _groundLayerMask, 
            QueryTriggerInteraction.Ignore);

        if (isGroundBelow)
        {
            DistanceToGround = _transform.position.y - hitinfo.point.y;
        }
        else
        {
            DistanceToGround = null;
        }
        
        IsGrounded = isGroundBelow && DistanceToGround <= _groundDistanceTolerance;
    }
}
