using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject _tile;

    public GameObject[,] tiles;
    public GameObject[,] gameBoard;

    int[] lineDir = { -1, 0, 1, 0 };
    int[] colDir = { 0, 1, 0, -1 };

    private void Start()
    {
        GenerateGameBoard(6, 10);
        GameObject.Find("Hero Manager").GetComponent<HeroManager>().SpawnHeroes();
    }

    private void GenerateGameBoard(int sizeX, int sizeY)
    {
        float xPos = -1.2f;
        float yPos = 0.65f;
        float positionIncrement = Mathf.Abs(xPos) * 2 / (sizeY - 1);

        tiles = new GameObject[sizeX, sizeY];
        gameBoard = new GameObject[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            xPos = -1.2f;

            for (int j = 0; j < sizeY; j++)
            {
                tiles[i, j] = Instantiate(_tile, new Vector3(xPos, yPos, 0), Quaternion.identity, GameObject.Find("Tile Container").transform);
                tiles[i, j].GetComponent<TileScript>().SetCoords(i, j);

                xPos += positionIncrement;
            }

            yPos -= positionIncrement;
        }

    }
}
