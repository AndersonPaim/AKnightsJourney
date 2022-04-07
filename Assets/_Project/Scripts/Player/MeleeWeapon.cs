using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitParticle;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if(damageable != null)
        {
            Debug.Log("HIT ENEMY");
            damageable.TakeDamage(100);
            //Knockback(other.gameObject);
            //Instantiate(_hitParticle, other.transform.position, other.transform.rotation);
        }
    }

    private void Knockback(GameObject obj)
    {
        Vector3 dir;
        Rigidbody rb = obj.GetComponent<Rigidbody>();


        if(obj.transform.eulerAngles.y < 90)
        {
            dir = new Vector3(0, 0, -1);
        }
        else
        {
            dir = new Vector3(0, 0, 1);
        }

        rb.AddForce(dir * 30, ForceMode.Impulse);
    }
}
