using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{

	public static event Action<PieceColor?> onPlayerDefeated;

	public bool MustAttack { get; private set; }
	public PieceColor Color { get { return color; }}

	public ObjectPool piecesPool;
	public ObjectPool kingsPool;
	public Material piecesMaterial;


	private const int rowsForPieces = 3;

	[SerializeField] private PieceColor color;
	private List<Piece> pieces;
	private int startingRow;
	private int finalRow;
	private bool isReplacingPieceWithKing;


	public bool MovesInCurrentTurn()
	{
		return (color == GameController.Instance.ColorToMove);
	}


	public void ReplacePieceWithKing(Piece piece)
	{
		//to not trigger onPlayerDefeated if we remove the last piece
		isReplacingPieceWithKing = true;
		piece.Remove();
		AdjustPiece(kingsPool.GetObject(), piece.Coordinates);
		isReplacingPieceWithKing = false;
	}


	public void RemovePiece(Piece pieceToRemove)
	{
		pieces.Remove(pieceToRemove);
		if (pieces.Count <= 0 && !isReplacingPieceWithKing && !GameController.Instance.IsGameOver)
		{
			onPlayerDefeated(color);
		}
	}


	private void Awake()
	{
		AssignRowsInformation();
		pieces = new List<Piece>();

		GameController.Instance.onNewTurnStart += CheckIfMustAttack;
		onPlayerDefeated += ClearPieces;
		GameController.Instance.onGameRestart += CreatePieces;
	}


	private void OnEnable () 
	{
		CreatePieces();
	}

	//Pieces are created on cells with even coordinates
	private void CreatePieces()
	{
		for (int y = startingRow; y <= finalRow; y++)
		{
			for (int x = 0; x < CheckBoard.Instance.SIZE_X; x++)
			{
				if ((x + y) % 2 == 1)
				{
					continue;
				}
					
				AdjustPiece(piecesPool.GetObject(), new Coordinates(x, y));
			}
		}
	}


	private void AdjustPiece(GameObject pieceObject, Coordinates pieceCoordinates)
	{
		Vector3 piecePosition = CheckBoard.Instance.BoardToWorldCoordinates(pieceCoordinates);
		pieceObject.transform.position = piecePosition;
		pieceObject.transform.SetParent(transform);
		pieceObject.SetActive(true);

		Piece pieceComponent = pieceObject.GetComponent<Piece>();
		pieceComponent.Coordinates = pieceCoordinates;
		pieceComponent.ApplyMaterial(piecesMaterial);
		pieceComponent.HoldingPlayer = this;
		pieces.Add(pieceComponent);

		CheckBoard.Instance.AddPieceToBoard(pieceComponent);
	}


	private void AssignRowsInformation()
	{
		if (color == PieceColor.White)
		{
			startingRow = 0;
			finalRow = (startingRow - 1) + rowsForPieces;
		}
		else
		{
			finalRow = CheckBoard.Instance.SIZE_Y - 1;
			startingRow = (finalRow + 1) - rowsForPieces;
		}
	}


	private void CheckIfMustAttack()
	{
		if (MovesInCurrentTurn())
		{
			for (int i = 0; i < pieces.Count; i++)
			{
				//player must attack if any of the players must attack
				if (pieces[i].MustAttack())
				{
					MustAttack = true;
					return;
				}
			}
			MustAttack = false;
		}
		else
		{
			MustAttack = false;
		}

		if (pieces.Count <= 0)
		{
			return;
		}
		//check for draw when no piece can move
		for (int i = 0; i < pieces.Count; i++)
		{
			if (pieces[i].CanMove())
			{
				return;
			}
		}

		onPlayerDefeated(null);
	}


	private void ClearPieces(PieceColor? defeatedPlayer)
	{
		while (pieces.Count > 0)
		{
			pieces[0].Remove();
		}
	}


	private void OnDestroy()
	{
		GameController.Instance.onNewTurnStart -= CheckIfMustAttack;
		onPlayerDefeated -= ClearPieces;
		GameController.Instance.onGameRestart -= CreatePieces;
	}
}
