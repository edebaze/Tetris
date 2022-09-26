 using System;
 using System.Collections.Generic;
 using System.IO;
 using System.Linq;
 using UnityEngine;

 public static class ScoreManager
 {
     public static List<Score> Scores => _scoreList.scores;             // list of scores
    
     private static ScoreListSerializable _scoreList = new ();          // Serializable object containing the list of scores 
     private static readonly string _jsonDataPath = "/Data/leaderboard.json";     // path to json file

     [Serializable]
     public class ScoreListSerializable
     {
         public List<Score> scores = new ();
     }
     
     public static void SaveScores() {
         File.WriteAllText(JsonPath(), JsonUtility.ToJson(_scoreList));
     }

     public static void ReadScores() {
         if (File.Exists(JsonPath())) {
             var json = File.ReadAllText(JsonPath());
             _scoreList = JsonUtility.FromJson<ScoreListSerializable>(json);
         }
         else {
             File.Create(JsonPath()).Dispose();
         }
     }

     public static IEnumerable<Score> GetHighScores() {
         return Scores.OrderByDescending(x => x.score);
     }

     public static void AddScore(Score score) {
         Scores.Add(score);
     }

     public static string JsonPath() {
         return Application.dataPath + _jsonDataPath;
     }
 }
