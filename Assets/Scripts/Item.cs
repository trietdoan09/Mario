using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject Player;

    public float bounceHeight = 0.5f; //do nay cua khoi
    public float bounceSpeed = 4f; //toc do nay
    private bool bounce = true;
    private Vector2 vector2; //vi tri khoi gach luc dau
    public float tocdonay = 0.001f;

    ///chua nam, hoa hay ngoi sao
    public bool mushroom = false;
    public bool Coin = false;
    public bool star = false;

    // hien thi so luong xu
    public int SLCoin = 1;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.tag == "ColiP" && col.contacts[0].normal.y > 0)
        {
            int i = 0;
            vector2 = transform.position;

            BB();
            GameObject KhoiRong = (GameObject)Instantiate(Resources.Load("Prefab/KhoiTrong"));
            KhoiRong.transform.position = vector2;
            if(col.collider.tag=="Player")
            {
              
                Player.GetComponent<Player>().level = 1;
                
            }
            
        }
    }


    private void BB()
    {
        if(bounce)
        {
            StartCoroutine(Blockbounce());
            bounce = false;
        }
        if(mushroom)
        {
            MushroomAndFlower();
           
        }
        if(Coin)
        {
            HCoin();
        }
    }
    

    IEnumerator Blockbounce() 
    {
        while (true)
        {
            
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounceSpeed*2 * Time.deltaTime);
            if (transform.localPosition.y >= vector2.y + bounceHeight) 
                break;
            
            yield return null;
        }
        while (true)
        {
            
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);
            if (transform.localPosition.y <= vector2.y) break;
            Destroy(gameObject);
            GameObject KhoiRong = (GameObject)Instantiate(Resources.Load("Prefab/KhoiTrong"));
            KhoiRong.transform.position = vector2;
            yield return null;
        }
    }

    private void MushroomAndFlower()
    {
        int levelMario = Player.GetComponent<Player>().level;
        GameObject Mushroom = null;
        if (levelMario == 0)
        {
            Mushroom = (GameObject)Instantiate(Resources.Load("Prefab/Mushroom"));
        }
        else if(levelMario>0)
        {
            Mushroom = (GameObject)Instantiate(Resources.Load("Prefab/Flower"));
        }
        Player.GetComponent<Player>().Audio("smb_powerup_appears");
        Mushroom.transform.SetParent(this.transform.parent);
        Mushroom.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y+1f);

       

    }

    private void HCoin()
    {
        
            GameObject coin = (GameObject)Instantiate(Resources.Load("Prefab/Coin"));
            Player.GetComponent<Player>().Audio("smb_coin");
            coin.transform.SetParent(this.transform.parent);
            coin.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 1f);
            StartCoroutine(Coinbounce());
        
    }

    IEnumerator Coinbounce()
    {
        int i = 0;
        while (i<=SLCoin)
        {
            i += 1;
            

            yield return null;
        }
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 3f * Time.deltaTime);
            if (transform.localPosition.y <= vector2.y) break;
            Destroy(gameObject);
            yield return null;
        }
    }
}
