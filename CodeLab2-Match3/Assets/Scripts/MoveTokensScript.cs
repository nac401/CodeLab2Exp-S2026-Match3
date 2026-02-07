using UnityEngine;
using System.Collections;

public class MoveTokensScript : MonoBehaviour {

	protected GameManagerScript gameManager;
	protected MatchManagerScript matchManager;

	public bool move = false;
	
	[SerializeField] private float lerpPercent;
	[SerializeField] private float lerpSpeed;
	bool userSwap;
	protected GameObject exchangeToken1;
	private GameObject exchangeToken2;

	private Vector2 exchangeGridPos1;
	private Vector2 exchangeGridPos2;

	public virtual void Start () 
	{
		gameManager = GetComponent<GameManagerScript>();
		matchManager = GetComponent<MatchManagerScript>();
		lerpPercent = 0;
	}
	
	public virtual void Update ()
	{
		//if not moving, stop
		if (!move) return;
		
		
		//increment the lerp, stop if over 1
		lerpPercent += lerpSpeed * Time.deltaTime;
		if (lerpPercent >= 1) lerpPercent = 1;
		
		if (exchangeToken1 != null) ExchangeTokens();
	}

	public void SetupTokenMove()
	{
		move = true;
		lerpPercent = 0;
	}

	public void SetupTokenExchange(GameObject token1, Vector2 pos1, GameObject token2, Vector2 pos2, bool reversable)
	{
		SetupTokenMove();

		exchangeToken1 = token1;
		exchangeToken2 = token2;
		
		exchangeGridPos1 = pos1;
		exchangeGridPos2 = pos2;
		
		userSwap = reversable;
	}
	public virtual void ExchangeTokens()
	{
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);
		
		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);
		
		exchangeToken1.transform.position = movePos1;
		exchangeToken2.transform.position = movePos2;
		
		if(lerpPercent >= 1)
		{
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1;
			gameManager.gridArray[(int)exchangeGridPos1.x, (int)exchangeGridPos1.y] = exchangeToken2;	
			
			if(!matchManager.GridHasMatch() && userSwap)
			{
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false);
			} 
			else 
			{
				exchangeToken1 = null;
				exchangeToken2 = null;
				move = false;
			}
		}
	}

	public virtual void MoveTokenToEmptyPos(int startGridX, int startGridY, int endGridX, int endGridY, GameObject token)
	{
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX, startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);
		
		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);
		
		token.transform.position =	pos;
		if (lerpPercent >= 1)
		{ 
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	public virtual bool MoveTokensToFillEmptySpaces()
	{
		bool movedToken = false;
		
		//cycle through all objects in grid
		for (int x = 0; x < gameManager.gridWidth; x++)
		{
			for (int y = 0; y < gameManager.gridHeight; y++)
			{
				//if the current slot is empty
				if (!gameManager.gridArray[x, y])
				{
					//check all objects above the empty slot
					for (int pos = y; pos < gameManager.gridHeight; pos++)
					{ 
						//if a token exists in this spot, move it
						GameObject token = gameManager.gridArray[x, pos];
						if (token)
						{
							MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
							movedToken = true;
						}
					}
					break; 
				}
			}
		}

		if (lerpPercent >= 1)
		{
			move = false;
			lerpPercent = 0;
		}
		
		return movedToken; 
	}
}
