using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mariodie : MonoBehaviour
{
    Player Player;
    private Scene scene;
    Vector2 dp;
    public float tocdonay = 0.001f;
    public float docaonay = 10.5f;
    private bool die = false;

    private bool len=true;


    void Start()
    {
        Player = GetComponent<Player>();
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {

        if (transform.localPosition.y < -3.3f && len)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + tocdonay/4 * Time.deltaTime);
            if (transform.localPosition.y >= -3.3f)
            {
                len = false;
            }
        }
        
        if (!len)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - tocdonay/3.5f * Time.deltaTime);
        }
        if(transform.localPosition.y<-10f)
        {
            Destroy(gameObject);
            Application.LoadLevel(scene.name);
        }
        
    }





}
