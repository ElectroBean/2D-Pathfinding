using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public Transform[] startPositions;
    public GameObject[] rooms; //0 = LR, 1 = LRB, 2 = LRT, 3 = LRTB
    public Transform[] roomPositions;
    public Transform roomsParent;
    public List<GameObject> allRooms;

    private int direction;
    private GameObject startRoom;
    private GameObject currentRoom;
    public float moveAmount;

    public float timeBetweenRooms;
    private float timeBetweenRoomsCounter;

    public float minX, maxX, minY, maxY;

    private bool continueGenerating = true;

    public LayerMask roomMask;

    int downCounter = 0;

    private bool hasGeneratedAllRooms = false;

    // Use this for initialization
    void Start()
    {
        if (startPositions.Length > 0)
        {
            int randStartPos = Random.Range(0, startPositions.Length);
            transform.position = startPositions[randStartPos].position;

            if (rooms.Length > 0)
            {
                startRoom = Instantiate(rooms[0], transform.position, Quaternion.identity, roomsParent);
                allRooms.Add(startRoom);
                currentRoom = startRoom;
            }
        }

        direction = Random.Range(1, 4);
        timeBetweenRoomsCounter = timeBetweenRooms;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasGeneratedAllRooms)
        {
            if (timeBetweenRoomsCounter <= 0)
            {
                if (continueGenerating)
                    Move();
                else
                    SpawnNewRooms();
                timeBetweenRoomsCounter = timeBetweenRooms;
            }
            else
            {
                timeBetweenRoomsCounter -= Time.deltaTime;
            }

        }
    }

    private void Move()
    {
        switch (direction)
        {
            case 1: //down
                if (transform.position.y > minY)
                {
                    downCounter++;

                    Collider2D roomDetect = Physics2D.OverlapCircle(transform.position, 1, roomMask);
                    if (roomDetect.GetComponent<RoomType>().type != 1 && roomDetect.GetComponent<RoomType>().type != 3)
                    {
                        bool roomIsStartRoom = roomDetect.gameObject == startRoom;
                        roomDetect.GetComponent<RoomType>().DestroyRoom();
                        allRooms.Remove(roomDetect.gameObject);


                        if (downCounter >= 2)
                        {
                            currentRoom = Instantiate(rooms[3], transform.position, Quaternion.identity, roomsParent);
                            if (roomIsStartRoom)
                                startRoom = currentRoom;
                        }
                        else
                        {
                            int randomRoom = Random.Range(1, 4);
                            if (randomRoom == 2)
                            {
                                randomRoom = 1;
                            }

                            currentRoom = Instantiate(rooms[randomRoom], transform.position, Quaternion.identity, roomsParent);
                            if (roomIsStartRoom)
                                startRoom = currentRoom;
                        }
                    }
                    Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                    transform.position = newPos;

                    int randRoom = Random.Range(2, 4);
                    currentRoom = Instantiate(rooms[randRoom], transform.position, Quaternion.identity, roomsParent);

                    direction = Random.Range(1, 4);
                }
                else
                {
                    continueGenerating = false;
                    return;
                }
                break;
            case 2: //left
                if (transform.position.x > minX)
                {
                    downCounter = 0;
                    Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                    transform.position = newPos;

                    int randRoom = Random.Range(1, 4);
                    currentRoom = Instantiate(rooms[randRoom], transform.position, Quaternion.identity, roomsParent);

                    direction = Random.Range(1, 4);
                    if (direction == 3)
                        direction = 2;
                }
                else
                {
                    direction = 1;
                    return;
                }
                break;
            case 3: //right
                if (transform.position.x < maxX)
                {
                    downCounter = 0;
                    Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                    transform.position = newPos;

                    int randRoom = Random.Range(1, 4);
                    currentRoom = Instantiate(rooms[randRoom], transform.position, Quaternion.identity, roomsParent);

                    direction = Random.Range(1, 4);
                    if (direction == 2)
                        direction = 3;
                }
                else
                {
                    direction = 1;
                    return;
                }
                break;
                
        }
        allRooms.Add(currentRoom);
    }

    private void SpawnNewRooms()
    {
        foreach (Transform dot in roomPositions)
        {
            if (!Physics2D.OverlapCircle((Vector2)dot.position, 1, roomMask))
            {
                GameObject newRoom = Instantiate(rooms[Random.Range(1, 4)], dot.position, Quaternion.identity, roomsParent);
                allRooms.Add(newRoom);
            }
        }
        hasGeneratedAllRooms = true;
    }

    public bool HasGeneratedRooms()
    {
        return hasGeneratedAllRooms;
    }

    public GameObject GetStartRoom()
    {
        return startRoom;
    }
}
