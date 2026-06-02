using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static event Action AllEnemiesDead;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _parentContainer;
    [SerializeField] private int  _NumbOfEnemies = 10;
    [SerializeField] private float _creationRadius;
    [SerializeField] private float _creationDelayS;
    
    private int _realNumbOfEnemies;
    private bool _enemySpawning = true;
    private int _deadEnemies = 0;
    

    void Start()
    {
        _realNumbOfEnemies = _NumbOfEnemies;
        StartCoroutine(CreatePickupRoutine());
        EnemyController.OnEnemyDead += CountDeadEnemies;
    }
    
    private IEnumerator CreatePickupRoutine()
    {
        while (_enemySpawning)
        {
            CreatePickup();
            _NumbOfEnemies--;
            if (_NumbOfEnemies <= 0)
            {
                _enemySpawning = false;
            }
            yield return new WaitForSeconds(_creationDelayS);
        }
    }

    private void CountDeadEnemies(int deadEnemie)
    {
        _deadEnemies += deadEnemie;
        if(_deadEnemies >= _realNumbOfEnemies)
        {
            AllEnemiesDead?.Invoke();
        }
    }

    private GameObject CreatePickup()
    {
        Vector2 position2D = Random.insideUnitCircle * _creationRadius;
        Vector3 position = new Vector3(position2D.x, 0, position2D.y);
        GameObject enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _parentContainer);
        return enemy;
    }

    private void OnDestroy()
    {
        EnemyController.OnEnemyDead -= CountDeadEnemies;
    }
}