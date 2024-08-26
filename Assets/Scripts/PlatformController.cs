using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class PlatformController : MonoBehaviour
{
    public GameObject platformPrefab;
    public Vector3 startingPosition = new Vector3(4.5f, 2.3f, 4.5f);
    public float spawnTime = 2.5f;
    public int maxPlatforms = 2;
    public Vector2 lifeRange = new Vector2(4f, 5f);

    private Queue<GameObject> activePlatforms = new Queue<GameObject>();
    private Vector3 lastPosition;
    private Vector3 previousPosition;
    private float platformSize;


    private void Start()
    {
        platformSize = platformPrefab.GetComponent<Renderer>().bounds.size.x;
        previousPosition = startingPosition - Vector3.one; 
        SpawnStartingPlatform();
        StartCoroutine(SpawnPlatforms());
    }

    private void SpawnStartingPlatform()
    {
        GameObject startingPlatform = Instantiate(platformPrefab, startingPosition, Quaternion.identity);
        lastPosition = startingPosition;
        activePlatforms.Enqueue(startingPlatform);
        StartCoroutine(DestroyPlatformAfterLifetime(startingPlatform));
    }

    private IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            if (activePlatforms.Count < maxPlatforms)
            {
                SpawnPlatform();
            }
        }
    }

    private void SpawnPlatform()
    {
        Vector3 spawnPosition = GetAdjacentPosition();
        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        activePlatforms.Enqueue(platform);
        StartCoroutine(DestroyPlatformAfterLifetime(platform));
        previousPosition = lastPosition;
        lastPosition = spawnPosition;
    }

    private Vector3 GetAdjacentPosition()
    {
        List<Vector3> possiblePositions = new List<Vector3>
        {
            lastPosition + Vector3.right * platformSize,
            lastPosition + Vector3.left * platformSize,
            lastPosition + Vector3.forward * platformSize,
            lastPosition + Vector3.back * platformSize
        };

        possiblePositions.RemoveAll(pos => pos == previousPosition);

        if (possiblePositions.Count == 0)
        {
            Debug.LogWarning("No valid positions found");
            possiblePositions = new List<Vector3>
            {
                lastPosition + Vector3.right * platformSize,
                lastPosition + Vector3.left * platformSize,
                lastPosition + Vector3.forward * platformSize,
                lastPosition + Vector3.back * platformSize
            };
        }

        return possiblePositions[Random.Range(0, possiblePositions.Count)];
    }

    private IEnumerator DestroyPlatformAfterLifetime(GameObject platform)
    {
        float lifetime = Random.Range(lifeRange.x, lifeRange.y);

        TextMeshProUGUI timeText = platform.GetComponentInChildren<TextMeshProUGUI>();

        float elapsed = 0f;

        while (elapsed < lifetime)
        {
            elapsed += Time.deltaTime;
            float remainingTime = Mathf.Max(0, lifetime - elapsed);
            timeText.text = remainingTime.ToString("F1") + "s"; 

            yield return null;
        }

        if (platform != null && activePlatforms.Contains(platform))
        {
            activePlatforms.Dequeue();
            Destroy(platform);
        }
    }

}

