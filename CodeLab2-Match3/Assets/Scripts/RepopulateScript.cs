using UnityEngine;
using System.Collections;

public sealed class RepopulateScript : MonoBehaviour 
{
	private GameManagerScript gameManager;

	public void Start () 
	{
		gameManager = GetComponent<GameManagerScript>();
	}

	public void AddNewTokensToRepopulateGrid()
	{
		//loop through each column on the grid
		for (int x = 0; x < gameManager.gridWidth; x++)
		{ 
			//add a token to empty spots at the top of the grid
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1];
			if (!token) gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid); 
		}
	}
}
