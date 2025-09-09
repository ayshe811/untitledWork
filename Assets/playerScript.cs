using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public GameObject platform, bullet;
    Rigidbody2D rb;
    [SerializeField] float jumpforce;

    private bool isDragging = false;
    private float offsetX; // The offset between touch position and object's X position
    public float dragForce = 10f; // The strength of the force applied. Tune this!
    private float targetX; // The X we want the object to move towards
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
                Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(currentTouch.position);
                touchPosWorld.z = 0; // 2D configuration
                switch (currentTouch.phase) 
                {
                    case TouchPhase.Began:
                    //    if (IsTouchOnObject(touchPosWorld))
                    // checks if the player is touching the GO attached to this script
                        isDragging = true;                        
                        break;
                        case TouchPhase.Moved:
                        if (isDragging) targetX = touchPosWorld.x; 
                        break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                        isDragging = false;
                        break;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, 4);
        if (isDragging)
        {
            float direction = targetX - transform.position.x;
            // if the targeX value is -10 and the position of the object is 8
            // (-10) - (+8) = it will trigger a strong force to the left of 180 units (-18)
            rb.AddForce(Vector2.right * direction * dragForce);
        }
    }
    private bool IsTouchOnObject(Vector3 touchPosition)
    {
        // For 2D Physics: Check if there's a Collider2D at the touch position
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
        if (hitCollider != null && hitCollider.gameObject == gameObject) return true;  // if there is a collider attached to the GO && 
        // the collider at the touch position is the collider of the GO attached to this script, it returns a true value to the place where the method is called. 
        return false;
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
