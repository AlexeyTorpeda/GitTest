using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private BlopBot _prefab;
    [SerializeField] private Vector2 _firstPosition;
    [SerializeField] private Vector2 _spawnRange = new Vector2(-6, 6);
    [SerializeField] private float _interval;

    private int _countOfEnemies = 5;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        var waitForIntervalSeconds = new WaitForSeconds(_interval);

        for (int i = 0; i < _countOfEnemies; i++)
        {
            Instantiate(_prefab, _firstPosition, Quaternion.identity);
            _firstPosition.x += Random.Range(_spawnRange.x, _spawnRange.y);
            yield return waitForIntervalSeconds;
        }
    }
}