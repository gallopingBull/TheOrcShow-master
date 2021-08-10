using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (CharacterController))]


public class PlayerController : MonoBehaviour {
	//Handling
	public float rotationSpeed = 450;
	public float walkSpeed = 5;
	public float runSpeed = 8;
	public float acceleration = 5; 

	// System
	private Quaternion targetRotation;
	private Vector3 currentVelocityMod;
	public bool reloading, CurrentGun, CurrentMelee; 

	//Animation Test

	private Animator anim;


	//Components 

	public Transform handHold; 
	public Gun currentGun; 
	private CharacterController controller; 
	private Camera cam;

	public GameObject currentWeapon; 
	private int weaponCounter = 1; 

	// Melee shit 
	public Melee currentMelee; 
	private float holdTime; 
	public float holdLimit;

	//ammo hud controller
	public Text AmmoCount;
	private string ammoinmag;
	private string totalammo;
	private string outputstr;


	public GameObject spawn; 

	// Use this for initialization
	void Start () {

		controller = GetComponent<CharacterController>();
		cam = Camera.main;
	
		anim = GameObject.Find("walking").GetComponent<Animator> ();
		WeaponEquipped (0);


	}
	
	// Update is called once per frame
	void Update () {
		controlMouse (); 
		//controlWASD ();

		// Gun Input
		if (CurrentGun) {
			//anim.SetBool ("HoldingSword", false);
			if (Input.GetButtonDown ("Shoot")) {
				currentGun.Shoot ();
			} else if (Input.GetButton ("Shoot")) {
				currentGun.ShootContinuous (); 
			}
			if (Input.GetButtonDown ("Reload")) {
				if (currentGun.Reload ()) {
					Debug.Log ("Reloading"); 
					// Add reload animation here 
					reloading = true; 
				}
			}

			if (reloading) {
				currentGun.FinishReload ();

				Debug.Log ("Finishing Reload activated...");
				reloading = false; 

			} 
		}


		// Melee Input 

		if (CurrentMelee) {
			//anim.SetBool ("HoldingSword", true);
			if (Input.GetButton ("Shoot")) {
				holdTime += Time.deltaTime; 
				Debug.Log ("Holding"); 
				
			}

			if (Input.GetButtonUp ("Shoot")) {
				if (holdTime > holdLimit) {
					currentMelee.HeavyAttack (); 

					//Debug.Log ("Heavy Attk Ready"); 
					
				} else {
				
					currentMelee.LightAttack (); 

					//Debug.Log ("perform Light Attack");
				}

				holdTime = 0; 
			}

		}

		if (!CurrentMelee) {
			//anim.SetBool ("HoldingSword", false);
		}

		//Weapon Switch 

		if (Input.GetMouseButtonDown(1)){
		
			weaponSwitch();
		}



		//sets the ammo text read out
		ammoinmag = currentGun.currentAmmoInMag.ToString ();
		totalammo = currentGun.totalAmmo.ToString ();
		outputstr = ammoinmag + "/" + totalammo;
		AmmoCount.text = outputstr;


	}

	public void weaponSwitch(){

		weaponCounter++;
		switch(weaponCounter){
			
			case 1: {
				
				WeaponEquipped (0);
				break; 
			}
				
				
			default: {
				WeaponEquipped (1);
				weaponCounter=0;
				break; };
			
		}


	}

	void WeaponEquipped(int index){

		for (int i = 0; i < handHold.transform.childCount; i++) {
			
			if (i == index){
				handHold.transform.GetChild(i).gameObject.SetActiveRecursively(true);
				//currentWeapon = handHold.transform.GetChild(i).gameObject;
				if (handHold.transform.GetChild(i).gameObject.GetComponent<Weapon>().CurrentGun){
					Debug.Log ("i: "+ i); 
					if(handHold.transform.GetChild(i).gameObject.tag == "Gun"){
						currentGun= GameObject.Find("Rifle").GetComponent<Gun>();
						currentWeapon = handHold.transform.GetChild(i).gameObject;
						//currentWeapon = currentGun; 
						CurrentMelee = false;
						CurrentGun = true; 
						Debug.Log ("recognizing gun");   
					}
					else if(handHold.transform.GetChild(i).gameObject.tag == "MagicWeapon"){

						currentGun= GameObject.Find("MagicStaff").GetComponent<Gun>();
						currentWeapon = handHold.transform.GetChild(i).gameObject;
						//currentWeapon = currentGun;
						CurrentMelee = false;
						CurrentGun = true; 
						Debug.Log ("recognizing magic staff"); 


					}


				}

				else if (handHold.transform.GetChild(i).gameObject.GetComponent<Weapon>().CurrentMelee){
					Debug.Log ("i: "+ i); 
					currentMelee= GameObject.Find("MeleeTemplate").GetComponent<Melee>();
					currentWeapon = handHold.transform.GetChild(i).gameObject;
					//currentWeapon = currentMelee;
					CurrentMelee = true;
					CurrentGun = false; 
					Debug.Log ("recognizing sword"); 
				}

				/*else if (handHold.transform.GetChild(i).gameObject == null){
					Debug.Log ("i: "+ i); 
					currentMelee= GameObject.sFind("MeleeTemplate").GetComponent<Melee>();
					currentWeapon = handHold.transform.GetChild(i).gameObject;
					//currentWeapon = currentMelee;
					CurrentMelee = true;
					CurrentGun = false; 
					Debug.Log ("recognizing sword"); 
				}*/




			}

			/*else if(i!=index) {
			 	currentWeapon = null; 
				Debug.Log ("i: "+ i); 
				handHold.transform.GetChild(i).gameObject.SetActiveRecursively(false);
				CurrentMelee = false; 
				CurrentGun = false; 
				Debug.Log ("recognizing nothing"); 
			
				
			}*/
		
			else{
				handHold.transform.GetChild(i).gameObject.SetActiveRecursively(false);
				//CurrentMelee = false;
				//CurrentGun = false; 

			
			}
		
			
		}
	
	} 


	void controlMouse() {

		Vector3 mousePos = Input.mousePosition; 
		mousePos = cam.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
		targetRotation = Quaternion.LookRotation (mousePos - new Vector3(transform.position.x, 0, transform.position.z));
		transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		currentVelocityMod = Vector3.MoveTowards (currentVelocityMod, input, acceleration * Time.deltaTime); 
		Vector3 motion = currentVelocityMod;
		motion *= (Mathf.Abs (input.x) == 1 && Mathf.Abs (input.z) == 1) ? .7f : 1;
		motion *= (Input.GetButton ("Run")) ? runSpeed : walkSpeed; 
		motion += Vector3.up * -8; 
		
		controller.Move (motion * Time.deltaTime);
	}

	void controlWASD () {
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		
		if (input != Vector3.zero) {
			targetRotation = Quaternion.LookRotation (input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}	
		currentVelocityMod = Vector3.MoveTowards (currentVelocityMod, input, acceleration * 
			Time.deltaTime); 
		Vector3 motion = currentVelocityMod;
		motion *= (Mathf.Abs (input.x) == 1 && Mathf.Abs (input.z) == 1) ? .7f : 1;
		motion *= (Input.GetButton ("Run")) ? runSpeed : walkSpeed; 
		motion += Vector3.up * -8; 
		
		controller.Move (motion * Time.deltaTime); 
	}
	
}
