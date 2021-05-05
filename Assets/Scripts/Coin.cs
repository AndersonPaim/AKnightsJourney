using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public delegate void CollectCoinHandler();
    public static CollectCoinHandler OnCollectCoin;

    [SerializeField] private ParticleSystem _coinParticle1;
    [SerializeField] private ParticleSystem _coinParticle2;

    private void OnTriggerEnter(Collider other)
    {
        OnCollectCoin?.Invoke();
        _coinParticle1.Stop();
        _coinParticle2.Play();
        StartCoroutine(DestroyObject());
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }
}
