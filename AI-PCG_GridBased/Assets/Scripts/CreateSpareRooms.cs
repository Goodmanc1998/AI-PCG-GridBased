using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpareRooms : MonoBehaviour
{
    public SpelunkyLevelGeneration levelGeneration;
    public LayerMask rooms;

    private void Update()
    {
        //Checks if a room is in the current position
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, rooms);
        if(roomDetection == null && levelGeneration.reachedBottom == true)
        {
            //If there is no room and the generation is finished then a random room is spawned
            int roomRdm = Random.Range(0, levelGeneration.rooms.Length);
            Instantiate(levelGeneration.rooms[roomRdm], transform.position, transform.rotation);
            //Destroying the spawn point
            Destroy(gameObject);
        }
    }
}
