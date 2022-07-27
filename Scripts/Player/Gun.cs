using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage = 20;
    public float range = 100f;

    //reference from the camera
    public Camera fpsCam;
    //reference for the particle system
    public ParticleSystem muuzzleFlash;
    public GameObject impactEffect;
    //reference for adding force to the object shot
    public float impactForce = 60f;
    //recoil
    //public Rigidbody PlayerRb;
    //public float recoilForce;


    // Update is called once per frame
    void Update()
    {
        //Check if player press the fire button
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            FindObjectOfType<AudioManager>().Play("PlayerShoot");
        }
    }

    void Shoot ()
    {
        muuzzleFlash.Play();

        //Add recoil to player
        //PlayerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);

        //When hit enemy
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            //For Target Script
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            //For EnemyAI Script
            EnemyAi enemy = hit.transform.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                return;
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            //For Enemy Script
            Enemy e = hit.transform.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage);
                return;
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            //For Turret Script
            Turret turret = hit.transform.GetComponent<Turret>();
            if (turret != null)
            {
                turret.TakeDamage(damage);
                return;
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
