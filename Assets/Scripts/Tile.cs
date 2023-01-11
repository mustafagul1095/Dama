using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int _coordinates = new Vector2Int();
    public Vector2Int Coordinate => _coordinates;

    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private GameObject highlight;

    private Piece _piece;

    public Piece Piece
    {
        get => _piece;
        set => _piece = value;
    }

    private bool _playable;

    public bool Playable
    {
        get => _playable;
        set => _playable = value;
    }

    private BoxCollider _boxCollider;
    
    private void Awake()
    {
        FindCoordinates();
        _boxCollider = GetComponent<BoxCollider>();
        gameHandler.OnClickedAPiece += HandlePieceClicked;
    }

    public void FindCoordinates()
    {
        _coordinates = Vector2IntExtensions.GetCoordinateFromPosition(transform.position);
    }

    private void HandlePieceClicked()
    {
        bool available = gameHandler.Board.Matrix[_coordinates.x, _coordinates.y].Playable;
        highlight.SetActive(available);
        _boxCollider.enabled = available;
    }

    private void OnMouseDown()
    {
        gameHandler.ClearCubePlayabilityGrid();
        gameHandler.SetClickedAPiece(false);
        gameHandler.MovePiece(_coordinates.x, _coordinates.y);
        
    }
}
