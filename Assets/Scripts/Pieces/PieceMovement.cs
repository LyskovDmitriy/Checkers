using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour 
{

	public PieceColor Color { get; set; }

	
	public virtual bool CanMove(Coordinates cellCoordinates)
	{
		if (Color == PieceColor.White)
		{
			return (CanMoveLeftUp(cellCoordinates) || CanMoveRightUp(cellCoordinates));
		}
		else
		{
			return (CanMoveLeftDown(cellCoordinates) || CanMoveRightDown(cellCoordinates));
		}
	}


	public virtual void GetPossibleMoves(List<Coordinates> moves, Coordinates cellCoordinates)
	{
		if (Color == PieceColor.White)
		{
			if (CanMoveLeftUp(cellCoordinates))
			{
				moves.Add(cellCoordinates + Coordinates.LeftUp);
			}
			if (CanMoveRightUp(cellCoordinates))
			{
				moves.Add(cellCoordinates + Coordinates.RightUp);
			}
		}
		else
		{
			if (CanMoveRightDown(cellCoordinates))
			{
				moves.Add(cellCoordinates + Coordinates.RightDown);
			}
			if (CanMoveLeftDown(cellCoordinates))
			{
				moves.Add(cellCoordinates + Coordinates.LeftDown);
			}
		}
	}


	protected bool CanMoveLeftUp(Coordinates coordinatesToMoveFrom)
	{
		return CanMoveInDirection(coordinatesToMoveFrom, Coordinates.LeftUp);
	}


	protected bool CanMoveRightUp(Coordinates coordinatesToMoveFrom)
	{
		return CanMoveInDirection(coordinatesToMoveFrom, Coordinates.RightUp);
	}


	protected bool CanMoveRightDown(Coordinates coordinatesToMoveFrom)
	{
		return CanMoveInDirection(coordinatesToMoveFrom, Coordinates.RightDown);
	}


	protected bool CanMoveLeftDown(Coordinates coordinatesToMoveFrom)
	{
		return CanMoveInDirection(coordinatesToMoveFrom, Coordinates.LeftDown);
	}


	protected bool CanMoveInDirection(Coordinates coordinatesToMoveFrom, Coordinates delta)
	{
		Coordinates possibleAvailableCoordinates = coordinatesToMoveFrom + delta;
		if (CheckBoard.Instance.IsCellWithinBoard(possibleAvailableCoordinates))
		{
			return CheckBoard.Instance.CellIsEmpty(possibleAvailableCoordinates);
		}

		return false;
	}
}
