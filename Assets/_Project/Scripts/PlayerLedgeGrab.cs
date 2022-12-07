using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLedgeGrab : MonoBehaviour
{
    [SerializeField] private Collider[] _greenBox;
    [SerializeField] private Collider[] _redBox;
    [SerializeField] private float _redXoffset;
    [SerializeField] private float _redYoffset;
    [SerializeField] private float _redXsize;
    [SerializeField] private float _redYsize;
    [SerializeField] private float _greenXoffset;
    [SerializeField] private float _greenYoffset;
    [SerializeField] private float _greenXsize;
    [SerializeField] private float _greenYsize;
    [SerializeField]private Rigidbody _rb;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _playerModel;

    public void Climbing()
    {
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), 0.2f).OnComplete(FinishClimbing);
    }

    private void FinishClimbing()
    {
        transform.DOMove(new Vector3(transform.position.x, transform.position.y, transform.position.z + (0.6f * _player.LastDirection)), 0.1f);
        _player.IsHanging = false;
        _rb.useGravity = true;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        FixPlayerPosition();
        LedgeCheck();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + _redYoffset, transform.position.z + ((_redXoffset * _player.LastDirection) * transform.localScale.z)), new Vector3(1, _redYsize, _redXsize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + _greenYoffset, transform.position.z + ((_greenXoffset * _player.LastDirection) * transform.localScale.z)), new Vector3(1, _greenYsize, _greenXsize));
    }

    private void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixPlayerPosition()
    {
        _playerModel.transform.localPosition = Vector3.zero;
        _playerModel.transform.localEulerAngles = Vector3.zero;
    }

    private void LedgeCheck()
    {
        _redBox = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + _redYoffset, transform.position.z + ((_redXoffset * _player.LastDirection) * transform.localScale.z)), new Vector3(1, _redYsize, _redXsize), transform.localRotation, _groundMask);
        _greenBox = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + _greenYoffset, transform.position.z + ((_greenXoffset * _player.LastDirection) * transform.localScale.z)), new Vector3(1, _greenYsize, _greenXsize), transform.localRotation, _groundMask);

        if(_greenBox.Length > 0 && _redBox.Length == 0 && !_player.IsHanging && _player.isJumping)
        {
            _player.IsHanging = true;
        }

        if(_player.IsHanging)
        {
            _rb.velocity = Vector3.zero;
            _rb.useGravity = false;
        }
    }
}
