using UnityEngine;

public class GroundController : MonoBehaviour
{
    // I stole this shit i am to lazy to explain it
    // actually i can not explain it pls watch YouTube Video on Ground detection.
    [SerializeField]
    private float _groundDistanceTolerance;
    
    [SerializeField]
    private LayerMask _groundLayerMask;
    
    private CapsuleCollider _capsuleCollider;
    
    public bool IsGrounded { get; private set; }

    public float? DistanceToGround { get; private set; }

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        float spherCastRadius = _capsuleCollider.radius - 0.1f;
        Vector3 spherCastOrigin = transform.position +  new Vector3(0, _capsuleCollider.radius, 0);

        bool isGroundBelow = Physics.SphereCast(
            spherCastOrigin,
            spherCastRadius, 
            Vector3.down, 
            out RaycastHit hitinfo, 
            1000, 
            _groundLayerMask, 
            QueryTriggerInteraction.Ignore);

        if (isGroundBelow)
        {
            DistanceToGround = transform.position.y - hitinfo.point.y;
        }
        else
        {
            DistanceToGround = null;
        }
        
        IsGrounded = isGroundBelow && DistanceToGround <= _groundDistanceTolerance;
    }
}
