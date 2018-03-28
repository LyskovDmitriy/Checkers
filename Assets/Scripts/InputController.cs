using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour 
{

	private Camera mainCamera;
	private Piece highlightedPiece;
	private List<Coordinates> possibleMoves;
	private bool pieceIsInAttackSequence;


	private void Awake()
	{
		mainCamera = Camera.main;
		possibleMoves = new List<Coordinates>();
		#if UNITY_ANDROID
		Input.multiTouchEnabled = false;
		Input.simulateMouseWithTouches = false;
		#endif
	}


	private void Update () 
	{
		#if UNITY_STANDALONE || UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			HandleInput(Input.mousePosition);
		}
		#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			HandleInput(Input.GetTouch(0).position);
		}
		#endif
	}


	private void HandleInput(Vector3 inputPosition)
	{
		Coordinates? coord = GetCoordinatesAtScreenInput(inputPosition);

		if (coord.HasValue)
		{
			Coordinates inputCoordinates = coord.Value;
			Piece selectedPiece = CheckBoard.Instance[inputCoordinates];

			if (selectedPiece != null)
			{
				TryHighlightPiece(selectedPiece);
			}
			else
			{
				Debug.Log("Click");
				if (possibleMoves.Contains(inputCoordinates))
				{
					Debug.Log("Possible Move");
					MoveOrAttack(inputCoordinates);
				}
			}
		}
		else
		{
			if (!pieceIsInAttackSequence)
			{
				UnhighlightCurrentPiece();
			}
		}
	}


	private void TryHighlightPiece(Piece selectedPiece)
	{
		bool pieceIsNotHighlighted = (selectedPiece != highlightedPiece);
		bool belongsToActivePlayer = selectedPiece.HoldingPlayer.MovesInCurrentTurn();
		bool playerMustAttack = selectedPiece.HoldingPlayer.MustAttack;

		if (pieceIsNotHighlighted
			&& belongsToActivePlayer 
				&& (((playerMustAttack && selectedPiece.MustAttack())) 
					|| (!playerMustAttack && selectedPiece.CanMove())))
		{
			HighlightPiece(selectedPiece);
		}
	}


	private void MoveOrAttack(Coordinates endCoordinates)
	{
		pieceIsInAttackSequence = false;
		bool pieceHasAttackedThisTurn = false;

		if (highlightedPiece.HoldingPlayer.MustAttack)
		{
			pieceHasAttackedThisTurn = true;
			CheckBoard.Instance.HandleAttack(highlightedPiece.Coordinates, endCoordinates);
		}

		highlightedPiece.Move(endCoordinates);
		possibleMoves.Clear();
		//if highlighted piece can attack again, enter attack sequence 
		if (pieceHasAttackedThisTurn && highlightedPiece.MustAttack())
		{
			CalculatePossibleMoves();
			pieceIsInAttackSequence = true;
		}
		else
		{
			highlightedPiece.UnhighlightPiece();
			highlightedPiece = null;
			GameController.Instance.EndTurn();
		}
	}


	private void UnhighlightCurrentPiece()
	{
		if (highlightedPiece != null)
		{
			pieceIsInAttackSequence = false;
			possibleMoves.Clear();
			highlightedPiece.UnhighlightPiece();
			highlightedPiece = null;
		}
	}


	private void HighlightPiece(Piece pieceToHighlight)
	{
		if (pieceIsInAttackSequence)
		{
			return;
		}

		UnhighlightCurrentPiece();
		highlightedPiece = pieceToHighlight;
		highlightedPiece.HighlightPiece();
		CalculatePossibleMoves();
	}


	private void CalculatePossibleMoves()
	{
		highlightedPiece.GetPossibleMoves(possibleMoves);
	}


	private Coordinates? GetCoordinatesAtScreenInput(Vector3 screenInputPosition)
	{
		Vector3 screenPosition = screenInputPosition;
		screenPosition.z = mainCamera.transform.position.y;
		Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
		return CheckBoard.Instance.WorldToBoardCoordinates(worldPosition);
	}
}
