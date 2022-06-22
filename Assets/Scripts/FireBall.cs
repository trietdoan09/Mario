using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 velocity;
    public float moveSpeed = 10f;
    private Vector2 ball;
    GameObject Ground;
    GameObject Player;
    public float tocdonay = 0.001f;
    public float docaonay = 10.5f;
    private bool len = true;
    private bool right = true;




    private void Start()
    {
        
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        velocity = rb.velocity;

    }
    private void Update()
    {
        DanBay();
        
        
    }
    private void FixedUpdate()
    {
        
    }
    private void DanBay()
    {
        Vector2 vector2 = transform.localPosition;
        GameObject Huongdan = Player.transform.Find("HuongDan").gameObject;
        if (Huongdan.transform.position.x - gameObject.transform.localPosition.x > 0)
            vector2.x -= moveSpeed * Time.deltaTime;
        else
        {
            vector2.x += moveSpeed * Time.deltaTime;
        }

        transform.localPosition = vector2;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        rb.velocity = new Vector2(velocity.x, -velocity.y);

        

        if (col.collider.tag == "Enemy" && col.collider.tag!="Player")
        {
            Destroy(col.gameObject);
            Explode();
            Destroy(gameObject);
            
        }

        if(col.collider.tag == "Ground" && transform.localPosition.y<docaonay)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + tocdonay * Time.deltaTime);
        }
        else
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - tocdonay * Time.deltaTime);
        }
        if (col.contacts[0].normal.x != 0)
        {
           
            Explode();
            Destroy(gameObject);
        }


    }



    void Explode()
    {

        GameObject banvano = (GameObject) Instantiate(Resources.Load("Prefab/No"), transform.position, Quaternion.identity);

        Destroy(gameObject,0.5f);

    }
}

