using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nenuphar : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _anim.SetTrigger("Collision");
    }
}
