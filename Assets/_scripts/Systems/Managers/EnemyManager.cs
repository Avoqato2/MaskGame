using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   [SerializeField]private GameObject _enemyPrefab;
   public int NumberOfEnemies = 10;
   
   public float Offset = 15f;
   
   private PlayerController _playerController;
   
   private List<EnemyController> EnemysList = new List<EnemyController>();
   
   private void Update()
   {
       
   }

   private GameObject CreateEnemys()
   {
       for (int i = 0; i < NumberOfEnemies; i++)
       {
           
       }
       return null;
   }

   private Vector3 GetPlayersPositionWithOffest()
   {
       float OffsetRadius = Offset;
       Vector3 _playerPosition = _playerController.transform.position + new Vector3(0, OffsetRadius, 0);
       return _playerPosition;
   }
}
