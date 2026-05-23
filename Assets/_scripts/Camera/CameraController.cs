using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _cameraHeight = 12.5f;
    private Vector3 _offSet;
	
    void Start(){
        _offSet = transform.position;
    }
	
    void LateUpdate()
    {
        //camera Position vom Player + Offset
        transform.position = new Vector3(_playerTransform.position.x + _offSet.x,_cameraHeight +_playerTransform.position.y, _playerTransform.position.z + _offSet.z);
    }
}
