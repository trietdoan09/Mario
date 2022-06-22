using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    //
    private float Speed; //toc do animator cua mario
    private bool Ground = true; // mario co dung tren dat hay khong
    private float Jumping = 480; // toc do nhay cua mario
    private float JumpLow = 15f; // khi mario nhan nhay va buong khong giu
    private float Gravity = 10f; // trong luc de mario roi nhanh hon
    private bool Navigation = false;   // co dang chuyen huong khong
    ///chay nhanh va ban
    private float maxSpeed = 12f;
    private float hold = 0.2f;
    private float delay = 0.1f;
    private float press = 0;
    private float fireTime1, fireTime2;
    /// di chuyen va chuyen huong
    private Rigidbody2D rd;
    private float x;
    private float y;
    private bool moveRight;
    private float moveSpeed = 7f; //toc do di chuyen cua mario
    // chuyen animator tu nho sang lon
    public int level = 0; //cap do cua mario
    public bool Transfiguration = false;// xem xet bien hinh cua mario nam hay hoa
    private Animator animator;
    ///Am thanh
    [SerializeField] private AudioClip hightJump;
    private AudioSource audio;
    //chet
    private Vector2 dp;
    //
    private Vector2 vitriban;
    //respawn;
    private int reSpawn;
    public bool reset = false;
    void Start()
    {
        rd = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Speed); // gia tri "Speed" duoc lay tu animator gan vao cho Speed  
        animator.SetBool("Ground", Ground);
        animator.SetBool("Navigation", Navigation);
        Jump();
        fireTime1 = 0.5f; /// thoi gian coldown moi qua cau se tang khi gia tri nay tang len
        fireTime2 = 0f;
        FireandsPrint();
        if(Transfiguration==true)
        {
            switch(level)
            {
                case 0:
                    {
                        StartCoroutine(SRMarioToMario());
                        Audio("smb_pipe");
                        Transfiguration = false;
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(MarioToSRMario());
                        Audio("smb_powerup");
                        Transfiguration = false;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(SRMarioToSSRMario());
                        Audio("smb_powerup");
                        Transfiguration = false;
                        break;
                    }
                
                default:Transfiguration=false; break;
            }
        }
        if (gameObject.transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    void Move() 
    {
        // sử dụng horizontal để di chuyển phải trái trong axis trong project setting
        float move = Input.GetAxis("Horizontal"); // horizontal : di chuyen ve ben phai x duong, di chuyen ve ben trai x am
        rd.velocity = new Vector2(moveSpeed * move, rd.velocity.y);
        Speed = Mathf.Abs(moveSpeed * move); // lay gia tri tuyet doi cua speed de value khong bi am vi dk trong animator set lon hon 0
        if (move > 0 && moveRight) //dk move phai di chuyen ve truc duong va no co phai moveright khong neu thoa thi di chuyen binh thuong
            LeftOrRight();
        else if (move < 0 && !moveRight)// dk move di chuyen ve truc am va no khong phai moveRight nen phai xoay chieu animation lai roi moi di chuyen
            LeftOrRight();            
    }

    void LeftOrRight()
    {
        moveRight = !moveRight; //neu moveRight khong phai la moveright thi set scale =-1
        Vector2 vector2 = transform.localScale;
        vector2.x *= -1;
        transform.localScale = vector2;
        if (moveSpeed >= 7)
            StartCoroutine(navigation());
    }
    IEnumerator navigation()// chuyen huong mario khi dang chay phai ma an trai
    {
        Navigation = true;
        yield return new WaitForSeconds(0.2f); // true tra ve false khi doi 0.2 giay
        Navigation = false;
    }

    public void MarioDie()
    {
        dp = transform.localPosition;
        GameObject MarioDie = (GameObject)Instantiate(Resources.Load("Prefab/MarioDie"));
        MarioDie.transform.localPosition = dp;
        Destroy(gameObject);
        reset = true;
    }

    private void HightJump()
    {
        audio.PlayOneShot(hightJump);
    }
    private void Jump()
    {
        if(Input.GetKey(KeyCode.UpArrow)/* || Input.GetKey(KeyCode.Space) */&& Ground == true)
        {
            HightJump();
            rd.AddForce((Vector2.up) * Jumping); //addForce la them mot luc vao bang voi luc cua jumping nhan huong len tren cua vector2.up
            Ground = false; // trong luc nay ground la false de co the hien duoc animator jump
        }

        // luc hut cho mario roi nhanh hon
        if(rd.velocity.y<0)
        {
            
            rd.velocity += Vector2.up * Physics2D.gravity.y * (Gravity /*- 1*/) * Time.deltaTime; // velocity bang luc huong len cua vector2 nhan voi luc hut theo y va nhan voi luc hut roi xuong tinh theo deltatime
            
        }
        else if(rd.velocity.y>0 && !Input.GetKey(KeyCode.UpArrow)) //neu an va tha thi mario se khogn nhay cao duoc 
        {
            //SmallJump();
            rd.velocity += Vector2.up * Physics2D.gravity.y * (JumpLow - 1) * Time.deltaTime;  
        }
    }
    private void OnTriggerEnter2D(Collider2D col) //ontriggerenter2d khi mot collider khac va cham voi collider nay
    {
        if (col.tag == "Ground")// su dung tag de biet vat the phia tren hoac phia duoi co phai la ground khong
        {
            Ground = true;
        }

    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            Ground = true;
        }
    }
    void FireandsPrint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            if (level == 2)
            {
                fireTime2 += Time.time;
                if (fireTime2 - fireTime1 >= delay)
                {
                    
                    fireTime1 = Time.time;
                    GameObject MarioFire = null;
                    MarioFire = (GameObject)Instantiate(Resources.Load("Prefab/Fire"));
                    MarioFire.transform.SetParent(this.transform.parent);
                    GameObject Huongdan = null;
                    Huongdan = gameObject.transform.Find("HuongDan").gameObject;
                    if (Huongdan.transform.position.x-gameObject.transform.localPosition.x>0)
                        MarioFire.transform.localPosition = new Vector2(transform.localPosition.x + 1f, transform.localPosition.y + 1f);
                    else
                        MarioFire.transform.localPosition = new Vector2(transform.localPosition.x - 1f, transform.localPosition.y + 1f);

                    delay = Time.time;
                }
                
            }
            
        }
    }

  

    IEnumerator MarioToSRMario() // mario an nam
    {
        float delay = 0.1f; // set do tre la 0.1 giay
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0); // set layer weight la 0 de no khong hien thi animator nay
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1); // vi la an nam nen set weight bang 1 de hien thi animator nay
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);

        // de cho mario nhap nhay khi bien hinh tu nho sang lon thi ta lap lai nhung dong tren 
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1); 
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0); 
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0); 
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1); 
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1); 
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1); 
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
    }
    IEnumerator SRMarioToMario() // mario an nam
    {
        float delay = 0.1f; // set do tre la 0.1 giay
        // de cho mario nhap nhay khi bien hinh tu nho sang lon thi ta lap lai nhung dong tren 
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
    }
    IEnumerator SRMarioToSSRMario() // mario an hoa
    {
        float delay = 0.1f; // set do tre la 0.1 giay
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0); // set layer weight la 0 de no khong hien thi animator nay
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0); 
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1); // vi la an hoa nen set weight bang 1 de hien thi animator nay
        yield return new WaitForSeconds(delay);

        // de cho mario nhap nhay khi bien hinh ta lap lai nhung dong tren 
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1);
        yield return new WaitForSeconds(delay);
    }
    IEnumerator SSRMariotoSRMario() // mario an hoa
    {
        float delay = 0.1f; // set do tre la 0.1 giay
        // de cho mario nhap nhay khi bien hinh ta lap lai nhung dong tren 
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("Mario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("SRMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("SSRMario"), 0);
        yield return new WaitForSeconds(delay);
    }
    public void Audio(string fileaudio)
    {
        audio.PlayOneShot(Resources.Load<AudioClip>("Audio/" + fileaudio));
    }
}
