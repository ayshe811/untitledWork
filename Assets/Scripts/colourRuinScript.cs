using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colourRuinScript : MonoBehaviour
{
    private SpriteRenderer ruinSprite, playerSprite;
    private spawnerScript spawner;
    private playerColourScript playerColour;
    private abilityManagerScript abilityScript;

    public GameObject trigger;
    private GameObject player;
    Vector2 triggerPosition;

    // Start is called before the first frame update
    void Start()
    {
        abilityScript = FindObjectOfType<abilityManagerScript>();
        spawner = FindObjectOfType<spawnerScript>();
        playerColour = FindObjectOfType<playerColourScript>();

        ruinSprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("player");
        playerSprite = player.GetComponent<SpriteRenderer>();

        if (!abilityScript.universalColor.isActive) ruinSprite.color = colourManagerScript.Instance.GetRandomColor();
        else ruinSprite.color = playerSprite.color;

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
