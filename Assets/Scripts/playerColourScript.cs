using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerColourScript : MonoBehaviour
{
    public Color[] availableColors; 
    private int currentColorIndex = 0;
    public SpriteRenderer playerSprite;

    public Color CurrentColour => availableColors[currentColorIndex];
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        setRandomColour();
    }
    public void setRandomColour()
    {
        int newIndex;
        do {
            newIndex = Random.Range(0, availableColors.Length);
        } while (newIndex == currentColorIndex && availableColors.Length > 1);

        currentColorIndex = newIndex;
        playerSprite.color = availableColors[currentColorIndex];
    }

    public void OnCorrectCollision()
    {
        setRandomColour();
        abilityManagerScript playerAbility = FindObjectOfType<abilityManagerScript>();
        if (playerAbility != null) playerAbility.AddCharge(25f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "trigger") setRandomColour();
    }
}
