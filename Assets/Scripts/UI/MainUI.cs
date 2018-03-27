using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour 
{

	public Text[] activePlayerColorTexts;


	private void Awake()
	{
		GameController.Instance.onNewTurnStart += ChangeActivePlayer;
	}


	private void ChangeActivePlayer()
	{
		for (int i = 0; i < activePlayerColorTexts.Length; i++)
		{
			activePlayerColorTexts[i].text = GameController.Instance.ColorToMove.ToString();
		}
	}


	private void OnDestroy()
	{
		GameController.Instance.onNewTurnStart -= ChangeActivePlayer;
	}
}
