using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private GameObject highlight;

    private Piece _piece;
    private BoxCollider _boxCollider;
    
    private Vector2Int _coordiinates = new Vector2Int();

    private void Awake()
    {
        FindCoordinates();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        gameHandler.OnClickedAPiece += HandlePieceClicked;
    }

    private void FindCoordinates()
    {
        _coordiinates.x = Mathf.RoundToInt(transform.position.x / 10) + 1;
        _coordiinates.y = Mathf.RoundToInt(transform.position.z / 10) + 1;
    }

    private void HandlePieceClicked()
    {
        bool available = gameHandler.CubePlayabilityGrid[_coordiinates.x, _coordiinates.y] == 1;
        highlight.SetActive(available);
        _boxCollider.enabled = available;
    }

    private void OnMouseDown()
    {
        gameHandler.ClearCubePlayabilityGrid();
        gameHandler.SetClickedAPiece(false);
        gameHandler.MovePiece(_coordiinates.x, _coordiinates.y);
        
    }
}
