using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colourRuinScript : MonoBehaviour
{
    private SpriteRenderer ruinSprite;
    private Color ruinColour;
    private spawnerScript spawner;
    private playerColourScript playerColour;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<spawnerScript>();
        playerColour = FindObjectOfType<playerColourScript>();

        ruinSprite = GetComponent<SpriteRenderer>();
        ruinColour = colourManagerScript.Instance.GetRandomColor();
        ruinSprite.color = ruinColour;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("collided with player!");

            playerColourScript playerColor = collision.gameObject.GetComponent<playerColourScript>();
            SpriteRenderer playerSprite = playerColor.GetComponent<SpriteRenderer>();

            Debug.Log("Comparing Ruin color: " + ruinSprite.color);
            Debug.Log("With Player sprite color: " + playerSprite.color);
          //  Debug.Log("With Player logical color (CurrentColour): " + playerColor.CurrentColour);

            if (ruinSprite.color == playerSprite.color)
            {
                Debug.Log("collided with correct colour!");

                playerColor.OnCorrectCollision();
                if (spawner != null) spawner.activeRuins.Remove(this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
