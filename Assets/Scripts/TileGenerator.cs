//References:
//Use of Perlin Noise: https://www.youtube.com/watch?v=DBjd7NHMgOE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tileOptions;
    [SerializeField] private int maxRows;
    [SerializeField] private int maxColumns;
    private float tileSize;
    float[,] noiseMap;
    private Vector3 startingPosition;

    private float PERLIN_SCALE = .1f;

    private void Start()
    {
        tileSize = tileOptions[0].transform.localScale.x;
        float startingXPosition = (-maxRows * tileSize / 2) - (-maxRows * tileSize / 2) % tileSize;
        float startingYPosition = (-maxColumns * tileSize / 2) - (-maxColumns * tileSize / 2) % tileSize;
        startingPosition = new Vector3(startingXPosition, startingYPosition, 0f);

        noiseMap = InitializeNoiseMap();

        GenerateTilemap();
    }

    private void GenerateTilemap()
    {
        int tile;
        Vector3 tilePosition;

        for (int y = 0; y < maxRows; y++)
        {
            for (int x = 0; x < maxColumns; x++)
            {
                tilePosition = new Vector3(tileSize * y, tileSize * x, 0) + startingPosition;
                if (PositionHasNoTile(tilePosition))
                {
                    //Debug.Log(y * 10 + x + 1);
                    tile = (int)noiseMap[x, y];
                    if (tile < 0) tile = 0;
                    else if (tile == tileOptions.Length) tile = tileOptions.Length - 1;
                    AddTile(tilePosition, tileOptions[tile]);
                }
            }
        }
    }

    private void AddTile(Vector3 position, GameObject tile)
    {
        Transform tileT = Instantiate(tile).transform;
        tileT.position = position;
    }

    private bool PositionHasNoTile(Vector3 position)
    {
        Vector2 raycastDirection = new Vector2(1, 1);
        Vector3 RAYCAST_OFFSET = new Vector3(.01f, .01f, 0f);
        RaycastHit2D hit = Physics2D.Raycast(position + RAYCAST_OFFSET, raycastDirection, tileSize / 10f);

        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }

    private float[,] InitializeNoiseMap()
    {
        float[,] initialNoiseMap = new float[maxColumns, maxRows];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for(int y = 0; y < maxRows; y++)
        {
            for(int x = 0; x < maxColumns; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * PERLIN_SCALE + xOffset, y * PERLIN_SCALE + yOffset);
                initialNoiseMap[x,y] = noiseValue * tileOptions.Length;
            }
        }

        return initialNoiseMap;
    }
}
