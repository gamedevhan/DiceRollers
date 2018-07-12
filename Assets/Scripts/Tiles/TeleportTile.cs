using System.Collections;
using UnityEngine;

public class TeleportTile : Tile, ISpecialTile
{
	private float startFxDelay = 0.5f;
    private float endFXDelay = 0.5f;

    [SerializeField] private GameObject startFX;
    [SerializeField] private GameObject endFX;
    [SerializeField] private Tile destinationTile;

    protected override void Start()
    {
        base.Start();
        startFxDelay = startFX.GetComponent<ParticleSystem>().main.duration;
        endFXDelay = startFX.GetComponent<ParticleSystem>().main.duration;
    }

    public override void OnCharacterEnter(CharacterMovementController character)
    {
        base.OnCharacterEnter(character);        
    }

    public IEnumerator SpecialTileBehaviour(CharacterMovementController character)
	{
        GameObject characterModel = character.transform.GetChild(0).gameObject;

        // Teleport Begins
        Vector3 fxPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        Instantiate(startFX, fxPosition, startFX.transform.rotation, null);
		yield return new WaitForSeconds(startFxDelay);
        characterModel.SetActive(false);
        
        character.transform.position = destinationTile.transform.position;

        yield return new WaitForSeconds(1f); // For cosmetic purpose. TODO: camera smooth follow

        // Update CurrentTile and NextTile
		character.TileBeforeMove = TileManager.Instance.Tiles[destinationTile.index];
		character.TileAfterMove = TileManager.Instance.Tiles[destinationTile.index + 1];

        // Telport Ends
        fxPosition = character.TileBeforeMove.transform.position;
        Instantiate(endFX, fxPosition, endFX.transform.rotation, null);
        yield return new WaitForSeconds(endFXDelay);
        characterModel.SetActive(true);
        yield return new WaitForSeconds(1f);

        GameManager.Instance.TurnManager.TurnEnd();
    }
}
