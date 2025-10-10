using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public GameObject platform, bullet;
    Rigidbody2D rb;

    private bool isDragging = false;
    [SerializeField] float maxX, minX; // The offset between touch position and object's X position
    [SerializeField] float dragForce, upwardForce;
    private float targetX;
    private SpriteRenderer playerSprite;

    TrailRenderer trailRenderer;    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerSprite = GetComponent<SpriteRenderer>();
        trailRenderer.emitting = false;
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
                        isDragging = true;
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (isDragging) targetX = touchPosWorld.x;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        isDragging = false;
                        break;
                }
            }
        }

        if (isDragging && rb.velocity.y > .1f) trailRenderer.emitting = true;
        else trailRenderer.emitting = false;

        trailRenderer.startColor = playerSprite.color;
    }
    private void FixedUpdate()
    {
        if (isDragging)
        {
            float direction = targetX - transform.position.x; // calculates the distance between the targetX and the player's X. 

            // if the targeX value is -10 and the position of the object is 8
            // (-10) - (+8) = it will trigger a strong force to the left of 180 units (-18)

            bool canMoveRight = (direction > 0 && transform.position.x < maxX);
            bool canMoveLeft = (direction < 0 && transform.position.x > minX);
          
            if (canMoveLeft || canMoveRight) rb.AddForce(Vector2.right * direction * dragForce);
            else rb.velocity = new Vector2(0, rb.velocity.y); // velocity in this context is a handbrake; instant command to stop immediately. 

            rb.velocity = new Vector2(rb.velocity.x, upwardForce);
        }
    }
}
