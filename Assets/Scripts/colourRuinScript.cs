using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colourRuinScript : MonoBehaviour
{
    private SpriteRenderer ruinSprite;
    private Color ruinColour;
    private spawnerScript spawner;
    private playerColourScript playerColour;
    public GameObject trigger;
    Vector2 triggerPosition;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<spawnerScript>();
        playerColour = FindObjectOfType<playerColourScript>();

        ruinSprite = GetComponent<SpriteRenderer>();
        ruinColour = colourManagerScript.Instance.GetRandomColor();
        ruinSprite.color = ruinColour;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerColourScript playerColor = collision.gameObject.GetComponent<playerColourScript>();
            SpriteRenderer playerSprite = playerColor.GetComponent<SpriteRenderer>();
            abilityManagerScript playerAbility = FindObjectOfType<abilityManagerScript>();

            if (ruinSprite.color == playerSprite.color)
            {
                Debug.Log("collided with correct colour!");

                playerColor.OnCorrectCollision();
                if (spawner != null) spawner.activeRuins.Remove(this.gameObject);
                Destroy(gameObject);
            }
            else playerAbility.currentCharge = 0;
            Destroy(gameObject);
        }
    }
}
