using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorBehavior : MonoBehaviour
{
	
	public bool isPuzzleDoor;
	
	public bool unlocked;
	public bool open = false;
	float rot = 0;
	
	public string id = "";
	public List<GameObject> torches = new List<GameObject>();
	
	void Start()
	{
		
	}
	
	void Update()
	{
		if (torches.Count > 0 && !unlocked)
		{
			bool allLit = true;
			for (int i = 0; i < torches.Count; i++)
				if (!torches[i].transform.GetComponent<TorchBehavior>().lit)
				{
					allLit = false;
					break;
				}
			if (allLit)
				Toggle();
		}
		
		if (open && rot > -90)
		{
			transform.Rotate(0, -2.5f, 0);
			rot -= 2.5f;
		}
		else if (!open && rot < 0)
		{
			transform.Rotate(0, 2.5f, 0);
			rot += 2.5f;
		}
	}
	
	public void Toggle()
	{
		if (open && rot == -90 || !open && rot == 0)
		{
			if (!unlocked)
				unlocked = true;
			open = !open;
			
			if (open)
			{
				if (!isPuzzleDoor || torches.Count > 0)
				{
					GameObject.Find("Sound").GetComponent<Sound>().doorOpen.Play();
					GameObject.Find("Sound").GetComponent<Sound>().doorClose.Stop();
				}
			}
			else
			{
				if (!isPuzzleDoor || torches.Count > 0)
				{
					GameObject.Find("Sound").GetComponent<Sound>().doorClose.Play();
					GameObject.Find("Sound").GetComponent<Sound>().doorOpen.Stop();
				}
			}
		}
	}
		
}
