using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class Ability
    {
        public bool isActive = false;
        public float duration;
    }

    public Ability universalColor;
    public Ability timeSlow;
    public Ability indicator;
    public Ability magnetField;

    [Header("Shared Charge System")]
    public float currentCharge = 0f;
    public float maxCharge = 100f;

    private spawnerScript ruinSpawner;
    public List<Color> originalRuinColors = new List<Color>();
    private List<GameObject> affectedRuins = new List<GameObject>();

    public GameObject player;
    Color originalPlayerColour;
    bool originalPlayerColourStored = false;
    private void Start()
    {
        ruinSpawner = FindObjectOfType<spawnerScript>();
    }
    public void AddCharge(float amount)
    {
        currentCharge = Mathf.Min(currentCharge + amount, maxCharge);

        if (currentCharge >= maxCharge)
        {
            TriggerRandomAbility();
            currentCharge = 0;
        }
    }
    void ChangeAllRuinsToPlayerColor()
    {
        originalRuinColors.Clear();
        affectedRuins.Clear();

        playerColourScript pColourScript = player.GetComponent<playerColourScript>();
        if (pColourScript == null)
        {
            Debug.LogError("playerColourScript not found on player!");
            return;
        }

        if (ruinSpawner == null) Debug.Log("no spawner found");
        if (ruinSpawner.activeRuins == null) Debug.Log("no ruins found");

        if (ruinSpawner != null && ruinSpawner.activeRuins.Count > 0)
        {
            Color playerColour = player.GetComponent<SpriteRenderer>().color;
            Debug.Log($"9. Player color: {playerColour}");

            foreach (GameObject ruin in ruinSpawner.activeRuins)
            {
                if (ruin != null)
                {
                    // Store original color
                    SpriteRenderer ruinSprite = ruin.GetComponent<SpriteRenderer>();
                    originalRuinColors.Add(ruinSprite.color);
                    affectedRuins.Add(ruin);

                    // Change to player's color
                    ruinSprite.color = playerColour;
                }
            }
        }
    }
    void RestoreRuinColors()
    {
        for (int i = 0; i < ruinSpawner.activeRuins.Count; i++)
        {
            if (ruinSpawner.activeRuins[i] != null)
            {
                SpriteRenderer ruinSprite = ruinSpawner.activeRuins[i].GetComponent<SpriteRenderer>();
                ruinSprite.color = originalRuinColors[i];
            }
        }

        // Clear lists
        originalRuinColors.Clear();
        affectedRuins.Clear();
    }
    void ChangePlayerColour(bool active)
    {
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        if(playerSprite != null)
        {
            if (active)
            {
                if (!originalPlayerColourStored)
                {
                    originalPlayerColour = playerSprite.color;
                    originalPlayerColourStored = true;
                }

                playerSprite.color = Color.white;
            }
            else
            {
                // Return to original color
                playerSprite.color = originalPlayerColour;
                originalPlayerColourStored = false;
            }
        }
    }
    void TriggerRandomAbility()
    {
        Debug.Log("Charge reached 100%! Triggering random ability...");
        int randomAbility = Random.Range(0, 4);

        switch (randomAbility)
        {
            case 0: StartCoroutine(ActivateUniversalColor()); break;
            case 1: StartCoroutine(ActivateTimeSlow()); break;
            case 2: StartCoroutine(ActivateIndicator()); break;
            case 3: StartCoroutine(ActivateMagnetField()); break;
        }
    }
    IEnumerator ActivateUniversalColor()
    {
        universalColor.isActive = true;
        Debug.Log("UNIVERSAL COLOR ACTIVATED! Any color works for " + 
            universalColor.duration + " seconds!");

        ChangePlayerColour(true);
        ChangeAllRuinsToPlayerColor();

        yield return new WaitForSeconds(universalColor.duration);

        //RestoreRuinColors();
        universalColor.isActive = false;
        Debug.Log("Universal Color ended");
    }
    IEnumerator ActivateTimeSlow()
    {
        timeSlow.isActive = true;
        Debug.Log("TIME SLOW ACTIVATED! Time is slowed for " + 
            timeSlow.duration + " seconds!");

        yield return new WaitForSecondsRealtime(timeSlow.duration);

        timeSlow.isActive = false;
        Debug.Log("Time Slow ended");
    }
    IEnumerator ActivateIndicator()
    {
        indicator.isActive = true;
        Debug.Log("INDICATOR ACTIVATED! Showing future ruins for " + 
            indicator.duration + " seconds!");

        yield return new WaitForSeconds(indicator.duration);

        indicator.isActive = false;
        Debug.Log("Indicator ended");
    }
    IEnumerator ActivateMagnetField()
    {
        magnetField.isActive = true;
        Debug.Log("MAGNET FIELD ACTIVATED! Collecting oxygen for " + 
            magnetField.duration + " seconds!");

        yield return new WaitForSeconds(magnetField.duration);

        magnetField.isActive = false;
        Debug.Log("Magnet Field ended");
    }
}
