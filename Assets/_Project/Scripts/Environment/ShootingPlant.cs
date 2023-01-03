using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlant : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _shootPosition;
    [SerializeField] private float _shootingDelay;

    private Animator _anim;

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = _shootPosition.transform.position;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 200, ForceMode.Force);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _anim = GetComponent<Animator>();
        StartCoroutine(ShootingDelay());
    }


    private IEnumerator ShootingDelay()
    {
        yield return new WaitForSeconds(_shootingDelay);
        _anim.SetTrigger("Shoot");
        StartCoroutine(ShootingDelay());
    }
}
