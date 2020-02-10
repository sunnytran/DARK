using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	CharacterController cc;
	public float moveSp = 5;
	float ysp = 0;
	
	public float mouseSp = 5;
	float xrot = 0;
	float xrotRange = 45;
	
	AudioSource[] footstepSounds;
	
	void Start()
	{
		cc = transform.GetComponent<CharacterController>();
		footstepSounds = new AudioSource[] {GameObject.Find("Sound").GetComponent<Sound>().footsteps1,
											GameObject.Find("Sound").GetComponent<Sound>().footsteps2,
											GameObject.Find("Sound").GetComponent<Sound>().footsteps3,
											GameObject.Find("Sound").GetComponent<Sound>().footsteps4};
	}
	
	void Update()
	{
		Move();
		Look();
	}
	
	void Move()
	{
		float xsp = Input.GetAxis("Horizontal") * moveSp;
		if (!cc.isGrounded)
			ysp += Physics.gravity.y * Time.deltaTime;
		float zsp = Input.GetAxis("Vertical") * moveSp;
		if (GetComponent<PlayerProperties>().isCarrying)
		{
			xsp /= 2.5f;
			zsp /= 2.5f;
		}
		Vector3 sp = transform.rotation * new Vector3(xsp, ysp, zsp);
		
				
		cc.Move(sp * Time.deltaTime);
		
		// Sound
		if ((Input.GetButton("Vertical") || Input.GetButton("Horizontal")) && !isPlaying())
			footstepSounds[Random.Range(0, footstepSounds.Length)].Play();
	}
	
	bool isPlaying()
	{
		for (int i = 0; i < footstepSounds.Length; i++)
			if (footstepSounds[i].isPlaying)
				return true;
		return false;
	}
	
	void Look()
	{
		float yrot = Input.GetAxis("Mouse X") * mouseSp;
		transform.Rotate(0, yrot, 0);
		
		xrot -= Input.GetAxis("Mouse Y") * mouseSp;
		xrot = Mathf.Clamp(xrot, -xrotRange, xrotRange);
		Camera.main.transform.localRotation = Quaternion.Euler(xrot, 0, 0);
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
}
