using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tileOptions;
    [SerializeField] private float maxRows;
    [SerializeField] private float maxColumns;
    private float tileSize;
    private Vector3 startingPosition;

    private void Start()
    {
        tileSize = tileOptions[0].transform.localScale.x;
        startingPosition = new Vector3(-maxRows * tileSize / 2, -maxColumns * tileSize / 2, 0f);

        GenerateTilemap();
    }

    private void GenerateTilemap()
    {
        int tile;
        Vector3 tilePosition;

        for (float i = 0; i < maxRows; i++)
        {
            for (float j = 0; j < maxColumns; j++)
            {
                tilePosition = new Vector3(tileSize * i, tileSize * j, 0) + startingPosition;
                if (PositionHasNoTile(tilePosition))
                {
                    //Debug.Log(i * 10 + j + 1);
                    tile = Random.Range(0, tileOptions.Length);
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
}
