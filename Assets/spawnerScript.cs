using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public GameObject testers;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            Instantiate(testers, new Vector3(Random.Range(transform.position.x -1.9f, transform.position.x + 1.1f), 
                transform.position.y), Quaternion.identity);
            timer = 0;
        }
    }
}
