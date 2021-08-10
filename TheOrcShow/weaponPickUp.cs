 using UnityEngine;
using System.Collections;

public class weaponPickUp : MonoBehaviour {

	Vector3 dropPosition; 
	 GameObject currentWeapon; 
	public Transform pickUpPosition;
	public Transform oW; 
	public int childrenInHand; 
	private PlayerController player; 



	// Use this for initialization
	void Start () {

	
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		oW = player.handHold; 
		Vector3 pickUpPosition = new Vector3 (oW.transform.position.x, 0, oW.transform.position.z); 

	}
	
	// Update is called once per frame
	void Update () {
		currentWeapon = player.currentWeapon;
		childrenInHand = GameObject.Find ("Hand").transform.childCount;
	
	}
	void OnTriggerEnter(Collider col)
	{
		Debug.Log ("walked in");
		if (col.gameObject.tag == "Player") {
			 Debug.Log("Press E to pick up weapon.");
			if (Input.GetKeyDown(KeyCode.E)){
				if(currentWeapon != null){
					dropWeapon(); 
					//Do weapon switch here
					//pickUpWeapon();
					Debug.Log ("switched weapon"); 

				}

				else if (currentWeapon == null){
					pickUpWeapon();
					Debug.Log ("Picked up");
					//if 
				}

				else if (currentWeapon == null && childrenInHand > 0){
					//player.weaponSwitch(); 
					Debug.Log("Weapon test"); 
					pickUpWeapon();

				}
			}
		}
	}

	void dropWeapon(){
		currentWeapon = player.currentWeapon;
		dropPosition = transform.position; 
		currentWeapon.transform.position = dropPosition;
		currentWeapon.GetComponent<weaponPickUpTest> ().enabled = true; 
		currentWeapon.transform.parent = null;


		pickUpWeapon();
		//statement where it checks the weapon type (gun, melee) and sets it 
		//GameObject.Find("Player").GetComponent<PlayerController> ().currentWeapon = null;
		//this.gameObject.SetActiveRecursively(false);

	}

	void pickUpWeapon(){


		if (player.CurrentGun == true) {
			GameObject.Find ("Player").GetComponent<PlayerController> ().currentWeapon = null;
			//currentWeapon.transform.parent = null;
			Debug.Log("TESTING"); 
			GameObject.Find ("Player").GetComponent<PlayerController> ().CurrentGun = false;
		} 

		else if (player.CurrentMelee == true) {
			GameObject.Find ("Player").GetComponent<PlayerController> ().currentWeapon = null;
				//currentWeapon.transform.parent = null;
			GameObject.Find ("Player").GetComponent<PlayerController> ().CurrentMelee = false; 
		}

		transform.parent = player.handHold; 
		//transform.parent.rotation = player.handHold.rotation; 
		currentWeapon = this.gameObject; 
		player.currentWeapon = currentWeapon; 

		if (currentWeapon.tag == "MagicWeapon") {
			GameObject.Find ("Player").GetComponent<PlayerController> ().CurrentGun = true;
			player.currentWeapon = currentWeapon; 
			player.currentGun=  GameObject.Find("MagicStaff").GetComponent<Gun>();

		}

		if (currentWeapon.tag == "Gun") {
			GameObject.Find ("Player").GetComponent<PlayerController> ().CurrentGun = true;
			player.currentWeapon = currentWeapon; 
			player.currentGun=  GameObject.Find("Rifle").GetComponent<Gun>();

		}

		else if (currentWeapon.tag == "Melee") {
			GameObject.Find ("Player").GetComponent<PlayerController> ().CurrentMelee = true; 
			player.currentWeapon = currentWeapon;
			player.currentMelee=  GameObject.Find("MeleeTemplate").GetComponent<Melee>();
		}
		/*else if (currentWeapon == null) {
			//GameObject.Find ("Player").GetComponent<PlayerController> ().CurrentMelee = true; 
			Debug.Log ("Confused"); 
		}*/
		this.GetComponent<weaponPickUpTest> ().enabled = false; 


	}
}
