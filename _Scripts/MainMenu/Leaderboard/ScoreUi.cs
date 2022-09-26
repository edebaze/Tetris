using System;
using System.Linq;
using UnityEngine;

public class ScoreUi: MonoBehaviour
{
    public RowUi rowUi;

    public const int MAX_SCORES = 5;

    private void Start() {
        ScoreManager.ReadScores();
        
        var scores = ScoreManager.GetHighScores().ToArray();
        for (int i = 0; i < Mathf.Min(scores.Length, MAX_SCORES); i++) {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}
