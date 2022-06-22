using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAndFlower : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if(Player.GetComponent<Player>().level==0)
            {
                Player.GetComponent<Player>().level += 1;
                Player.GetComponent<Player>().Transfiguration = true;
                Destroy(gameObject);
            }
            else if(Player.GetComponent<Player>().level > 0)
            {
                Player.GetComponent<Player>().level = 2;
                Player.GetComponent<Player>().Transfiguration = true;
                Destroy(gameObject);
            }
        }
    
        
}
        
}
