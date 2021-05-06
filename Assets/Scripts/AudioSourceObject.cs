using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceObject : MonoBehaviour
{
    void Update()
    {
        /// evite colocar isso no update... perceba só vc esta a cada frame:
        /// 1) pegando o componente
        /// 2) perguntando ao componente se ele terminou de tocar
        /// Faça um Enum que pega a duração do Clip e espera por ela, e então chama o metodo de desativar o audio...
        /// de uma olhada nesse link: https://forum.unity.com/threads/callback-for-audioclip-finished.251481/
        ///
        /// ps: sempre que possivel use Enums ao invés de algo no Update, pois quando o enum esta esperando é como se ele esperasse no "background" e com isso usa bem menos recurso de processamento.
        if (!GetComponent<AudioSource>().isPlaying)
        {
            gameObject.SetActive(false);
        }
    }

}
