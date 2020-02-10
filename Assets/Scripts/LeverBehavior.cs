using UnityEngine;
using System.Collections;

public class LeverBehavior : MonoBehaviour
{
	
	public GameObject door;
	public string id = "";
	
	bool flipped;
	float rot = 0;
	
	void Start()
	{
		
	}
	
	void Update()
	{
		if (flipped && rot > -90)
		{
			rot -= 2.5f;
			transform.GetChild(1).transform.Rotate(0, 0, -2.5f);
		}
		else if (!flipped && rot < 0)
		{
			rot += 2.5f;
			transform.GetChild(1).transform.Rotate(0, 0, 2.5f);
		}
	}
	
	public void Toggle()
	{
		if (flipped && rot == -90 || !flipped && rot == 0)
		{
			flipped = !flipped;
			door.GetComponent<DoorBehavior>().Toggle();
			GameObject.Find("Sound").GetComponent<Sound>().flipLever.Play();
		}
	}
	
}
