using UnityEngine;
using System.Collections.Generic;

public class MatchManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager;

	public virtual void Start () 
	{
		gameManager = GetComponent<GameManagerScript>();
	}

	#region Match Testing

	public virtual bool GridHasMatch(){
		
		bool match = false;
		
		//cycle through all the objects in the grid
		for (int x = 0; x < gameManager.gridWidth; x++){
			for (int y = 0; y < gameManager.gridHeight ; y++){
				
				//test for a minimum of 3 match, horizontally
				if (x < gameManager.gridWidth - 2)
				{
					match = match || GridHasHorizontalMatch(x, y);
				}

				//test for a minimum of 3 match, vertically
				if (y < gameManager.gridHeight - 2)
				{
					match = match || GridHasVerticalMatch(x, y);
				}
			}
		}
		return match;
	}

	protected bool GridHasHorizontalMatch(int x, int y)
	{
		GameObject token1 = gameManager.gridArray[x + 0, y];
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
		if (token1 && token2 && token3)
		{
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		}
		
		return false;
	}

	private bool GridHasVerticalMatch(int x, int y)
	{
		GameObject token1 = gameManager.gridArray[x, y + 0];
		GameObject token2 = gameManager.gridArray[x, y + 1];
		GameObject token3 = gameManager.gridArray[x, y + 2];
		
		if (token1&& token2&& token3)
		{
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		}
		
		return false;
	}

	#endregion

	#region Getting Match Length

	protected int GetHorizontalMatchLength(int x, int y)
	{
		//initialize base length and first game object in match
		int matchLength = 1;
		GameObject first = gameManager.gridArray[x, y];

		//return base length if first is null
		if (!first) return matchLength;
			
		//get sprite (how we are telling if objects are matching)
		SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
		
		//check 
		for(int i = x + 1; i < gameManager.gridWidth; i++){
			
			GameObject other = gameManager.gridArray[i, y];
			if(other != null){
				SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();
				if(sr1.sprite == sr2.sprite){
					matchLength++;
				} else {
					break;
				}
			} else {
				break;
			}
		}
		return matchLength;
	}

	private int GetVerticalMatchLength(int x, int y)
	{
		//initialize base length and first game object in match
		int matchLength = 1;
		GameObject first = gameManager.gridArray[x, y];

		//return base length if first is null
		if (!first) return matchLength;
			
		//get sprite (how we are telling if objects are matching)
		SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
		
		//check 
		for(int i = y + 1; i < gameManager.gridWidth; i++){
			
			GameObject other = gameManager.gridArray[x, i];
			if(other != null){
				SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();
				if(sr1.sprite == sr2.sprite){
					matchLength++;
				} else {
					break;
				}
			} else {
				break;
			}
		}
		return matchLength;
	}

	#endregion

	#region Getting All Matches

	public virtual List<GameObject> GetAllMatchTokens()
	{
		List<GameObject> tokensToRemove = new List<GameObject>();

		//go through every object in the grid
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			for(int y = 0; y < gameManager.gridHeight ; y++)
			{
				//check if there is enough room to the right of object
				//for a match to occur
				if(x < gameManager.gridWidth - 2)
				{
					//get all horizontal matches by testing their length
					int hMatchLength = GetHorizontalMatchLength(x, y);
					
					if(hMatchLength > 2)
					{
						//prime all tokens in match for removal
						for(int i = x; i < x + hMatchLength; i++){
							GameObject token = gameManager.gridArray[i, y]; 
							tokensToRemove.Add(token);
						}
					}
				}
				
				//check if there is enough room below the object
				//for a match to occur
				if(y < gameManager.gridHeight - 2)
				{
					//get all vertical matches by testing their length
					int vMatchLength = GetVerticalMatchLength(x, y);
					
					if(vMatchLength > 2)
					{
						//prime all tokens in match for removal
						for(int i = y; i < y + vMatchLength; i++){
							GameObject token = gameManager.gridArray[x, i]; 
							tokensToRemove.Add(token);
						}
					}
				}
			}
		}
		return tokensToRemove;
	}

	#endregion
}
