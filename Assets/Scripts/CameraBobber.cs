using UnityEngine;
using System.Collections;

public class CameraBobber : MonoBehaviour
{
	
	float timer = 0.0f; 
	float bobSp = 0.1f; 
	float bobAmt = 0.25f; 
	float midpoint = 0.5f; 
	
	void Start()
	{
	
	}
	
	void Update()
	{
		Vector3 localPos = transform.localPosition;
		float x = localPos.x;
		float z = localPos.z;
				
		float waveslice = 0.0f; 
		float horizontal = Input.GetAxis("Horizontal"); 
		float vertical = Input.GetAxis("Vertical"); 
		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
			timer = 0.0f; 
		else
		{ 
			waveslice = Mathf.Sin(timer); 
			timer += bobSp; 
			if (timer > Mathf.PI * 2) 
				timer -= Mathf.PI * 2; 
		} 
		if (waveslice != 0)
		{ 
			float translateChange = waveslice * bobAmt; 
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical); 
			totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f); 
			translateChange = totalAxes * translateChange;
			transform.localPosition = new Vector3(x, midpoint + translateChange, z);
		} 
		else
			transform.localPosition = new Vector3(x, midpoint, z);
	}
	
}
