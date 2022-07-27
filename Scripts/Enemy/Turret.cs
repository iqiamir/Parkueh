using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform Player;
    float distance;
    public float detectDistance;
    public float projectileSpeed;
    public Transform head, projectilePoint;
    public GameObject projectile;
    public float fireRate, nextFire;
    public int health;
    public Healthbar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        healthbar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Player.position, transform.position);

        if(distance <= detectDistance)
        {
            head.LookAt(Player);
            if(Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        FindObjectOfType<AudioManager>().Play("EnemyShoot");
        GameObject clone = Instantiate(projectile, projectilePoint.position, head.rotation);
        clone.GetComponent<Rigidbody>().AddForce(head.forward * projectileSpeed);
        Destroy(clone, 3);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetHealth(health);
        if (health <= 0f)
        {
            DestroyEnemy();
            //Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
