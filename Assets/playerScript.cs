using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public GameObject platform, bullet;
    Rigidbody2D rb;
    [SerializeField] float jumpforce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0;  i < Input.touchCount; i++)
            {
                Touch currentTouch = Input.GetTouch(i);
                switch (currentTouch.phase) 
                {
                    case TouchPhase.Began:
                        if (i == 0) Instantiate(bullet, transform.position, Quaternion.identity);
                        break;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            Debug.Log("New Y Velocity: " + rb.velocity.y);
        }
    }
}
