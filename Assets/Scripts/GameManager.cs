using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Board _board;

    private const int TileCount = 2;

    private void Start()
    {
        NewGame();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        NewGame();
    }

    public void NewGame()
    {
        _board.ClearBoard();
        SpawnTiles();
    }

    private void SpawnTiles()
    {
        for(int i = 0; i < TileCount; i++)
        {
            _board.SpawnTile();
        }
    }
}
