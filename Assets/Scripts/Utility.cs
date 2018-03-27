[System.Serializable]
public struct Coordinates
{
	public int x;
	public int y;


	public Coordinates (int X, int Y)
	{
		x = X;
		y = Y;
	}
		
	//Shorthand for writing new Coordinates(-1, 1)
	public static Coordinates LeftUp { get { return new Coordinates(-1, 1); } }

	//Shorthand for writing new Coordinates(1, 1)
	public static Coordinates RightUp { get { return new Coordinates(1, 1); } }

	//Shorthand for writing new Coordinates(1, -1)
	public static Coordinates RightDown { get { return new Coordinates(1, -1); } }

	//Shorthand for writing new Coordinates(-1, -1)
	public static Coordinates LeftDown { get { return new Coordinates(-1, -1); } }

	//Shorthand for writing new Coordinates(0, 0)
	public static Coordinates Zero { get { return new Coordinates(0, 0); } }


	public static Coordinates operator +(Coordinates first, Coordinates second)
	{
		return new Coordinates(first.x + second.x, first.y + second.y);
	}


	public static Coordinates operator *(Coordinates coord, int multiplier)
	{
		return new Coordinates(coord.x * multiplier, coord.y * multiplier);
	}


	public static bool operator ==(Coordinates first, Coordinates second)
	{
		return ((first.x == second.x) && (first.y == second.y));
	}


	public static bool operator !=(Coordinates first, Coordinates second)
	{
		return ((first.x != second.x) && (first.y != second.y));
	}


	public override string ToString()
	{
		return string.Format("Coordinates ({0}, {1})", x, y);
	}
}

public enum PieceColor { White, Black }
