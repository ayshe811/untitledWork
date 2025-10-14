using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerColourScript : MonoBehaviour
{
    public Color[] availableColors; 
    private int currentColorIndex = 0;
    public SpriteRenderer playerSprite;
    public abilityManagerScript abilityScript;

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
        if (!abilityScript.universalColor.isActive) setRandomColour();
        else playerSprite.color = Color.white;
        //abilityManagerScript playerAbility = FindObjectOfType<abilityManagerScript>();
        if (abilityScript != null 
           && !abilityScript.universalColor.isActive
           && !abilityScript.timeSlow.isActive
           && !abilityScript.indicator.isActive
           && !abilityScript.magnetField.isActive) abilityScript.AddCharge(25f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "trigger" && !abilityScript.universalColor.isActive) setRandomColour();
    }
}
