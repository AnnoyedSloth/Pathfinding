using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public ParticleSystem explosion;
    public LayerMask mask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5.0f, mask);


        for(int a=0; a<colliders.Length; a++)
        {
            Rigidbody targetRigidbody = colliders[a].GetComponent<Rigidbody>();

            if (!targetRigidbody) continue;

            targetRigidbody.AddExplosionForce(50, transform.position, 5.0f);

            Enemy enemy = targetRigidbody.GetComponent<Enemy>();

            if (!enemy) continue;

            float damage = 30;
            print("Collide");

            enemy.TakeDamage(damage);

        }

        explosion.Play();

        Destroy(gameObject, 2.0f);

    }
}
