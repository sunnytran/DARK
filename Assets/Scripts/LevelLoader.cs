using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
	
	GameObject level;
	
	public Texture2D image;
	int scale = 3;
	
	string wallHex = "000000";
	string playerHex = "00FF00";
	string torchHex = "FFFF00";
	string tinderboxHex = "9B9090";
	string keyHex = "EA9E22";
	string keyRedHex = "B50000";
	string keyGreenHex = "00B500";
	string keyBlueHex = "0000B5";
	string doorHex = "7F3300";
	string doorRedHex = "C14141";
	string doorGreenHex = "457745";
	string doorBlueHex = "4D5EBC";
	string leverPfxHex = "EA9E";
	string leverDoorPfxHex = "7F33";
	string pressurePlatePfxHex = "8001";
	string pressurePlateDoorPfxHex = "7F30";
	string unlitTorchPfxHex = "CCCC";
	string unlitTorchDoorPfxHex = "7F50";
	string crateHex = "7C4927";
	string pistolHex = "7F6A01";
	string shotgunHex = "7F6A02";
	string smgHex = "7F6A02";
				
	public GameObject playerPf;
	public GameObject pistolPf;
	public GameObject shotgunPf;
	public GameObject smgPf;
			
	public GameObject torchPf;
	public GameObject tinderboxPf;
	public GameObject keyPf;
	public GameObject doorPf;
	public GameObject leverPf;
	public GameObject pressurePlatePf;
	public GameObject cratePf;
	
	public GameObject cubePf;
	public Material wallMat;
	public Material floorMat;
	public Material roofMat;
	public Material doorMat;
	public Material puzzleDoorMat;
	public Material pressurePlateMat;
		
	List<GameObject> levers = new List<GameObject>();
	List<GameObject> leverDoors = new List<GameObject>();
	List<GameObject> pressurePlates = new List<GameObject>();
	List<GameObject> pressurePlateDoors = new List<GameObject>();
	List<GameObject> unlitTorches = new List<GameObject>();
	List<GameObject> unlitTorchDoors = new List<GameObject>();
	
	void Start()
	{
		level = new GameObject();
		level.transform.name = "Level";
	}
	
	void Update()
	{
		int width = image.width;
		int height = image.height;
		int half = scale / 2;
		print(width + " " + height);
		
		for (int x = 0; x < width; x++)
			for (int z = 0; z < height; z++)
			{
				string hex = image.GetPixel(x, z).ToHexStringRGB();
				bool north = z - 1 >= 0 && image.GetPixel(x, z - 1).ToHexStringRGB() == "000000";
				bool east = x + 1 < width && image.GetPixel(x + 1, z).ToHexStringRGB() == "000000";
				bool south = z + 1 < height && image.GetPixel(x, z + 1).ToHexStringRGB() == "000000";
				bool west = x - 1 >= 0 && image.GetPixel(x - 1, z).ToHexStringRGB() == "000000";	
				
				if (hex == playerHex)
				{
					GameObject player = (GameObject)Instantiate(playerPf, new Vector3(x * scale + half, 1.5f, z * scale), Quaternion.identity);
					player.transform.name = "Player";
				}
				else if (hex == pistolHex || hex == shotgunHex || hex == smgHex)
					SpawnGun(x, z, half, hex.Substring(4));
								
				// Objects
				if (hex == torchHex)
					SpawnTorch(x, z, width, height, half, north, east, south, west, true, "");
				else if (hex.Substring(0, 4) == unlitTorchPfxHex)
					SpawnTorch(x, z, width, height, half, north, east, south, west, false, hex.Substring(4));
				else if (hex.Substring(0, 4) == unlitTorchDoorPfxHex)
					SpawnPuzzleDoor(x, z, half, north, east, south, west, hex.Substring(4), "Torch");
				else if (hex == tinderboxHex)
					SpawnTinderbox(x, z, half);
				else if (hex == keyHex || hex == keyRedHex || hex == keyGreenHex || hex == keyBlueHex)
					SpawnKey(x, z, half, hex);
				else if (hex == doorHex || hex == doorRedHex || hex == doorGreenHex || hex == doorBlueHex)
					SpawnDoor(x, z, half, north, east, south, west, hex);
				else if (hex.Substring(0, 4) == leverPfxHex)
					SpawnLever(x, z, half, north, east, south, west, hex.Substring(4));
				else if (hex.Substring(0, 4) == leverDoorPfxHex)
					SpawnPuzzleDoor(x, z, half, north, east, south, west, hex.Substring(4), "Lever");
				else if (hex.Substring(0, 4) == pressurePlateDoorPfxHex)
					SpawnPuzzleDoor(x, z, half, north, east, south, west, hex.Substring(4), "PressurePlate");
				else if (hex == crateHex)
					SpawnCrate(x, z, half);
								
				if (hex != wallHex)
				{
					for (int i = 0; i < scale; i++)
						for (int j = 0; j < scale; j++)
						{
							if (hex.Substring(0, 4) == pressurePlatePfxHex && i == half && j == half)
							{
								GameObject plate = (GameObject)Instantiate(pressurePlatePf, new Vector3(x * scale + i, 0, z * scale + j), Quaternion.identity);
								plate.GetComponent<PressurePlateBehavior>().id = hex.Substring(4);
								plate.transform.name = "PressurePlate";
								plate.transform.parent = level.transform;
								pressurePlates.Add(plate);
							}
							else
							{
								GameObject floor = (GameObject)Instantiate(cubePf, new Vector3(x * scale + i, 0, z * scale + j), Quaternion.identity);
								floor.GetComponent<Renderer>().material = floorMat;
								floor.transform.name = "Floor";
								floor.transform.parent = level.transform;
							}
							
							GameObject roof = (GameObject)Instantiate(cubePf, new Vector3(x * scale + i, scale + 1, z * scale + j), Quaternion.identity);
							roof.GetComponent<Renderer>().material = roofMat;
							roof.transform.name = "Roof";
							roof.transform.parent = level.transform;
						}
				}
				else if (hex == wallHex)
				{
					string hexN = "";
					string hexE = "";
					string hexS = "";
					string hexW = "";
					if (z - 1 >= 0)
						hexN = image.GetPixel(x, z - 1).ToHexStringRGB();
					if (x + 1 < width)
						hexE = image.GetPixel(x + 1, z).ToHexStringRGB();
					if (z + 1 < height)
						hexS = image.GetPixel(x, z + 1).ToHexStringRGB();
					if (x - 1 >= 0)
						hexW = image.GetPixel(x - 1, z).ToHexStringRGB();
					
					if (hexN != "" && hexN != "000000" || hexE != "" && hexE != "000000" || hexS != "" && hexS != "000000" || hexW !=  "" && hexW != "000000")
					{
						for (int i = 0; i < scale; i++)
							for (int j = 1; j <= scale; j++)
								for (int k = 0; k < scale; k++)
								{
									GameObject wall = (GameObject)Instantiate(cubePf, new Vector3(x * scale + i, j, z * scale + k), Quaternion.identity);
									wall.GetComponent<Renderer>().material = wallMat;
									wall.transform.name = "Wall";
									wall.transform.parent = level.transform;
								}
					}
				}
		}
		LinkDoors();
		
		Destroy(gameObject);
	}
	
	void SpawnGun(int x, int z, int half, string val)
	{
		GameObject gun = null;
		if (val == "01")
		{
			gun = (GameObject)Instantiate(pistolPf, new Vector3(x * scale + half, 1, z * scale + half), Quaternion.identity);
			gun.transform.name = "Pistol";
		}
		else if (val == "02")
		{
			gun = (GameObject)Instantiate(shotgunPf, new Vector3(x * scale + half, 1, z * scale + half), Quaternion.identity);
			gun.transform.name = "Shotgun";
		}
		else if (val == "03")
		{
			gun = (GameObject)Instantiate(smgPf, new Vector3(x * scale + half, 1, z * scale + half), Quaternion.identity);
			gun.transform.name = "SMG";
		}
		gun.transform.parent = level.transform;
	}
	
	void SpawnTorch(int x, int z, int width, int height, int half, bool north, bool east, bool south, bool west, bool lit, string id)
	{
		if (north)
		{
			GameObject torch = (GameObject)Instantiate(torchPf, new Vector3(x * scale + half, 2.5f, z * scale - 0.4f),  Quaternion.identity);
			torch.transform.Rotate(16, 0, 0);
			if (!lit)
				torch.name = "UnlitTorch";
			else
				torch.name = "Torch";
			torch.transform.parent = level.transform;
			
			if (!lit)
			{
				torch.GetComponent<Light>().enabled = false;
				torch.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = false;
				torch.GetComponent<TorchBehavior>().lit = false;
			}
			
			torch.GetComponent<TorchBehavior>().id = id;
			if (!lit)
				unlitTorches.Add(torch);
		}
		else if (south)
		{
			GameObject torch = (GameObject)Instantiate(torchPf, new Vector3(x * scale + half, 2.5f, z * scale + 3 - 1 + 0.4f),  Quaternion.identity);
			torch.transform.Rotate(-16, 0, 0);
			if (!lit)
				torch.name = "UnlitTorch";
			else
				torch.name = "Torch";
			torch.transform.parent = level.transform;
			
			if (!lit)
			{
				torch.GetComponent<Light>().enabled = false;
				torch.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = false;
				torch.GetComponent<TorchBehavior>().lit = false;
			}
			
			torch.GetComponent<TorchBehavior>().id = id;
			if (!lit)
				unlitTorches.Add(torch);
		}
		else if (west)
		{
			GameObject torch = (GameObject)Instantiate(torchPf, new Vector3(x * scale - 0.4f, 2.5f, z * scale + half), Quaternion.identity);
			torch.transform.Rotate(0, 0, -16);
			if (!lit)
				torch.name = "UnlitTorch";
			else
				torch.name = "Torch";
			torch.transform.parent = level.transform;
			
			if (!lit)
			{
				torch.GetComponent<Light>().enabled = false;
				torch.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = false;
				torch.GetComponent<TorchBehavior>().lit = false;
			}
			
			torch.GetComponent<TorchBehavior>().id = id;
			if (!lit)
				unlitTorches.Add(torch);
		}
		else if (east)
		{
			GameObject torch = (GameObject)Instantiate(torchPf, new Vector3(x * scale + 3 - 1 + 0.4f, 2.5f, z * scale + half), Quaternion.identity);
			torch.transform.Rotate(0, 0, 16);
			if (!lit)
				torch.name = "UnlitTorch";
			else
				torch.name = "Torch";
			torch.transform.parent = level.transform;
			
			if (!lit)
			{
				torch.GetComponent<Light>().enabled = false;
				torch.transform.GetChild(0).GetComponent<ParticleSystem>().enableEmission = false;
				torch.GetComponent<TorchBehavior>().lit = false;
			}
			
			torch.GetComponent<TorchBehavior>().id = id;
			if (!lit)
				unlitTorches.Add(torch);
		}
	}
	
	void SpawnTinderbox(int x, int z, int half)
	{
		GameObject tinderbox = (GameObject)Instantiate(tinderboxPf, new Vector3(x * scale + half, 1, z * scale + half), Quaternion.identity);
		tinderbox.name = "Tinderbox";
		tinderbox.transform.parent = level.transform;
	}
	
	void SpawnKey(int x, int z, int half, string hex)
	{
		GameObject key = (GameObject)Instantiate(keyPf, new Vector3(x * scale + half, 1, z * scale + half), Quaternion.identity);
		key.name = "Key";
		key.transform.parent = level.transform;
		
		if (hex == "EA9E22")
			key.GetComponent<SpriteRenderer>().color = Color.yellow;
		else if (hex == "B50000")
			key.GetComponent<SpriteRenderer>().color = Color.red;
		else if (hex == "00B500")
			key.GetComponent<SpriteRenderer>().color = Color.green;
		else if (hex == "0000B5")
			key.GetComponent<SpriteRenderer>().color = Color.blue;
	}
	
	void SpawnDoor(int x, int z, int half, bool north, bool east, bool south, bool west, string hex)
	{
		bool spawnedDoor = false;
		GameObject door = null;
		if (north && south)
		{
			for (int i = 0; i < scale; i++)
				for (int j = 1; j <= scale; j++)
			{
				if (i == half && j <= 2)
				{
					if (!spawnedDoor)
					{
						door = (GameObject)Instantiate(doorPf, new Vector3(x * scale + half, 1.5f, z * scale + i + 0.5f), Quaternion.identity);
						door.transform.Rotate(0, 90, 0);
						door.transform.name = "Door";
						door.transform.parent = level.transform;
						spawnedDoor = true;
					}
				}
				else
				{
					GameObject wall = (GameObject)Instantiate(cubePf, new Vector3(x * scale + half, j, z * scale + i), Quaternion.identity);
					wall.GetComponent<Renderer>().material = wallMat;
					wall.transform.name = "Wall";
					wall.transform.parent = level.transform;
				}
			}
		}
		else if (east && west)
		{
			for (int i = 0; i < scale; i++)
				for (int j = 1; j <= scale; j++)
			{
				if (i == half && j <= 2)
				{
					if (!spawnedDoor)
					{
						door = (GameObject)Instantiate(doorPf, new Vector3(x * scale + half - 0.5f, 1.5f, z * scale + i), Quaternion.identity);
						door.transform.name = "Door";
						door.transform.parent = level.transform;
						spawnedDoor = true;
					}
				}
				else
				{
					GameObject wall = (GameObject)Instantiate(cubePf, new Vector3(x * scale + i, j, z * scale + half), Quaternion.identity);
					wall.GetComponent<Renderer>().material = wallMat;
					wall.transform.name = "Wall";
					wall.transform.parent = level.transform;
				}
			}
		}
		
		door.transform.GetChild(0).GetComponent<MeshRenderer>().material = doorMat;
		if (hex == doorHex)
			door.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.2f, 0, 0);
		else if (hex == doorRedHex)
			door.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
		else if (hex == doorGreenHex)
			door.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
		else if (hex == doorBlueHex)
			door.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
	}
	
	void SpawnLever(int x, int z, int half, bool north, bool east, bool south, bool west, string val)
	{
		GameObject lever = null;
		
		if (north)
		{
			lever = (GameObject)Instantiate(leverPf, new Vector3(x * scale + half, 2, z * scale - 0.4f),  Quaternion.identity);
			lever.transform.Rotate(0, 270, 0);
			lever.name = "Lever";
			lever.transform.parent = level.transform;
		}
		else if (south)
		{
			lever = (GameObject)Instantiate(leverPf, new Vector3(x * scale + half, 2, z * scale + 3 - 1 + 0.4f),  Quaternion.identity);
			lever.transform.Rotate(0, 90, 0);
			lever.name = "Lever";
			lever.transform.parent = level.transform;
		}
		else if (west)
		{
			lever = (GameObject)Instantiate(leverPf, new Vector3(x * scale - 0.4f, 2, z * scale + half), Quaternion.identity);
			lever.name = "Lever";
			lever.transform.parent = level.transform;
		}
		else if (east)
		{
			lever = (GameObject)Instantiate(leverPf, new Vector3(x * scale + 3 - 1 + 0.4f, 2, z * scale + half), Quaternion.identity);
			lever.transform.Rotate(0, 180, 0);
			lever.name = "Lever";
			lever.transform.parent = level.transform;
		}
		
		lever.GetComponent<LeverBehavior>().id = val;
		levers.Add(lever);
	}
	
	void SpawnPuzzleDoor(int x, int z, int half, bool north, bool east, bool south, bool west, string val, string type)
	{
		bool spawned = false;
		GameObject door = null;
		if (north && south)
		{
			for (int i = 0; i < scale; i++)
				for (int j = 1; j <= scale; j++)
				{
					if (i == half && j <= 2)
					{
						if (!spawned)
						{
							door = (GameObject)Instantiate(doorPf, new Vector3(x * scale + half, 1.5f, z * scale + i + 0.5f), Quaternion.identity);
							door.transform.Rotate(0, 90, 0);
							door.transform.name = "Door" + type;
							door.transform.parent = level.transform;
							spawned = true;
							door.GetComponent<DoorBehavior>().isPuzzleDoor = true;
							door.GetComponent<DoorBehavior>().id = val;
							if (type == "Lever")
								leverDoors.Add(door);
							else if (type == "PressurePlate")
								pressurePlateDoors.Add(door);
							else if (type == "Torch")
								unlitTorchDoors.Add(door);
						}
					}
					else
					{
						GameObject wall = (GameObject)Instantiate(cubePf, new Vector3(x * scale + half, j, z * scale + i), Quaternion.identity);
						wall.GetComponent<Renderer>().material = wallMat;
						wall.transform.name = "Wall";
						wall.transform.parent = level.transform;
					}
				}
		}
		else if (east && west)
		{
			for (int i = 0; i < scale; i++)
				for (int j = 1; j <= scale; j++)
				{
					if (i == half && j <= 2)
					{
						if (!spawned)
						{
							door = (GameObject)Instantiate(doorPf, new Vector3(x * scale + half - 0.5f, 1.5f, z * scale + i), Quaternion.identity);
							door.transform.name = "Door" + type;
							door.transform.parent = level.transform;
							spawned = true;
							door.GetComponent<DoorBehavior>().isPuzzleDoor = true;
							door.GetComponent<DoorBehavior>().id = val;
							if (type == "Lever")
								leverDoors.Add(door);
							else if (type == "PressurePlate")
								pressurePlateDoors.Add(door);
							else if (type == "Torch")
								unlitTorchDoors.Add(door);
						}
					}
					else
					{
						GameObject wall = (GameObject)Instantiate(cubePf, new Vector3(x * scale + i, j, z * scale + half), Quaternion.identity);
						wall.GetComponent<Renderer>().material = wallMat;
						wall.transform.name = "Wall";
						wall.transform.parent = level.transform;
					}
				}
		}
		
		door.transform.GetChild(0).GetComponent<MeshRenderer>().material = puzzleDoorMat;
	}
	
	void LinkDoors()
	{
		for (int i = 0; i < levers.Count; i++)
			for (int j = 0; j < leverDoors.Count; j++)
				if (levers[i].GetComponent<LeverBehavior>().id == leverDoors[j].GetComponent<DoorBehavior>().id)
				{
					levers[i].GetComponent<LeverBehavior>().door = leverDoors[j];
					break;
				}
		for (int i = 0; i < pressurePlates.Count; i++)
			for (int j = 0; j < pressurePlateDoors.Count; j++)
				if (pressurePlates[i].GetComponent<PressurePlateBehavior>().id == pressurePlateDoors[j].GetComponent<DoorBehavior>().id)
				{
					pressurePlates[i].GetComponent<PressurePlateBehavior>().door = pressurePlateDoors[j];
					break;
				}
		for (int i = 0; i < unlitTorchDoors.Count; i++)
			for (int j = 0; j < unlitTorches.Count; j++)
				if (unlitTorchDoors[i].GetComponent<DoorBehavior>().id == unlitTorches[j].GetComponent<TorchBehavior>().id)
					unlitTorchDoors[i].GetComponent<DoorBehavior>().torches.Add(unlitTorches[j]);
					
	}
	
	void SpawnCrate(int x, int z, int half)
	{
		GameObject crate = (GameObject)Instantiate(cratePf, new Vector3(x * scale + half, 1, z * scale + half),  Quaternion.identity);
		crate.name = "Crate";
		crate.transform.parent = level.transform;
	}
	
}
