using UnityEngine;
using System;

public class GameController : MonoBehaviour 
{

	public static GameController Instance { get; private set; }

	public PieceColor ColorToMove { get; private set; }
	public bool IsGameOver { get; private set; }

	public event Action onNewTurnStart;
	public event Action onGameRestart;


	public void EndTurn()
	{
		ColorToMove = (PieceColor)( ((int)ColorToMove + 1) % 2 );
		onNewTurnStart();
	}


	public void RestartGame()
	{
		onGameRestart();
	}


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		Player.onPlayerDefeated += OnGameOver;
		onGameRestart += Start;
	}


	private void Start () 
	{
		IsGameOver = false;
		ColorToMove = PieceColor.White;
		onNewTurnStart();
	}


	private void OnGameOver(PieceColor? defeatedPlayer)
	{
		IsGameOver = true;
	}


	private void OnDestroy()
	{
		Player.onPlayerDefeated -= OnGameOver;
		onGameRestart -= Start;
	}
}
