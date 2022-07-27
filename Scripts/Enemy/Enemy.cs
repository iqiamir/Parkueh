using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public Healthbar healthbar;
    public GameObject projectile;
    public Transform projectilePoint;

    public Animator animator;

    void Start()
    {
        healthbar.SetMaxHealth(health);
    }

    public void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("EnemyShoot");
        Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 42f, ForceMode.Impulse);
        rb.AddForce(transform.up * 1, ForceMode.Impulse);
        Destroy(rb, 3);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetHealth(health);

        if (health <= 0f)
        {
            //Invoke(nameof(DestroyEnemy), 0.5f);

            //Play Death Animation
            animator.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled = false;

            DisableCanvas();
        }
        else
        {
            //Play Damage Animation
            animator.SetTrigger("damage");
        }
    }

    void DisableCanvas()
    {
        healthbar.enabled = false;
    }
}
