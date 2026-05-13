using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private Vector3 OffSet;
    public Transform player;
	
    void Start(){
        OffSet = transform.position;
    }
	
    void LateUpdate()
    {
        //camera	Position vom Player + Offset
        transform.position = new Vector3(Player.transform.position.x + OffSet.x,12.5f, Player.transform.position.z + OffSet.z);
    }
}
