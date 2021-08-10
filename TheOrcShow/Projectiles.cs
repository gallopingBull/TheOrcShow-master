using UnityEngine;
using System.Collections;

public class Projectiles : Entity {

	public GameObject ShootExplo; 
	public float damage;
	public bool ifMagic; 
	public int collisionCount; 
	private float turretDamage = 10; 
	private float explosionDamage = 2; 
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnTriggerEnter(Collider col) {

		Debug.Log( "collide (name) : " + col.GetComponent<Collider>().gameObject.name );
		collisionCount++; 
		Debug.Log ("collision count =" + collisionCount); 
		//DestoryProjectile (); 

		if (collisionCount == 1) {

			switch (collisionCount) {
			
				case 1:

					if (col.gameObject.tag == "Enemy") {
						if(ifMagic){
						AOE();
						Debug.Log("AOE Damage");
					}
							
						//Debug.Log ("Hit Enemy_Charger");
						col.GetComponent<Entity> ().TakeDamage (damage);
						DestoryProjectile (); 
					} 


					else if (col.gameObject.tag == "Player") {
						if(ifMagic){
							AOE();
							Debug.Log("AOE Damage");
						}
				
						DestoryProjectile (); 
						//Debug.Log ("Hit Player");
						col.GetComponent<Entity> ().TakeDamage (damage);
					} 


					else if (col.gameObject.tag == "Wall") {
						if(ifMagic){
							AOE();
							Debug.Log("AOE Damage");
						}
						DestoryProjectile (); 
						//Debug.Log ("Hit Wall");

					} 

					else if (col.gameObject.tag == "Destructible") {

						if(ifMagic){
							AOE();
							Debug.Log("AOE Damage");
						}
						DestoryProjectile (); 
						//Debug.Log ("Hit Destructible");
						col.GetComponent<Entity> ().TakeDamage (damage);

					}

				else if (col.gameObject.tag == "DM") {
					
					if(ifMagic){
						col.GetComponent<Entity> ().TakeDamage (damage);
						AOE();
						Debug.Log("AOE Damage");
					}
					DestoryProjectile (); 
					//Debug.Log ("Hit Destructible");
					//col.GetComponent<Entity> ().TakeDamage (damage);
					
				}

					else if ( ifMagic && col.gameObject.tag == "Bullet") {
						//Debug.Log ("Destory Bullet");
						col.GetComponent<Projectiles> ().DestoryProjectile ();
					
					}

					else if (col.gameObject.tag == "Turret") {
						if(ifMagic){
							AOE();
							Debug.Log ("Hit Turret");
							col.GetComponent<Entity> ().TakeDamage (turretDamage);
							Debug.Log("AOE Damage");
						}
						else {
							col.GetComponent<Entity> ().TakeDamage (damage);
						}
					
					DestoryProjectile (); 
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
	
		if(collisionCount != 0){
			if (ifMagic){
				collisionCount =0;
			}
			else{
			collisionCount = -1; 
			}
		}	
	}

	private void AOE(){

		GameObject explo = Instantiate(ShootExplo,transform.position,transform.rotation) as GameObject;
		Collider[] Arround = Physics.OverlapSphere(transform.position,3.0f);

		foreach(Collider inExp in Arround)
		{                      
			if(inExp.transform.tag == "Enemy")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);

			else if(inExp.transform.tag == "Player")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if(inExp.transform.tag == "Destructible")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if(inExp.transform.tag == "DM")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);
			else if(inExp.transform.tag == "Turret")
				inExp.GetComponent<Entity> ().TakeDamage (explosionDamage);

			else if(inExp.transform.tag == "bullet")
				inExp.GetComponent<Projectiles> ().DestoryProjectile ();
			
			
		}

	}

	public void DestoryProjectile (){

		Destroy (this.gameObject);
		// instantiate particle here 
		// play audio of projectile hitting object 

	}
}
