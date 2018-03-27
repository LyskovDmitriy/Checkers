using UnityEngine;

[SelectionBase]
public class BoardMember : MonoBehaviour 
{

	public Coordinates Coordinates { get { return cellCoordinates; } set { cellCoordinates = value; } }


	protected Coordinates cellCoordinates;


	private new MeshRenderer renderer;


	public void ApplyMaterial(Material cellColor)
	{
		renderer.material = cellColor;
	}


	protected virtual void Awake()
	{
		renderer = GetComponentInChildren<MeshRenderer>();
	}
}
