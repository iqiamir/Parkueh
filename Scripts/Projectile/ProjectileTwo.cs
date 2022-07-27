using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTwo : MonoBehaviour
{
    public GameObject impactEffect;
    public float radius = 3;
    public int damageAmount = 5;

    private void OnCollisionEnter(Collision collision)
    {
        //Idea - add audio here
        //FindObjectOfType<AudioManager>().Play("Explosion");
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 2);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.tag == "Player")
            {
                StartCoroutine(FindObjectOfType<PlayerManagerTwo>().TakeDamage(damageAmount));
            }
        }
        //Destroy(gameObject);
        this.enabled = false;
    }
}
