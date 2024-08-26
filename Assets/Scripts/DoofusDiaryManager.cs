using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class PlayerData
{
    public float speed;
}

[System.Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}

[System.Serializable]
public class DoofusDiary
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

public class DoofusDiaryManager : MonoBehaviour
{
    public static DoofusDiaryManager Instance { get; private set; }

    public string jsonUrl = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";
    public DoofusDiary diary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(FetchAndParseJson());
        }
        else
        {
            Destroy(gameObject);
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
            try
            {
                diary = JsonUtility.FromJson<DoofusDiary>(json);

                if (diary != null && diary.pulpit_data != null)
                {
                    Debug.Log("JSON parsed successfully");
                }
                else
                {
                    Debug.LogError("Parsed DoofusDiary is null or missing pulpit_data");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception during JSON parsing: " + e.Message);
            }
        }
    }
}
