using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelunkyLevelGeneration : MonoBehaviour
{
    //Storing the different types of rooms
    public GameObject[] rooms;

    //Storing the starting positions
    public Transform[] startingPositions;

    //Storing the direction
    int direction;
    //The move amount
    public int moveAmount;

    //Timer
    float timer;
    public float timeBetweenSpawns;

    //Minimum and maximum movement
    public float minX;
    public float maxX;
    public float minY;

    //Bool to check when the generation is at the bottom
    public bool reachedBottom;

    public LayerMask room;

    int downCounter;


    // Start is called before the first frame update
    void Start()
    {
        //Getting a random starting position and spawning the first room here
        int startingPositionRdm = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[startingPositionRdm].position;
        Instantiate(rooms[0], transform.position, transform.rotation);

        //Generating a random direction
        direction = Random.Range(1, 6);

    }

    private void Update()
    {
        //Checking the timer between spawning rooms
        if(timer <= 0 && !reachedBottom)
        {
            Move();
            timer = timeBetweenSpawns;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    //Function called to move and spawn the next room
    void Move()
    {
        //Move Right
        if (direction == 1 || direction == 2)
        {
            //Checking if the current position is within the bounderes
            if (transform.position.x < maxX)
            {
                downCounter = 0;

                //Setting up the new postion
                Vector2 newPosition = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPosition;
                
                //Generating a random room and spawning it
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, transform.rotation);

                //Creating a new direction and making sure it doesnt move back in on itself
                direction = Random.Range(1, 6);
                if(direction == 3)
                {
                    direction = 1;
                }else if(direction == 4)
                {
                    direction = 5;
                }
            }
            else
                direction = 5;
            
        }//Move Left
        else if (direction == 3 || direction == 4)
        {
            //Checking if the current position is within the bounderes
            if (transform.position.x > minX)
            {
                downCounter = 0;
                //Setting up the new postion
                Vector2 newPosition = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPosition;

                //Generating a random room and spawning it
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, transform.rotation);

                //Creating a new direction and making sure it doesnt move back in on itself
                direction = Random.Range(3, 6);

            }
            else
                direction = 5;
            
        }
        else if (direction == 5)
        {
            downCounter++;

            //Checking if the current position is within the bounderes
            if (transform.position.y > minY)
            {
                //Checking if the previous room spawned has openings at the bottom
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<RoomType>().roomType != 1 && roomDetection.GetComponent<RoomType>().roomType != 3)
                {

                    if(downCounter >= 2)
                    {
                        //Destroying the previous room and making that room one with a bottom opening
                        roomDetection.GetComponent<RoomType>().DestoyRoom();

                        Instantiate(rooms[3], transform.position, transform.rotation);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().DestoyRoom();

                        int roomRnd = Random.Range(0, 4);
                        if (roomRnd == 2)
                        {
                            roomRnd = 1;
                        }
                        Instantiate(rooms[roomRnd], transform.position, transform.rotation);
                    }

                    
                }

                //Creating the new positoon
                Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPosition;

                //Generating a new random room for the current position
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, transform.rotation);

                //Generating a new direction
                direction = Random.Range(1, 6);

            }//If the generation trys to move down again then the generation is finished
            else
            {
                reachedBottom = true;
            }
               
            
        }

        
    }
}
