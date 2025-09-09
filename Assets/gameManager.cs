using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public enum gameState { start, play, end }
    public gameState state;
    [SerializeField] bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        state = gameState.start;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
