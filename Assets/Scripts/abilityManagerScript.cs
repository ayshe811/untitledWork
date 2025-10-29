using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

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
    public Ability repelField;

    [Header("Shared Charge System")]
    public float currentCharge = 0f;
    public float maxCharge = 100f;

    private spawnerScript ruinSpawner;
    public List<Color> originalRuinColors = new List<Color>();
    private List<GameObject> affectedRuins = new List<GameObject>();

    public GameObject player;
    Color originalPlayerColour;
    bool originalPlayerColourStored = false;

    [SerializeField] float repelRadius, repelForce;


    private void Start()
    {
        ruinSpawner = FindObjectOfType<spawnerScript>();
    }
    public void AddCharge(float amount)
    {
        currentCharge = Mathf.Min(currentCharge + amount, maxCharge);

        if (currentCharge >= maxCharge)
        {
            //TriggerRandomAbility();
            StartCoroutine(ActivateMagnetField());  
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

    void repelInfinity()
    {
        Collider2D[] ruinsinRadius = Physics2D.OverlapCircleAll(player.transform.position, repelRadius);
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();

        foreach(Collider2D ruinCollider in ruinsinRadius)
        {
            if (ruinCollider == null) continue; 
            Debug.Log($"Collider: {ruinCollider.gameObject.name}, Tag: {ruinCollider.tag}");
            if (ruinCollider.CompareTag("Ruins"))
            {
                Rigidbody2D ruinRb = ruinCollider.attachedRigidbody;
                SpriteRenderer ruinSprite = ruinCollider.GetComponent<SpriteRenderer>();

                if (ruinRb != null && ruinSprite != null && ruinSprite.color != playerSprite.color)
                {
                    ApplyIninity(ruinRb, ruinCollider.transform.position);
                }
                else if (ruinRb == null) Debug.LogWarning($"No attached Rigidbody2D found on ruin: {ruinCollider.gameObject.name}");
            }
        }
    }

    private void ApplyIninity(Rigidbody2D ruinRb, Vector3 ruinPosition)
    {
        Vector2 toRuin = ruinPosition - player.transform.position;
        Vector2 repelDirection;

        if (toRuin.x > 0) repelDirection = Vector2.right;
        else if (toRuin.x < 0)repelDirection = Vector2.left;
        else repelDirection = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;

        //Debug.Log($"Applying force to {ruinRb.gameObject.name}");
        //Debug.Log($"Force: {repelDirection * repelForce}");
        //Debug.Log($"Ruin velocity before: {ruinRb.velocity}");
        //Debug.Log($"Ruin position: {ruinPosition}");
        //Debug.Log($"Player position: {player.transform.position}");

        ruinRb.AddForce(repelDirection * repelForce, ForceMode2D.Force);
    }
    private void OnDrawGizmosSelected()
    {
        if (repelField.isActive && player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.transform.position, repelRadius);
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
        repelField.isActive = true;
        Debug.Log("MAGNET FIELD ACTIVATED! Collecting oxygen for " + 
            repelField.duration + " seconds!");

        float endTime = Time.time + repelField.duration;
        while(Time.time < endTime)
        {
            repelInfinity();
            yield return null;
        }

        repelField.isActive = false;
        Debug.Log("Magnet Field ended");
    }
}
