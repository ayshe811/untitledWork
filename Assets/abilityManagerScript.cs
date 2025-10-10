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
    public void AddCharge(float amount)
    {
        currentCharge = Mathf.Min(currentCharge + amount, maxCharge);

        if (currentCharge >= maxCharge)
        {
            TriggerRandomAbility();
            currentCharge = 0;
        }
    }
    void TriggerRandomAbility()
    {
        Debug.Log("Charge reached 100%! Triggering random ability...");
        int randomAbility = Random.Range(0, 4);

        switch (randomAbility)
        {
            case 0:
                StartCoroutine(ActivateUniversalColor());
                break;
            case 1:
                StartCoroutine(ActivateTimeSlow());
                break;
            case 2:
                StartCoroutine(ActivateIndicator());
                break;
            case 3:
                StartCoroutine(ActivateMagnetField());
                break;
        }
    }

    IEnumerator ActivateUniversalColor()
    {
        universalColor.isActive = true;
        Debug.Log("UNIVERSAL COLOR ACTIVATED! Any color works for " + 
            universalColor.duration + " seconds!");

        yield return new WaitForSeconds(universalColor.duration);

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
