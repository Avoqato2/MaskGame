using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
   [SerializeField] private GameObject LodingZone;

   private void Start()
   {
      EnemyManager.AllEnemiesDead += SpawnLodingZone;
      PlayerHealth.PlayerDead += HandlePlayerDeath;
   }

   private void SpawnLodingZone()
   {
      LodingZone.SetActive(true);
   }

   private void HandlePlayerDeath()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
   
   private void OnDestroy()
   {
      EnemyManager.AllEnemiesDead -= SpawnLodingZone;
      PlayerHealth.PlayerDead -= HandlePlayerDeath;
   }
}
