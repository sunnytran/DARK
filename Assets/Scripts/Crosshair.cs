using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crosshair : MonoBehaviour
{
	
	public Texture2D tex;
	
	int lookAt = -1;
	const int UNLIT_TORCH = 0;
	const int UNLIT_TORCH_NO_TINDERBOX = 1;
	const int KEY = 2;
	const int LOCKED_DOOR = 3;
	const int UNLOCKED_DOOR = 4;
	const int LEVER = 5;
	const int LOCKED_DOOR_LEVER = 6;
	const int PRESSURE_PLATE_UNTRIGGERED = 7;
	const int PRESSURE_PLATE_TRIGGERED = 8;
	const int LOCKED_DOOR_PRESSURE_PLATE = 9;
	const int CRATE = 10;
	const int CRATE_ALREADY_CARRYING = 11;
	const int CRATE_PLACE = 12;
	const int PISTOL = 13;
	const int SHOTGUN = 14;
	const int SMG = 15;
	const int TINDERBOX = 16;
	
	GameObject doorLook;
	
	public GameObject cratePf;
	
	void Start()
	{
		
	}
	
	void Update()
	{
		Interact();
	}
	
	void Interact()
	{
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		Transform transf;
		PlayerProperties pp = GameObject.Find("Player").GetComponent<PlayerProperties>();
		if (Physics.Raycast(ray, out hit, 3))
		{
			transf = hit.transform;
			while (transf.parent != null && transf.parent.name != "Level")
				transf = transf.parent;
						
			string name = transf.transform.name;
			bool mouse = Input.GetMouseButtonDown(1);
			if (name == "UnlitTorch" && pp.hasTinderbox && mouse)
			{
				transf.name = "Torch";
				transf.GetComponent<Light>().enabled = true;
				transf.GetChild(0).GetComponent<ParticleSystem>().enableEmission = true;
				transf.GetComponent<TorchBehavior>().lit = true;
				GameObject.Find("Sound").GetComponent<Sound>().lightTorch.Play();
			}
			else if (name == "UnlitTorch" && pp.hasTinderbox)
				lookAt = UNLIT_TORCH;
			else if (name == "UnlitTorch")
				lookAt = UNLIT_TORCH_NO_TINDERBOX; 
			else if (name == "Key" && mouse)
			{
				GameObject.Find("Player").GetComponent<PlayerProperties>().keys.Add(transf.gameObject.GetComponent<SpriteRenderer>().color);
				GameObject.Find("Sound").GetComponent<Sound>().pickupKey.Play();
				Destroy(transf.gameObject);
			}
			else if (name == "Key")
				lookAt = KEY;
			else if (name == "Door" && mouse)
			{
				DoorBehavior db = transf.GetComponent<DoorBehavior>();
				List<Color> keys = pp.keys;
				if (db.unlocked)
					transf.GetComponent<DoorBehavior>().Toggle();
				else if (keys.Count == 0 || GetKey(pp, transf.gameObject) == -1)
				{
					if (!GameObject.Find("Sound").GetComponent<Sound>().doorLocked.isPlaying)
						GameObject.Find("Sound").GetComponent<Sound>().doorLocked.Play();
				}
				else if (GetKey(pp, transf.gameObject) != -1)
				{
					transf.GetComponent<DoorBehavior>().Toggle();
					transf.GetComponent<DoorBehavior>().unlocked = true;
					GameObject.Find("Player").GetComponent<PlayerProperties>().keys.RemoveAt(GetKey(pp, transf.gameObject));
				}
			}
			else if (name == "Door" && !transf.GetComponent<DoorBehavior>().unlocked)
			{
				lookAt = LOCKED_DOOR;
				doorLook = transf.gameObject;
			}
			else if (name == "Door" && transf.GetComponent<DoorBehavior>().unlocked)
			{
				lookAt = UNLOCKED_DOOR;
				doorLook = transf.gameObject;
			}
			else if (name == "Lever" && mouse)
				transf.GetComponent<LeverBehavior>().Toggle();
			else if (name == "Lever")
				lookAt = LEVER;
			else if (name == "DoorLever" && !transf.GetComponent<DoorBehavior>().unlocked)
				lookAt = LOCKED_DOOR_LEVER;
			else if (name == "PressurePlate" && !transf.GetComponent<PressurePlateBehavior>().triggered)
				lookAt = PRESSURE_PLATE_UNTRIGGERED;
			else if (name == "PressurePlate")
				lookAt = PRESSURE_PLATE_TRIGGERED;
			else if (name == "DoorPressurePlate")
				lookAt = LOCKED_DOOR_PRESSURE_PLATE;
			else if (name == "Crate" && !pp.isCarrying && mouse)
			{
				Destroy(transf.gameObject);
				GetComponent<PlayerProperties>().isCarrying = true;
				GameObject.Find("Sound").GetComponent<Sound>().pickupBox.Play();
			}
			else if (name == "Crate" && !pp.isCarrying)
				lookAt = CRATE;
			else if (name == "Crate")
				lookAt = CRATE_ALREADY_CARRYING;
			else if (name == "Pistol" && mouse)
			{
				transf.parent = transform.GetChild(0);
				transf.GetComponent<PickupableBehavior>().enabled = false;
				transf.GetComponent<BoxCollider>().enabled = false;
				transf.localPosition = new Vector3(-0.5f, -0.4f, 1);
				transf.localRotation = Quaternion.identity;
				transf.Rotate(new Vector3(0, 270, 0));
				transf.gameObject.layer = 8;
				
				GameObject.Find("Sound").GetComponent<Sound>().pickupPistol.Play();
			}
			else if (name == "Pistol")
				lookAt = PISTOL;			
			else if (name == "Shotgun" && mouse)
			{
				transf.parent = transform.GetChild(0);
				transf.GetComponent<PickupableBehavior>().enabled = false;
				transf.GetComponent<BoxCollider>().enabled = false;
				transf.localPosition = new Vector3(-0.5f, -0.4f, 1);
				transf.localRotation = Quaternion.identity;
				transf.Rotate(new Vector3(0, 270, 0));
				transf.gameObject.layer = 8;
				
				GameObject.Find("Sound").GetComponent<Sound>().pickupShotgun.Play();
			}
			else if (name == "Shotgun")
				lookAt = SHOTGUN;				
			else if (name == "SMG" && mouse)
			{
				transf.parent = transform.GetChild(0);
				transf.GetComponent<PickupableBehavior>().enabled = false;
				transf.GetComponent<BoxCollider>().enabled = false;
				transf.localPosition = new Vector3(-0.5f, -0.4f, 1);
				transf.localRotation = Quaternion.identity;
				transf.Rotate(new Vector3(0, 270, 0));
				transf.gameObject.layer = 8;
				
				GameObject.Find("Sound").GetComponent<Sound>().pickupSMG.Play();
			}
			else if (name == "SMG")
				lookAt = SMG;
			else if (name == "Tinderbox" && mouse)
			{
				GetComponent<PlayerProperties>().hasTinderbox = true;
				Destroy(transf.gameObject);
				
				GameObject.Find("Sound").GetComponent<Sound>().pickupTinderbox.Play();
			}
			else if (name == "Tinderbox")
				lookAt = TINDERBOX;
			else
				lookAt = -1;
			
			if (pp.isCarrying && (name == "Floor" || name == "HalfBlock" || name == "PressurePlate") && mouse)
			{
				float x = hit.point.x;
				float y = 1;
				float z = hit.point.z;
				if (name == "HalfBlock")
					y = 1.15f;
				else if (name == "PressurePlate")
					y = 1.2f;
				
				GameObject crate = (GameObject)Instantiate(cratePf, new Vector3(x, y, z),  Quaternion.identity);
				crate.name = "Crate";
				crate.transform.parent = GameObject.Find("Level").transform;
				
				GetComponent<PlayerProperties>().isCarrying = false;
				GameObject.Find("Sound").GetComponent<Sound>().dropBox.Play();
			}
			else if (pp.isCarrying && (name == "Floor" || name == "HalfBlock" || name == "PressurePlate"))
				lookAt = CRATE_PLACE;
		}
	}
	
	int GetKey(PlayerProperties pp, GameObject door)
	{
		List<Color> keys = pp.keys;
		
		for (int i = 0; i < keys.Count; i++)
		{
			if (keys[i].ToHexStringRGB() == Color.yellow.ToHexStringRGB() && door.transform.GetChild(0).GetComponent<MeshRenderer>().material.color == new Color(0.5f, 0.2f, 0, 0) ||
			    keys[i] == door.transform.GetChild(0).GetComponent<MeshRenderer>().material.color)
			    return i;
		}
		return -1;
	}
	
	void OnGUI()
	{
		float x = (Screen.width / 2) - (tex.width / 2);
		float y = (Screen.height / 2) - (tex.height / 2);
		GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
		
		PlayerProperties pp = GameObject.Find("Player").GetComponent<PlayerProperties>();
		if (lookAt == UNLIT_TORCH)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Light the Torch");
		else if (lookAt == UNLIT_TORCH_NO_TINDERBOX)
			GUI.Label(new Rect(10, 10, 500, 20), "This Torch is Unlit");
		else if (lookAt == KEY)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Pick Up the Key");
		else if (lookAt == UNLOCKED_DOOR || (lookAt == LOCKED_DOOR && pp.keys.Count > 0 && GetKey(pp, doorLook) != -1))
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Open/Close the Door");
		else if (lookAt == LOCKED_DOOR)
			GUI.Label(new Rect(10, 10, 500, 20), "You either do not have the correct Key or any Key at all");
		else if (lookAt == LEVER)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Flip Lever");
		else if (lookAt == LOCKED_DOOR_LEVER)
			GUI.Label(new Rect(10, 10, 500, 20), "You have to Flip a Lever to open the Door");
		else if (lookAt == PRESSURE_PLATE_UNTRIGGERED)
			GUI.Label(new Rect(10, 10, 500, 20), "Apply Pressure to this Plate for it to Trigger");
		else if (lookAt == PRESSURE_PLATE_TRIGGERED)
			GUI.Label(new Rect(10, 10, 500, 20), "The Plate has been Triggered and something has Opened");
		else if (lookAt == LOCKED_DOOR_PRESSURE_PLATE)
			GUI.Label(new Rect(10, 10, 500, 20), "You have to Trigger a Pressure Plate to open the Door");
		else if (lookAt == CRATE)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Pick Up the Crate");
		else if (lookAt == CRATE_ALREADY_CARRYING)
			GUI.Label(new Rect(10, 10, 500, 20), "You are already Carrying a Crate. Right Click in order to drop the Crate that you are Carrying");
		else if (lookAt == CRATE_PLACE)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Place the Crate");
		else if (lookAt == PISTOL)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Pick Up the Pistol");
		else if (lookAt == SHOTGUN)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Pick Up the Shotgun");
		else if (lookAt == SMG)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Pick Up the SMG");
		else if (lookAt == TINDERBOX)
			GUI.Label(new Rect(10, 10, 500, 20), "Right Click to Pick Up the Tinderbox");
	}
		
}
