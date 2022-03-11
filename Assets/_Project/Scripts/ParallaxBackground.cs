using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject _camera;

    [SerializeField] private float _speed;
    [SerializeField] private float _spriteWidth;

    private float _startPosition;

    void Start()
    {
        Initialize();
    }

    void FixedUpdate()
    {
        ParallaxEffect();
    }

    private void Initialize()
    {
        _startPosition = transform.position.z;
    }

    private void ParallaxEffect()
    {
        float resetPosition = _camera.transform.position.z * (1 - _speed);
        float distance = _camera.transform.position.z * _speed;

        transform.position = new Vector3(transform.position.x, transform.position.y, _startPosition + distance);
        
        if(resetPosition > _startPosition + _spriteWidth / 2)
        {
            _startPosition += _spriteWidth;
        }
        else if (resetPosition < _startPosition - _spriteWidth / 2)
        {
            _startPosition -= _spriteWidth;
        }
    }
}
