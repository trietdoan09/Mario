using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject Player;

    public float moveSpeed = 2f;
    private bool right = true;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        Vector2 vector2 = transform.localPosition;
        if (right) vector2.x += moveSpeed * Time.deltaTime;
        else vector2.x -= moveSpeed * Time.deltaTime;
        transform.localPosition = vector2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.x < 0 && collision.collider.tag!="Player")
        {
            right = false;
            doihuong();
        }
        else if(collision.contacts[0].normal.x > 0 && collision.collider.tag != "Player")
        {
            right = true;
            doihuong();
        }

        
    }

    void doihuong()
    {
        Vector2 huong = transform.localScale;
        huong.x *= -1;
        transform.localScale = huong;
    }

}
