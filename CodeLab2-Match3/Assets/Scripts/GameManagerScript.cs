using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	protected MatchManagerScript matchManager;
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;
	protected TokenObjectPool tokenObjectPool;

	public GameObject grid;
	public GameObject[,] gridArray;
	GameObject selected;
	
	
	//Initialize
	public virtual void Start () {
		gridArray = new GameObject[gridWidth, gridHeight];
		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		tokenObjectPool = GetComponent<TokenObjectPool>();
		moveTokenManager = GetComponent<MoveTokensScript>();
		MakeGrid();
	}

	public virtual void Update()
	{
		if(!GridHasEmpty()){
			if(matchManager.GridHasMatch()){
				RemoveAllMatchTokens(matchManager.GetAllMatchTokens());
			} 
			else {
				inputManager.SelectToken();
			}
		} 
		else 
		{
			
			if(!moveTokenManager.move) moveTokenManager.SetupTokenMove();
			
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()) repopulateManager.AddNewTokensToRepopulateGrid();
		} 
	}
	
	void MakeGrid() 
	{
		grid = new GameObject("TokenGrid"); 
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}
	
	public virtual void RemoveAllMatchTokens(List<GameObject> removeTokens)
	{
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(removeTokens.Contains(gridArray[x, y])){
					tokenObjectPool.RemoveToken(gridArray[x,y]);
					gridArray[x, y] = null;
				}
			}
		}
	}

	public virtual bool GridHasEmpty()
	{
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == null){
					return true;
				}
			}
		}

		return false;
	}

	public Vector2 GetPositionOfTokenInGrid(GameObject token)
	{
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == token){
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}
		
	public Vector2 GetWorldPositionFromGridPosition(int x, int y)
	{
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}

	public void AddTokenToPosInGrid(int x, int y, GameObject parent)
	{
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = tokenObjectPool.GetToken(position);
			// Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
			//             position, 
			//             Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}
}
