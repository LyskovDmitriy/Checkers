using UnityEngine;

[SelectionBase]
public class BoardMember : MonoBehaviour 
{

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
