using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Piece : BoardMember 
{
	
	public Player HoldingPlayer 
	{ 
		get { return holdingPlayer; } 
		set 
		{
			holdingPlayer = value;
			attackComponent.Color = holdingPlayer.Color;
			movementComponent.Color = holdingPlayer.Color;
		}
	}
	public float animationSpeed;


	protected Player holdingPlayer;
	protected PieceAttack attackComponent;
	protected PieceMovement movementComponent;

	private const float highlightedY = 0.6f;

	private Vector3 highlightedPosition;
	private Vector3 unhighlightedPosition;
	private bool isHighlighted;
	private bool startedMovement;


	public void HighlightPiece()
	{
		isHighlighted = true;
		ApplyMaterial(holdingPlayer.highlightedPieceMaterial);
		if (!startedMovement)
		{
			StartCoroutine(Raise());
		}
	}


	public void UnhighlightPiece()
	{
		isHighlighted = false;
		ApplyMaterial(holdingPlayer.piecesMaterial);
		if (!startedMovement)
		{
			StartCoroutine(Raise());
		}
	}


	public bool CanMove()
	{
		return movementComponent.CanMove(cellCoordinates);
	}


	public bool MustAttack()
	{
		return attackComponent.MustAttack(cellCoordinates);
	}


	public void GetPossibleMoves(List<Coordinates> moves)
	{
		if (holdingPlayer.MustAttack)
		{
			attackComponent.GetPossibleMoves(moves, cellCoordinates);
		}
		else
		{
			movementComponent.GetPossibleMoves(moves, cellCoordinates);
		}
	}


	public void Move(Coordinates newCoordinates)
	{
		CheckBoard.Instance.Move(cellCoordinates, newCoordinates);
		cellCoordinates = newCoordinates;
		if (startedMovement)
		{
			startedMovement = false;
			isHighlighted = false;
			StopAllCoroutines();
		}
		Vector3 newPosition = CheckBoard.Instance.BoardToWorldCoordinates(newCoordinates);
		newPosition.y = transform.position.y;
		transform.position = newPosition;

		if (CheckBoard.Instance.IsAtVerticalBorder(holdingPlayer.Color, cellCoordinates.y))
		{
			holdingPlayer.ReplacePieceWithKing(this);
		}
	}


	public void Remove()
	{
		holdingPlayer.RemovePiece(this);
		CheckBoard.Instance.RemoveFromBoard(cellCoordinates);
		GetComponent<PoolSignature>().ReturnToPool();
	}


	protected override void Awake()
	{
		base.Awake();
		attackComponent = GetComponent<PieceAttack>();
		movementComponent = GetComponent<PieceMovement>();
	}


	private IEnumerator Raise()
	{
		startedMovement = true;

		Vector3 currentPosition = transform.position;
		unhighlightedPosition = currentPosition;
		unhighlightedPosition.y = 0;
		highlightedPosition = unhighlightedPosition;
		highlightedPosition.y = highlightedY;

		while (true)
		{
			Vector3 targetPosition = isHighlighted ? highlightedPosition : unhighlightedPosition;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationSpeed * Time.deltaTime);
			if (transform.position == targetPosition)
			{
				break;
			}
			yield return null;
		}

		startedMovement = false;
	}
}
