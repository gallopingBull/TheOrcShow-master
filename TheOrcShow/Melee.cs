using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melee : Weapon{

	public List<GameObject> targets;
	public float attackTimer;
	public float coolDown;
	public float lightAtkDmg, heavyAtkDmg; 
	public int collisionCount; 
	//public float minDistance = 4f; 
	private bool heavyAttacking, lightAttacking; 

	int numEnemies; 
	float connected; 

	public Rigidbody projectile; 
	public int bulletSpeed = 30;
	public Transform returnSpawn; 
	public Transform returnSpawnL;
	public Transform returnSpawnR; 

	//Animation Test
	
	private Animator anim;


	// Use this for initialization
	void Start () {

		anim = GameObject.Find("walking").GetComponent<Animator> ();
		targets = new List<GameObject> (); 

		GameObject[] moreMoreTargets = GameObject.FindGameObjectsWithTag ("Destructible"); // add destructibles to player target list
		if (moreMoreTargets != null) {
			foreach (GameObject go in moreMoreTargets) {
				targets.Add (go);
			}
		}   
		GameObject[] moremoreMoreTargets = GameObject.FindGameObjectsWithTag ("Turret"); // add destructibles to player target list
		if (moremoreMoreTargets != null) {
			foreach (GameObject go in moremoreMoreTargets) {
				targets.Add (go);
			}
		}   


		AddEnemy ();

	}

	// Update is called once per frame
	void Update () {

		//Removes any missing objects from the list// 
		for (int i=targets.Count -1; i>-1; i--) {
			if(targets[i]==null){
				targets.RemoveAt(i); 
			}
		} 
		//Debug.DrawLine (target.transform.position, transform.position, Color.yellow);
		
		/*if(attackTimer > 0)
			attackTimer -= Time.deltaTime;
		
		if(attackTimer < 0)
			attackTimer = 0;
		
		if(Input.GetKeyUp(KeyCode.F)) {
			if(attackTimer == 0) {
				LightAttack();
				attackTimer = coolDown;
			}
		}*/ 

	
	}



	void OnTriggerEnter(Collider col) {
		collisionCount ++; 
		Debug.Log ("collision count =" + collisionCount); 

		
		if (collisionCount == 1) {
			
			switch (collisionCount) {
				
			case 1:
				
				if (col.gameObject.tag == "Enemy") {
					
					//AddEnemy();

				} 

				
				else if (col.gameObject.tag == "Magic") {

					AddProjectile();
				} 


				
				else if (col.gameObject.tag == "Destructible") {
					//AddTarget();
					
				}

				break;
				
			case 2:
				Debug.Log ("Colliding more than once");
				break;
				
			default:
				Debug.Log ("Fuck you");
				break; 
			}
		}
		
		collisionCount = 0; 

	}
	

	public void AddProjectile (){ 


		GameObject[] moreTargets = GameObject.FindGameObjectsWithTag ("Magic");
		if (moreTargets != null) {
			foreach (GameObject go in moreTargets) {
				targets.Add (go);
			}
		}    
	
	}

	public void AddEnemy (){ 
		
		GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag ("Enemy");
		if (enemyTargets != null) {
			foreach (GameObject go in enemyTargets) {
				targets.Add (go);
			}
		}

	}

	

	public void LightAttack(){

		//anim.SetBool ("HoldingSword", true);
		lightAttacking = true; 
		heavyAttacking = false;
		bool isAttacking = true; 
		float lightDistance; 
		Debug.Log ("Performing Light Attck"); 
		if (targets == null) return;
		//anim.SetBool("Swing", true);


		foreach (GameObject target in targets) {//** FIX THIS: This multiplies damage by each enemy in an array**//
			if (target != null) {
				lightDistance = Vector3.Distance (target.transform.position, transform.position);
				//Debug.Log ("Light Distance" + lightDistance); 
				Vector3 Dir = (target.transform.position - transform.position).normalized;
				float Direction = Vector3.Dot (Dir, transform.forward);

				if (lightDistance < 1.5f && target.gameObject.tag == "Enemy") {
					if (Direction > 0) {
						if (target != null) {
							Debug.Log ("Hit Enemy w/light");
							target.GetComponent<Entity> ().TakeDamage (lightAtkDmg);
						}

					}
				} else if (lightDistance < 1.5f && target.gameObject.tag == "Destructible") {
					if (Direction > 0) {
						if (target != null) {
							Debug.Log ("Hit Destructible w/light");
							target.GetComponent<Entity> ().TakeDamage (lightAtkDmg);
						}
					}
				} else if (lightDistance < 1.5f && target.gameObject.tag == "Magic") {

					target.GetComponent<Projectiles> ().DestoryProjectile ();
					if (Direction > 0 && lightDistance > 1f) {
						if (target != null) {
							Deflect (); 
						} 

					}
				}
				else if (lightDistance < 1.5f && target.gameObject.tag == "Turret") {
					if (Direction > 0) {
						if (target != null) {
							Debug.Log ("Hit Enemy w/light");
							target.GetComponent<Entity> ().TakeDamage (lightAtkDmg);
						}
						
					}
				}
			
			}
		}
		//anim.SetBool("Swing", false);
		//anim.SetBool ("HoldingSword", false);
		lightAttacking = false; 
		isAttacking = false; 
	}
	


	public void HeavyAttack(){
		lightAttacking = false; 
		heavyAttacking = true;
		bool isAttacking = true; 
		float heavyDistance; 
		Debug.Log ("Performing Heavy Attack"); 
		//anim.SetBool("Swing", true);
		if (targets == null) return;
	
		
		foreach (GameObject target in targets) //** FIX THIS: This multiplies damage by each enemy in an array**//
		{
			if (target != null)
				
			{
				heavyDistance = Vector3.Distance(target.transform.position, transform.position);
				//Debug.Log ("Heavy Distance" + heavyDistance); 
				Vector3 Dir = (target.transform.position - transform.position).normalized;
				float Direction = Vector3.Dot (Dir, transform.forward);
				
				if (heavyDistance < 1.5f&& target.gameObject.tag == "Enemy") 
				{
					if(Direction>0){
						if (target != null)
						{
							Debug.Log ("Hit Enemy w/heavy");
							target.GetComponent<Entity> ().TakeDamage (heavyAtkDmg);
						}
						
					}
				}
				
				else if(heavyDistance < 1.5f&& target.gameObject.tag == "Destructible"){
					if(Direction>0){
						if (target != null)
						{
							Debug.Log ("Hit Destructible w/heavy");
							target.GetComponent<Entity> ().TakeDamage (lightAtkDmg);
						}
					}
				}

				else if (heavyDistance < 1.5f&& target.gameObject.tag == "Turret") 
				{
					if(Direction>0){
						if (target != null)
						{
							Debug.Log ("Hit Enemy w/heavy");
							target.GetComponent<Entity> ().TakeDamage (heavyAtkDmg);
						}
						
					}
				}
				
				else if(heavyDistance < 1.5f&& target.gameObject.tag == "Magic"){

					target.GetComponent<Projectiles> ().DestoryProjectile ();
					if(Direction>0 && heavyDistance > 1f){
						if (target != null)
						{
							Deflect (); 
						}

					}
				}
				
			}	
		}
		//anim.SetBool("Swing", false);
		//anim.SetBool ("HoldingSword", false);
		heavyAttacking = false;
		isAttacking = false; 
	}

	private void Deflect(){

		if (lightAttacking) {

			Debug.Log ("deflected w/ light");
			Rigidbody clone;
			clone = Instantiate (projectile, returnSpawn.position, transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection (Vector3.forward * bulletSpeed);
		} else if (heavyAttacking) {
			// make heavy deflection return a spread instead of a single projectile
			Debug.Log ("deflected w/ heavy");
			Rigidbody clone;
			clone = Instantiate (projectile, returnSpawn.position, transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection (Vector3.forward * bulletSpeed);
			clone = Instantiate (projectile, returnSpawnL.position, transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection (Vector3.forward * bulletSpeed);
			clone = Instantiate (projectile, returnSpawnR.position, transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection (Vector3.forward * bulletSpeed);
		}

	}

}