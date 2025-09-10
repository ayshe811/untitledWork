using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public GameObject[] ruinPrefabs;

    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;

    public List<GameObject> activeRuins = new List<GameObject>();

    Vector2 xPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xPosition = transform.position;
        if (Time.time >= nextSpawnTime)
        {
            SpawnRuin();
            nextSpawnTime = Time.time + spawnRate;
        }

        for (int i = activeRuins.Count - 1; i >= 0; i--)
        {
            if (activeRuins[i].transform.position.y < transform.position.y - 20f)
            {
                Destroy(activeRuins[i]);
                activeRuins.RemoveAt(i);
            }
        }
    }

    void SpawnRuin()
    {
        int randomIndex = Random.Range(0, ruinPrefabs.Length);
        float randomX = Random.Range(xPosition.x - 3f, xPosition.x + 3f);

        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);
        GameObject newRuin = Instantiate(ruinPrefabs[randomIndex], 
            spawnPosition, Quaternion.identity);

        activeRuins.Add(newRuin);
    }
}
