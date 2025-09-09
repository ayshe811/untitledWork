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
                        if (IsTouchOnObject(touchPosWorld))
                        // checks if the player is touching the GO attached to this script
                        {
                            isDragging = true;
                            offsetX = touchPosWorld.x - transform.position.x;
                        }
                        break;
                        case TouchPhase.Moved:
                        if (isDragging) transform.position = new Vector3(touchPosWorld.x - offsetX, 
                            transform.position.y, transform.position.z);
                        break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                        isDragging = false;
                        break;
                }
            }
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
