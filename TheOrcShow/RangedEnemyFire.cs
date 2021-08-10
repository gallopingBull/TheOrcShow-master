using UnityEngine;
using System.Collections;

public class RangedEnemyFire : MonoBehaviour {

	public float attackTimer;
	public float coolDown;
	
	// System:
	private float secondsBetweenShots;
	private float nextPossibleShootTime;
	public int bulletSpeed = 30;
	public Rigidbody bullet; 
	
	
	public GameObject SpawnObject;
	public Vector3 spawn;



	// Use this for initialization
	void Start () {
		spawn = SpawnObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(attackTimer > 0)
			attackTimer -= Time.deltaTime;
		
		if(attackTimer < 0)
			attackTimer = 0;
		
		if (attackTimer == 0) {
			
			Rigidbody clone;
			clone = Instantiate (bullet, spawn, transform.rotation) as Rigidbody;//transform to local position -- keeps spawning at 0,0,0
			clone.velocity = transform.TransformDirection (Vector3.forward * bulletSpeed);
			
			attackTimer = coolDown;

			nextPossibleShootTime = Time.time + secondsBetweenShots;

			//GetComponent<AudioSource> ().Play ();
		}
	}
	void OnTriggerEnter(Collider col){
		
		
		if (col.gameObject.tag == "Player") {

		}
	}
}