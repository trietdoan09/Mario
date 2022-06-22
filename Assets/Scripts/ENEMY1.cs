using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY1 : MonoBehaviour
{
    GameObject Player;
    GameObject Enemy;
    GameObject Koopas;
    GameObject MainCamera;
    private Animator animator;
    private AudioSource audio;

    public float moveSpeed=0f;
    private bool left = true;

    ///koopas die
    public bool Kick = false;
    private bool Die = false;

    //public void OnBecameVisible()
    //{
    //    moveSpeed = 2f;
    //}
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Koopas = GameObject.FindGameObjectWithTag("Enemy");
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        

        
    }
    private void Update()
    {
        animator.SetBool("Kick", Kick);
        

    }
    private void FixedUpdate()
    {
        
        Vector2 vector2 = transform.localPosition;
        if (vector2.x - MainCamera.transform.position.x < 15f)
        {
            moveSpeed = 2f;
        }
        else { moveSpeed = 0; }
        if (left) vector2.x -= moveSpeed * Time.deltaTime;
        else vector2.x += moveSpeed * Time.deltaTime;
        transform.localPosition = vector2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.x > 0 && collision.collider.tag != "Player")
        {
            left = false;
            doihuong();
        }
        else if (collision.contacts[0].normal.x < 0 && collision.collider.tag != "Player")
        {
            left = true;
            doihuong();
        }

        if (collision.collider.tag == "Player" && (collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0))
        {
            if (Player.GetComponent<Player>().level > 0)
            {
                new WaitForSeconds(5f);
                Player.GetComponent<Player>().level = 0;
                Player.GetComponent<Player>().Transfiguration = true;
                
            }
            else if(Player.GetComponent<Player>().level ==0)
            {
                new WaitForSeconds(2f);
                Player.GetComponent<Player>().MarioDie();
            }

        }
        if (collision.collider.tag == "Player" && collision.contacts[0].normal.y < 0)
        {
            Kick = true;
            moveSpeed = 0;
            Audio("smb_kick");
            if (moveSpeed <= 0)
            {
                StartCoroutine(Doixoa());
            }
            
        }
    }
    IEnumerator Doixoa()
    {
        yield return new WaitForSeconds(0.5f); 
        Destroy(gameObject);
    }
    void doihuong()
    {
        Vector2 huong = transform.localScale;
        huong.x *= -1;
        transform.localScale = huong;
    }
    
    public void Audio(string fileaudio)
    {
        audio.PlayOneShot(Resources.Load<AudioClip>("Audio/" + fileaudio));
    }
}
