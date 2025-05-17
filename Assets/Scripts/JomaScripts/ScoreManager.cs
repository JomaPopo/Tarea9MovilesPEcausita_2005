using UnityEngine;
using Firebase.Database;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private DatabaseReference reference;
    private string userID;
    private List<ScoreData> scores = new List<ScoreData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        userID = SystemInfo.deviceUniqueIdentifier;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveScore(string playerName, int score)
    {
        StartCoroutine(SaveScoreCoroutine(playerName, score));
    }

    private IEnumerator SaveScoreCoroutine(string playerName, int score)
    {
        // Check if this is a new high score for the player
        var highScoreData = reference.Child("scores").Child(userID).GetValueAsync();
        yield return new WaitUntil(() => highScoreData.IsCompleted);

        bool isNewHighScore = true;
        if (highScoreData.Result.Exists)
        {
            ScoreData existingScore = JsonUtility.FromJson<ScoreData>(highScoreData.Result.GetRawJsonValue());
            if (existingScore.score >= score)
            {
                isNewHighScore = false;
            }
        }

        if (isNewHighScore)
        {
            ScoreData newScore = new ScoreData(playerName, score);
            string json = JsonUtility.ToJson(newScore);
            yield return reference.Child("scores").Child(userID).SetRawJsonValueAsync(json);

            // Also save to global scores
            string newKey = reference.Child("globalScores").Push().Key;
            yield return reference.Child("globalScores").Child(newKey).SetRawJsonValueAsync(json);
        }
    }

    public void LoadScores(Action<List<ScoreData>> callback)
    {
        StartCoroutine(LoadScoresCoroutine(callback));
    }

    private IEnumerator LoadScoresCoroutine(Action<List<ScoreData>> callback)
    {
        var scoresData = reference.Child("globalScores").OrderByChild("score").LimitToLast(5).GetValueAsync();
        yield return new WaitUntil(() => scoresData.IsCompleted);

        scores.Clear();

        if (scoresData.Result.Exists)
        {
            foreach (DataSnapshot snapshot in scoresData.Result.Children.Reverse())
            {
                ScoreData score = JsonUtility.FromJson<ScoreData>(snapshot.GetRawJsonValue());
                scores.Add(score);
            }
        }

        callback?.Invoke(scores);
    }
}

[System.Serializable]
public class ScoreData
{
    public string playerName;
    public int score;

    public ScoreData(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}