using System.Collections.Generic;
using UnityEngine;

public class PieceAttack : MonoBehaviour 
{

	public PieceColor Color { get; set; }


	public virtual bool MustAttack(Coordinates cellCoordinates)
	{
		if (CheckBoard.Instance.IsAtVerticalBorder(Color, cellCoordinates.y))
		{
			return false;
		}

		return (CanAttackLeftUp(cellCoordinates) || CanAttackRightUp(cellCoordinates) 
			|| CanAttackRightDown(cellCoordinates) || CanAttackLeftDown(cellCoordinates));
	}


	public virtual void GetPossibleMoves(List<Coordinates> moves, Coordinates cellCoordinates)
	{
		if (CanAttackLeftUp(cellCoordinates))
		{
			moves.Add(cellCoordinates + Coordinates.LeftUp * 2);
		}
		if (CanAttackRightUp(cellCoordinates))
		{
			moves.Add(cellCoordinates + Coordinates.RightUp * 2);
		}
		if (CanAttackRightDown(cellCoordinates))
		{
			moves.Add(cellCoordinates + Coordinates.RightDown * 2);
		}
		if (CanAttackLeftDown(cellCoordinates))
		{
			moves.Add(cellCoordinates + Coordinates.LeftDown * 2);
		}
	}


	private bool CanAttackLeftUp(Coordinates coordinatesToAttackFrom)
	{
		return CanAttack(coordinatesToAttackFrom, Coordinates.LeftUp);
	}


	private bool CanAttackRightUp(Coordinates coordinatesToAttackFrom)
	{
		return CanAttack(coordinatesToAttackFrom, Coordinates.RightUp);
	}


	private bool CanAttackRightDown(Coordinates coordinatesToAttackFrom)
	{
		return CanAttack(coordinatesToAttackFrom, Coordinates.RightDown);
	}


	private bool CanAttackLeftDown(Coordinates coordinatesToAttackFrom)
	{
		return CanAttack(coordinatesToAttackFrom, Coordinates.LeftDown);
	}


	protected bool CanAttack(Coordinates coordinatesToAttackFrom, Coordinates delta)
	{
		Coordinates possibleEnemyCoord = coordinatesToAttackFrom + delta;
		if (CheckBoard.Instance.CellCanBeAttacked(possibleEnemyCoord))
		{
			Piece possibleEnemy = CheckBoard.Instance[possibleEnemyCoord.x, possibleEnemyCoord.y];

			//if the cell isn't empty and the piece in it doesn't belong to the same player
			if ((possibleEnemy != null) && (possibleEnemy.HoldingPlayer.Color != Color))
			{ 
				Piece pieceBehindEnemy = CheckBoard.Instance[possibleEnemy.Coordinates + delta];
				//and there is no piece behind enemy
				if (pieceBehindEnemy == null)
				{
					return true;
				}
			}
		}

		return false;
	}

}
