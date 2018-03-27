using System.Collections.Generic;

public class KingMovement : PieceMovement 
{
	public override bool CanMove(Coordinates cellCoordinates)
	{
		return (CanMoveLeftUp(cellCoordinates) || CanMoveRightUp(cellCoordinates) 
			|| CanMoveLeftDown(cellCoordinates) || CanMoveRightDown(cellCoordinates));
	}


	public override void GetPossibleMoves(List<Coordinates> moves, Coordinates cellCoordinates)
	{
		GetAvailableCellsToMoveForLine(moves, cellCoordinates, Coordinates.LeftUp);
		GetAvailableCellsToMoveForLine(moves, cellCoordinates, Coordinates.RightUp);
		GetAvailableCellsToMoveForLine(moves, cellCoordinates, Coordinates.RightDown);
		GetAvailableCellsToMoveForLine(moves, cellCoordinates, Coordinates.LeftDown);
	}


	private void GetAvailableCellsToMoveForLine(List<Coordinates> moves, Coordinates cellCoordinates, Coordinates delta)
	{
		for (Coordinates currentCoordinates = cellCoordinates; 
			CheckBoard.Instance.IsCellWithinBoard(currentCoordinates); 
				currentCoordinates += delta)
		{
			if (CanMoveInDirection(currentCoordinates, delta))
			{
				moves.Add(currentCoordinates + delta);
			}
		}
	}
}
