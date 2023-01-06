using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ThrowingSword : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _throwingSwordObj;
    [SerializeField] private GameObject _originalSwordObj;
    [SerializeField] private Collider _hitCollider;
    [SerializeField] private float _throwDistance;
    [SerializeField] private float _travelTime;

    private bool _isThrowing = false;
    private bool _isReturning = false;
    private Vector3 _throwPos;

    public void ThrowSword()
    {
        StartCoroutine(ThrowSwordDelay());
    }

    private IEnumerator ThrowSwordDelay()
    {
        yield return new WaitForSeconds(0.4f);
        _hitCollider.enabled = true;
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 1, _player.transform.position.z);
        _throwingSwordObj.SetActive(true);
        _originalSwordObj.SetActive(false);
        _isThrowing = true;
        _throwPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + (_throwDistance * _player.LastDirection));
        StartCoroutine(ReturnSword());
    }

    private void Update()
    {
        if(_isThrowing)
        {
            transform.Rotate(Vector3.right * 2000 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _throwPos, Time.deltaTime * 40);
        }
        else if(_isReturning)
        {
            transform.Rotate(Vector3.right * 2000 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y + 1, _player.transform.position.z), Time.deltaTime * 40);
        }

        if(_isReturning && Vector3.Distance(_player.transform.position, transform.position) < 1.5f)
        {
            SwordReturned();
        }
    }

    private void SwordReturned()
    {
        _originalSwordObj.SetActive(true);
        _throwingSwordObj.SetActive(false);
        _isReturning = false;
        _hitCollider.enabled = false;
    }

    private IEnumerator ReturnSword()
    {
        yield return new WaitForSeconds(_travelTime);
        _isThrowing = false;
        _isReturning = true;
    }
}
