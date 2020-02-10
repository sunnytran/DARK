using UnityEngine;
using System.Collections;

public class PressurePlateBehavior : MonoBehaviour
{
	
	public GameObject door;
	public string id = "";
	
	public bool triggered = false;
	bool lastTriggered = false;
	bool played = false;
	float ysp = 0.025f;
	float height = 0.3f;
	
	void Start()
	{
		
	}
	
	// block corridors
	void Update()
	{
		Ray ray = new Ray(transform.position, transform.up);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.name == "Player" || hit.transform.name == "Crate" || hit.transform.name == "SJW")
			{
				triggered = true;
				
				if (!played)
				{
					GameObject.Find("Sound").GetComponent<Sound>().triggerPlate.Play();
					played = true;
				}
				if (!door.GetComponent<DoorBehavior>().open)
					door.GetComponent<DoorBehavior>().Toggle();
			}
			else if (hit.transform.name == "Roof")
			{
				triggered = false;
				if (lastTriggered)
				{
					GameObject.Find("Sound").GetComponent<Sound>().unTriggerPlate.Play();
					door.GetComponent<DoorBehavior>().Toggle();
					played = false;
					lastTriggered = false;
				}
				if (door.GetComponent<DoorBehavior>().open)
					door.GetComponent<DoorBehavior>().Toggle();
			}
		}
		float y = transform.position.y;
		if (triggered && y > 0)
			transform.position = new Vector3(transform.position.x, y - ysp, transform.position.z);
		else if (!triggered && y < height)
			transform.position = new Vector3(transform.position.x, y + ysp, transform.position.z);
		lastTriggered = triggered;
	}
	
}
