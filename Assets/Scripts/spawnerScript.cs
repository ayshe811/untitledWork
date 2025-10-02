using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public GameObject[] ruinPrefabs;
    public GameObject player;
    public colourRuinScript colourRuin;
    public GameObject trigger;

    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;

    public List<GameObject> activeRuins = new List<GameObject>();

    Vector2 xPosition;
    void Update()
    {
        transform.position = new Vector2(0, player.transform.position.y + 12);
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
        float randomX = Random.Range(xPosition.x - 2f, xPosition.x + 2f);

        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);
        GameObject newRuin = Instantiate(ruinPrefabs[randomIndex], 
            spawnPosition, Quaternion.identity);
        GameObject detector = Instantiate(trigger, spawnPosition, Quaternion.identity);

        activeRuins.Add(newRuin);

        StartCoroutine(MakeDetectorFollowRuin(detector, newRuin));
    }

    IEnumerator MakeDetectorFollowRuin(GameObject detector, GameObject ruin)
    {
        while (ruin != null && detector != null)
        {
            Vector3 newDetectorPos = new Vector2(
                ruin.transform.position.x,
                ruin.transform.position.y + .6f
            );
            detector.transform.position = newDetectorPos;
            yield return null;
        }

        if (detector != null)
            Destroy(detector);
    }
}
