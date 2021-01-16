using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPill : MonoBehaviour
{
    public GameObject pickupVFX;
    [SerializeField] float durationOfPickupVFX = 1f;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] [Range(0,1)] float pickupSoundVolume = 0.7f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Pill collision registered with");
        Debug.Log(collision.tag);
        if (collision.tag == "Player")
        {
            Pickup();
        }
    }


    private void Pickup()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(pickupVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfPickupVFX);
        AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position, pickupSoundVolume);
    }
}
