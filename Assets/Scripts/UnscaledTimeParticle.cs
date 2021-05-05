using UnityEngine;
using System.Collections;

public class UnscaledTimeParticle : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale < 0.01f)
        {
            GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);
            //tocar particle com jogo pausado / final da cena
        }
    }
}