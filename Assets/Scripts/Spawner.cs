using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Vector2 _firstPosition;
    [SerializeField] private float _step;
    [SerializeField] private float _interval;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            Instantiate(_gameObject, _firstPosition, Quaternion.identity);
            _timer -= _interval;
            _firstPosition.x += _step;
        }
    }
}
