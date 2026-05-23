using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private GameObject _player;
    [SerializeField] private float _cameraHeight = 12.5f;
    private Vector3 _offSet;
	
    void Start(){
        _offSet = transform.position;
    }
	
    void LateUpdate()
    {
        //camera Position vom Player + Offset
        transform.position = new Vector3(_player.transform.position.x + _offSet.x,_cameraHeight +_player.transform.position.y, _player.transform.position.z + _offSet.z);
    }
}
