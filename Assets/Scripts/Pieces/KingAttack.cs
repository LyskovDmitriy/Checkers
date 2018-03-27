using System.Collections.Generic;

public class KingAttack : PieceAttack 
{
	public override bool MustAttack(Coordinates cellCoordinates)
	{
		return (CanAttackLine(cellCoordinates, Coordinates.LeftUp) || CanAttackLine(cellCoordinates, Coordinates.RightUp) 
			|| CanAttackLine(cellCoordinates, Coordinates.RightDown) || CanAttackLine(cellCoordinates,  Coordinates.LeftDown));
	}


	public override void GetPossibleMoves(List<Coordinates> moves, Coordinates cellCoordinates)
	{
		GetPossibleMovesToAttackForLine(moves, cellCoordinates, Coordinates.LeftUp);
		GetPossibleMovesToAttackForLine(moves, cellCoordinates, Coordinates.RightUp);
		GetPossibleMovesToAttackForLine(moves, cellCoordinates, Coordinates.RightDown);
		GetPossibleMovesToAttackForLine(moves, cellCoordinates, Coordinates.LeftDown);
	}


	private bool CanAttackLine(Coordinates cellCoordinates, Coordinates delta)
	{
		for (Coordinates currentCoordinates = cellCoordinates; 
			CheckBoard.Instance.IsCellWithinBoard(currentCoordinates); currentCoordinates += delta)
		{
			//king can't move past two pieces that stand in diagonal line and have no empty cell between them
			if (CheckBoard.Instance.CellCanBeAttacked(currentCoordinates + delta))
			{
				if (!CheckBoard.Instance.CellIsEmpty(currentCoordinates + delta)
					&& !CheckBoard.Instance.CellIsEmpty(currentCoordinates + delta * 2))
				{
					return false;
				}
			}

			if (CanAttack(currentCoordinates, delta))
			{
				return true;
			}
		}

		return false;
	}


	private void GetPossibleMovesToAttackForLine(List<Coordinates> moves, Coordinates cellCoordinates, Coordinates delta)
	{
		bool foundEnemy = false;
		for (Coordinates currentCoordinates = cellCoordinates; 
			CheckBoard.Instance.IsCellWithinBoard(currentCoordinates); currentCoordinates += delta)
		{
			if (!foundEnemy && CanAttack(currentCoordinates, delta))
			{
				currentCoordinates += delta * 2;
				moves.Add(currentCoordinates);
				foundEnemy = true;
			}
			//if the enemy was found all cells behind him are available until another enemy is found
			else if (foundEnemy)
			{
				if (CheckBoard.Instance.CellIsEmpty(currentCoordinates))
				{
					moves.Add(currentCoordinates);
				}
				else
				{
					return;
				}
			}
		}
	}
}
