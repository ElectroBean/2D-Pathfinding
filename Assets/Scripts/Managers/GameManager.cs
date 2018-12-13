using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool hasGeneratedLevel = false;
    private bool hasGeneratedPathfinding = false;

    private CustomGrid customGrid;
    private LevelGeneration levelGeneration;

    public GameObject playerPrefab;
    private GameObject player;
    public GameObject enemyPrefab;

    private void Awake()
    {
        customGrid = FindObjectOfType<CustomGrid>();
        levelGeneration = FindObjectOfType<LevelGeneration>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasGeneratedLevel)
        {
            if (levelGeneration.HasGeneratedRooms())
            {
                if (!hasGeneratedPathfinding)
                {
                    customGrid.CreateGrid();
                    hasGeneratedPathfinding = true;
                    hasGeneratedLevel = true;

                    player = Instantiate(playerPrefab, levelGeneration.GetStartRoom().GetComponent<RoomType>().spawnPoints[Random.Range(0, levelGeneration.GetStartRoom().GetComponent<RoomType>().spawnPoints.Length)].transform.position, Quaternion.identity);
                    Instantiate(enemyPrefab, levelGeneration.allRooms[Random.Range(0, levelGeneration.allRooms.Count)].GetComponent<RoomType>().spawnPoints[Random.Range(0, levelGeneration.allRooms[Random.Range(0, levelGeneration.allRooms.Count)].GetComponent<RoomType>().spawnPoints.Length)].transform.position, Quaternion.identity);
                }
            }

        }
    }
}
