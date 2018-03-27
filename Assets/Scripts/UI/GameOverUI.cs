using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour 
{

	public GameObject gameOverScreen;
	public Text winnerText;


	public void Restart()
	{
		GameController.Instance.RestartGame();
		gameOverScreen.SetActive(false);
	}


	private void Awake () 
	{
		Player.onPlayerDefeated += GameOver;
	}


	private void GameOver(PieceColor? defeatedPlayer)
	{
		gameOverScreen.SetActive(true);
		if (defeatedPlayer.HasValue)
		{
			PieceColor winner = (PieceColor)(((int)defeatedPlayer.Value + 1) % 2);
			winnerText.text = winner.ToString() + " player won";
		}
		else
		{
			winnerText.text = "Draw";
		}
	}


	private void OnDestroy () 
	{
		Player.onPlayerDefeated -= GameOver;
	}
}
