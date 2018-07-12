using System.Collections;
using UnityEngine;

public class MoveTile : Tile, ISpecialTile
{
	private float fxDelay = 1f;
    
    [SerializeField] private GameObject moveFX;
    [SerializeField] private int moveAmount;

    protected override void Start()
    {
        base.Start();
        fxDelay = moveFX.GetComponent<ParticleSystem>().main.duration;        
    }

    public override void OnCharacterEnter(CharacterMovementController character)
    {
        base.OnCharacterEnter(character);
    }

    public IEnumerator SpecialTileBehaviour(CharacterMovementController character)
	{
        Vector3 fxPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Instantiate(moveFX, fxPosition, moveFX.transform.rotation, null);
        yield return new WaitForSeconds(fxDelay);
     			
		character.MoveLeft += moveAmount;		

        Move(character);
	}
    
	private void Move(CharacterMovementController character)
	{
		if (character.MoveLeft > 0)
			MoveForward(character);
		else if (character.MoveLeft < 0)		
			MoveBackWard(character);
		else
			Debug.LogError("Check the inspector, amount is probably set to 0");
	}

	private void MoveForward(CharacterMovementController character)
    {        
        StartCoroutine(character.Move());
	}

	private void MoveBackWard(CharacterMovementController character)
	{	
		character.TileAfterMove = TileManager.Instance.Tiles[character.TileBeforeMove.index - 1];
        StartCoroutine(character.Move());
    }
}
