using UnityEngine;
using System.Collections;

public class Enemy : Entity {
	//enemy types
	public bool charger;
	public bool suicider;
	public bool mage;
	public bool gunner;
	public bool suiciderSpawn;
	
	//targeting the player variables
	Transform target; //the enemy's target
	Transform myTransform; //current transform data of this enemy

	//movement variables
	public Transform navTarget;
	NavMeshAgent agent;
	public float rotationSpeed;
	public float moveSpeed;

	//suicider explosion
	public GameObject ShootExplo;
	private float explosionDamage = 1f;

	//charger varables
	private bool chargerHit;
	private float chargerDelay = 20f;
	private bool chargerHitF;
	private float CHDF = 5f;

	//ranged enememies
	private bool inRange = false;

	public bool Aggro;

	void Start () {
		if (charger == true) {
			chargerHit = false;
			myTransform = transform; //cache transform data for easy access/preformance
			agent = GetComponent<NavMeshAgent> ();
			target = GameObject.FindWithTag("Player").transform; //target the player
		} 
		else if (suicider == true) {
			myTransform = transform; //cache transform data for easy access/preformance
			target = GameObject.FindWithTag("Player").transform; //target the player
			agent = GetComponent<NavMeshAgent> ();

		} 
		else if (mage == true) {
			myTransform = transform; //cache transform data for easy access/preformance
			target = GameObject.FindWithTag("Player").transform; //target the player
		} 
		else if (gunner == true) {

		} 
		else if (suiciderSpawn == true) {

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (charger == true) {
			if (Aggro == true) {
				if (chargerHit == true && chargerDelay > 0f) {
					myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime;
					chargerDelay --;
					if (chargerDelay == 0f) {
						chargerHit = false;
						chargerDelay = 20f;
					}
				} else {
					agent.SetDestination (navTarget.position);
					//myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
					//move towards the player
					//myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
				}
			}
		}
		else if (suicider == true) {
			if(Aggro == true){
				agent.SetDestination(navTarget.position);
				//myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
				//move towards the player
				//myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
				}
			} 
		else if (mage == true) {
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);

		} 
		else if (gunner == true) {
			
		} 
		else if (suiciderSpawn == true) {
			
		}
	}

	void OnTriggerEnter(Collider col){
		if (charger == true) {
		/*	if(col.gameObject.tag == "Enemy")
			{
				Debug.Log("charger hitting friend");
				myTransform.position -= col.transform.forward * moveSpeed * Time.deltaTime;
			}
*/
			if (col.gameObject.name == "Player") {	
			
				Debug.Log ("hit the player");
			
				GameObject player = GameObject.FindWithTag ("Player");
				player.GetComponent<Player> ().TakeDamage (1f);
				chargerHit = true;
			}
		} else if (suicider == true) {
			if (col.gameObject.name == "Player") {	
				
				Debug.Log ("hit the player");
				
				GameObject player = GameObject.FindWithTag ("Player");
				//player.GetComponent<Player>().TakeDamage(2f);
				TakeDamage (100f);
				AOE ();
			}
		} else if (mage == true) {
		
		}
		else if (gunner == true) {
			
		} 
		else if (suiciderSpawn == true) {
			
		}
	}

	private void AOE(){
		
		GameObject explo = Instantiate (ShootExplo, transform.position, transform.rotation) as GameObject;
		Collider[] Arround = Physics.OverlapSphere (transform.position, 3.0f);
		
		foreach (Collider inExp in Arround) {                      
			if (inExp.transform.tag == "Enemy")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if (inExp.transform.tag == "Player")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if (inExp.transform.tag == "Destructible")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if (inExp.transform.tag == "Turret")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if (inExp.transform.tag == "bullet")
				inExp.GetComponent<Projectiles> ().DestoryProjectile ();
			
			
		}
	}
}
