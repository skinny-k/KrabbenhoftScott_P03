using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseBehavior : MonoBehaviour
{
    public AudioSource sfxPlayer;
    public AudioClip fallSFX;
    public bool isConcealed = false;

    void Start()
    {
        gameObject.tag = "Corpse";
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = true;
        StartCoroutine(PlayDropSFX());
    }

    IEnumerator PlayDropSFX()
    {
        yield return new WaitForSeconds(0.6f);
        sfxPlayer.PlayOneShot(fallSFX);
    }
}
