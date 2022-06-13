using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    private Transform Player;
    private float minX = 0f, maxX = 183.58f;


    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        if(Player!=null)
        {
            Vector3 vector3 = transform.position;
            vector3.x = Player.position.x;
            if (vector3.x < minX)
                vector3.x = 0;
            if (vector3.x > maxX)
                vector3.x = maxX;
            transform.position = vector3;
        }
    }
}
