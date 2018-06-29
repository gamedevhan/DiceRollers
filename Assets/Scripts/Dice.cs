using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{	
	public static int DiceResult; // Why is this static?

	[SerializeField] private Vector3 forceMin;
	[SerializeField] private Vector3 forceMax;	
	[SerializeField] private Vector3 offsetFromCamera;

	private MeshRenderer meshRenderer;
	private Rigidbody rigidBody;
	private Camera mainCamera;

	// This event needs to be raised when dice is rolled
	public delegate void DiceRollEvent();
	public static event DiceRollEvent DiceRolled;
	
	public static Dice Instance = null;

	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{			
		mainCamera = Camera.main;		
		meshRenderer = GetComponent<MeshRenderer>();
		rigidBody = GetComponent<Rigidbody>();

		transform.position = mainCamera.transform.position + offsetFromCamera;
		meshRenderer.enabled = false;
		rigidBody.isKinematic = true;		
	}
	
	public IEnumerator Roll()
	{		
		rigidBody.isKinematic = false;
		meshRenderer.enabled = true;

		// Get random value
		float randomX = (int)Random.Range(forceMin.x, forceMax.x);
		float randomY = (int)Random.Range(forceMin.y, forceMax.y);
		float randomZ = (int)Random.Range(forceMin.z, forceMax.z);
		float randomTorqueUp = Random.Range(-10000 , 10000);
		float randomTorqueRight = Random.Range(-10000 , 10000);

		// Apply Force and Torque
		rigidBody.AddForce(randomX, randomY, randomZ);
		rigidBody.AddTorque(transform.up * randomTorqueUp);
		rigidBody.AddTorque(transform.right * randomTorqueRight);
				
		while (!rigidBody.IsSleeping())
		{
			yield return null;
		}
		
		GetDiceResult();

		// If there is any subscriber, publish event
		if (DiceRolled != null)
			DiceRolled();

		Debug.Log("<b><color=green>Rolling a die! Result:</color> " + DiceResult + "</b>");

		// Reset Dice when character stops moving
		StartCoroutine(ResetDice());
	}

	private void GetDiceResult()
	{		
		if (Vector3.Dot (transform.forward, Vector3.up) > 0.6f)
				DiceResult = 2;
		if (Vector3.Dot (-transform.forward, Vector3.up) > 0.6f)
				DiceResult = 5;
		if (Vector3.Dot (transform.up, Vector3.up) > 0.6f)
				DiceResult = 3;
		if (Vector3.Dot (-transform.up, Vector3.up) > 0.6f)
				DiceResult = 4;
		if (Vector3.Dot (transform.right, Vector3.up) > 0.6f)
				DiceResult = 6;
		if (Vector3.Dot (-transform.right, Vector3.up) > 0.6f)
				DiceResult = 1;		
	}
	
	private IEnumerator ResetDice()
	{
		CharacterMovement characterMovement = FindObjectOfType<CharacterMovement>();
		while (characterMovement.IsMoving)
		{
			yield return null;
		}

		// TODO: Dice disappearing VFX and SFX at current position

		rigidBody.isKinematic = true;
		meshRenderer.enabled = false;		
		transform.position = mainCamera.transform.position + offsetFromCamera;
		transform.rotation = Quaternion.identity;
	}
}
