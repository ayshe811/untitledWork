using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class colourManagerScript : MonoBehaviour
{
    public static colourManagerScript Instance;
    public Color[] colourPalette;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public Color GetRandomColor()
    {
        return colourPalette[Random.Range(0, colourPalette.Length)];
    }

}
