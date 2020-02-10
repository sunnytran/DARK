using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProperties : MonoBehaviour
{
	
	public List<Color> keys;
	public bool isCarrying = false;	
	public bool hasTinderbox = false;
	
	void Start()
	{
		keys = new List<Color>();
	}
	
	void Update()
	{
		
	}
	
}
