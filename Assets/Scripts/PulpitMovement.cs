using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PulpitMovement : MonoBehaviour
{
    private float speed;
    public string jsonUrl = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";
    void Start()
    {
        StartCoroutine(FetchAndParseJson());
    }
    [System.Serializable]
    private class PlayerData
    {
        public float speed;
    }

    [System.Serializable]
    private class DoofusDiary
    {
        public PlayerData player_data;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * speed * 2);
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * speed * 2);
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

            if (diary != null && diary.player_data != null)
            {
                speed = diary.player_data.speed;
            }
            else
            {
                Debug.LogError("Failed to parse JSON into DoofusDiary object. Attempting manual parse.");

                // If the above fails, try to parse just the pulpit_data
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

                if (playerData != null)
                {
                    Debug.Log("Data" + playerData);
                    speed = playerData.speed;
                }
                else
                {
                    Debug.LogError("Failed to parse JSON into PulpitData object. Raw JSON: " + json);
                }
            }
        }
    }
}
