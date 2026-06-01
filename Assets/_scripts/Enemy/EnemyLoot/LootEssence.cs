using UnityEngine;

public class LootEssence : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float _upwardForce = 5f;
    [SerializeField] private float _sideForce = 3f;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        JumpOut();
    }

    private void JumpOut()
    {
        float randomX = Random.Range(-_sideForce, _sideForce);
        float randomZ = Random.Range(-_sideForce, _sideForce);
        float randomUp = Random.Range(_upwardForce * 0.8f, _upwardForce * 1.2f);
        Vector3 jumpForce = new Vector3(randomX, randomUp, randomZ);
        _rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Essenz eingesammelt!");
            
            Destroy(gameObject);
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        _rb.useGravity = false;
        _rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = Quaternion.identity;
    }
    
}