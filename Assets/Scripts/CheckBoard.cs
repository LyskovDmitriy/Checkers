using UnityEngine;

public class CheckBoard : MonoBehaviour 
{

	public static CheckBoard Instance { get; private set; }

	public Piece this [int x, int y] { get { return board[x, y]; } }
	public Piece this [Coordinates coord] { get { return board[coord.x, coord.y]; } }

	public int SIZE_X { get { return sizeX; } }
	public int SIZE_Y { get { return sizeY; } }

	public Material beigeMaterial;
	public Material brownMaterial;
	public GameObject boardCellPrefab;


	private const int sizeX = 8;
	private const int sizeY = 8;

	private Vector3 startingPosition = new Vector3(-3.5f, 0.0f, -3.5f);
	private Piece[,] board;
	private int lastRowIndex;
	private int lastColumnIndex;


	public Coordinates? WorldToBoardCoordinates(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x - startingPosition.x);
		int y = Mathf.RoundToInt(position.z - startingPosition.z);

		if (IsCellWithinBoard(x, y))
		{
			return new Coordinates(x, y);
		}
		else
		{
			return null;
		}
	}


	public Vector3 BoardToWorldCoordinates(Coordinates coord)
	{
		return BoardToWorldCoordinates(coord.x, coord.y);
	}


	public Vector3 BoardToWorldCoordinates(int x, int y)
	{
		Vector3 cellPosition = startingPosition;
		cellPosition.x += x;
		cellPosition.z += y;
		return cellPosition;
	}


	public bool CellIsEmpty(int x, int y)
	{
		return (board[x, y] == null);
	}


	public bool CellIsEmpty(Coordinates coord)
	{
		return (board[coord.x, coord.y] == null);
	}


	public bool IsCellWithinBoard(Coordinates coord)
	{
		return IsCellWithinBoard(coord.x, coord.y);
	}


	public bool IsCellWithinBoard(int x, int y)
	{
		if ((0 <= x) && (x <= lastColumnIndex) 
			&& (0 <= y)  && (y <= lastRowIndex))
		{
			return true;
		}

		return false;
	}


	public bool CellCanBeAttacked(Coordinates possibleEnemyCoord)
	{
		if ((0 < possibleEnemyCoord.x) && (possibleEnemyCoord.x < lastColumnIndex)
			&& (0 < possibleEnemyCoord.y) && (possibleEnemyCoord.y < lastRowIndex))
		{
			return true;
		}

		return false;
	}


	public bool IsAtVerticalBorder(PieceColor playerColor, int y)
	{
		if (playerColor == PieceColor.White)
		{
			if (y == lastRowIndex)
			{
				return true;
			}
		}
		else
		{
			if (y == 0)
			{
				return true;
			}
		}

		return false;
	}


	public void HandleAttack(Coordinates startingPosition, Coordinates endPosition)
	{
		Coordinates delta = Coordinates.Zero;
		delta.x = (endPosition.x < startingPosition.x) ? -1 : 1;
		delta.y = (endPosition.y < startingPosition.y) ? -1 : 1;

		for (Coordinates currentCoordinates = startingPosition + delta;
			currentCoordinates != endPosition; currentCoordinates += delta)
		{
			Piece pieceToRemove = board[currentCoordinates.x, currentCoordinates.y];
			if (pieceToRemove != null)
			{
				pieceToRemove.Remove();
			}
		}
	}


	public void AddPieceToBoard(Piece pieceToAdd)
	{
		Coordinates pieceCoordinates = pieceToAdd.Coordinates;
		board[pieceCoordinates.x, pieceCoordinates.y] = pieceToAdd;
	}


	public void Move(Coordinates startingPosition, Coordinates endPosition)
	{
		board[endPosition.x, endPosition.y] = board[startingPosition.x, startingPosition.y];
		board[startingPosition.x, startingPosition.y] = null;
	}


	public void RemoveFromBoard(Coordinates coord)
	{
		board[coord.x, coord.y] = null;
	}


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		lastRowIndex = sizeX - 1;
		lastColumnIndex = sizeY - 1;
		CreateBoard();
	}
	

	private void CreateBoard () 
	{
		for (int x = 0; x < sizeX; x++)
		{
			for (int y = 0; y < sizeY; y++)
			{
				Vector3 cellPosition = BoardToWorldCoordinates(x, y);
				GameObject cellObject = Instantiate(boardCellPrefab, cellPosition, Quaternion.identity);
				cellObject.transform.SetParent(transform);

				BoardMember cellInfo = cellObject.GetComponent<BoardMember>();
				Material cellMaterial = ((x + y) % 2 == 0) ? beigeMaterial : brownMaterial;
				cellInfo.ApplyMaterial(cellMaterial);
			}
		}

		board = new Piece[sizeX, sizeY];
	}
}
