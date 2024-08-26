using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;


public class PlatformController : MonoBehaviour
{
    public GameObject platformPrefab;
    public Vector3 startingPosition = new Vector3(4.5f, 2.3f, 4.5f);
    public int maxPlatforms = 2;
    public Vector2 lifeRange;

    private Queue<GameObject> activePlatforms = new Queue<GameObject>();
    private Vector3 lastPosition;
    private Vector3 previousPosition;
    private float platformSize;

    private float minLifetime;
    private float maxLifetime;
    public float spawnTime;

    public string jsonUrl = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    private void Start()
    {
        StartCoroutine(FetchAndParseJson());
        platformSize = platformPrefab.GetComponent<Renderer>().bounds.size.x;
        previousPosition = startingPosition - Vector3.one;
        SpawnStartingPlatform();
        StartCoroutine(SpawnPlatforms());
    }

    [System.Serializable]
    private class PulpitData
    {
        public float min_pulpit_destroy_time;
        public float max_pulpit_destroy_time;
        public float pulpit_spawn_time;
    }

    [System.Serializable]
    private class DoofusDiary
    {
        public PulpitData pulpit_data;
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
    private IEnumerator FetchAndParseJson()
    {
        UnityWebRequest request = UnityWebRequest.Get(jsonUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching JSON data: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            Debug.Log("Raw JSON: " + json);

            // First, try to parse the entire JSON
            DoofusDiary diary = JsonUtility.FromJson<DoofusDiary>(json);
            Debug.Log("Diary: " + diary);

            if (diary != null && diary.pulpit_data != null)
            {
                minLifetime = diary.pulpit_data.min_pulpit_destroy_time;
                maxLifetime = diary.pulpit_data.max_pulpit_destroy_time;
                spawnTime = diary.pulpit_data.pulpit_spawn_time;


                lifeRange = new Vector2(minLifetime, maxLifetime);
            }
            else
            {
                Debug.LogError("Failed to parse JSON into DoofusDiary object. Attempting manual parse.");

                // If the above fails, try to parse just the pulpit_data
                PulpitData pulpitData = JsonUtility.FromJson<PulpitData>(json);

                if (pulpitData != null)
                {
                    Debug.Log("Data" + pulpitData);
                    minLifetime = pulpitData.min_pulpit_destroy_time;
                    maxLifetime = pulpitData.max_pulpit_destroy_time;
                    spawnTime = pulpitData.pulpit_spawn_time;


                    lifeRange = new Vector2(minLifetime, maxLifetime);
                }
                else
                {
                    Debug.LogError("Failed to parse JSON into PulpitData object. Raw JSON: " + json);
                }
            }
        }
    }
}

