using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public enum gameState { start, play, end }
    public gameState state;
    [SerializeField] bool isPaused;

    GameObject player;
    Rigidbody2D playerRB;
    // Start is called before the first frame update
    void Start()
    {
        state = gameState.start;
        isPaused = false;

        player = GameObject.Find("player");
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == gameState.start) playerRB.gravityScale = 0f;
        else if (state == gameState.play) playerRB.gravityScale = 3f;
    }
}
