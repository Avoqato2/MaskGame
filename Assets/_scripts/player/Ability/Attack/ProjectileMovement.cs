using System;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
   private Vector3 _flightDirection;
   private float _speed;
   private float _demage;

   public void Init(Vector3 direction, float speed, float demage, float destoryTime)
   {
       _flightDirection = direction;
       _speed = speed;
       _demage = demage;
       
       Destroy(gameObject, destoryTime);
   }
    private void Update()
    {
        transform.position += _flightDirection * _speed * Time.deltaTime;
    }
}
