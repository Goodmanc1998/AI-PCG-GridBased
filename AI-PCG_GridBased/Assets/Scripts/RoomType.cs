using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    //Storing the rooms type
    public int roomType;

    //Function called to destroy the room
    public void DestoyRoom()
    {
        Destroy(gameObject);
    }
}
