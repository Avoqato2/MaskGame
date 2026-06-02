using UnityEngine;

public class Projectile : MonoBehaviour
{
   private Vector3 _flightDirection;
   private float _speed;

   public void Init(Vector3 direction, float speed, float destoryTime)
   {
       _flightDirection = direction;
       _speed = speed;
       
       Destroy(gameObject, destoryTime);
   }
    private void Update()
    {
        transform.position += _flightDirection * _speed * Time.deltaTime;
    }
}
