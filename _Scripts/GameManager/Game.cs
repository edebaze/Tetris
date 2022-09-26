using System;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public double baseFallTick;
    public TMP_Text scoreText;
    
    public AudioSource audioSourceTheme;
    public AudioSource audioSourceEffects;
    public AudioClip completeLineAudio;
    public AudioClip completeTetrisAudio;
    public AudioClip moveBlockAudio;
    
    private MenuManager _menuManager;
    private string _playerName;
    private int _score;
    private int _level = 1;

    private static Spawner _spawner;
    public static Transform[,] Grid = new Transform[BackgroundWidth, BackgroundHeight];
    
    public static double FallTick;
    public const int BackgroundWidth = 10;
    public const int BackgroundHeight = 20;


    public void Start() {
        _playerName = PlayerPrefs.GetString("playerName");
        _menuManager = GetComponent<MenuManager>();
        
        SetLevel(_level);
        
        _spawner = GetComponent<Spawner>();
        _spawner.SpawnPosition = new Vector3(Mathf.RoundToInt(BackgroundWidth / 2), BackgroundHeight, 0);
        _spawner.Init();
    }
    
    private void GameOver() {
        SaveScore();
        _menuManager.GameOverScreen();
    }

    private void NextLevel() {
        SetLevel(_level + 1);
    }

    public void Reset() {
        _score = 0;
        scoreText.text = "0";
        SetLevel(1);
        
        // reset spawner
        _spawner.Reset();
        
        // clean all blocks
        foreach (var block in FindObjectsOfType<BlockMovement>()) {
            Destroy(block.gameObject);
        }
    }

    public void SaveScore() {
        ScoreManager.AddScore(new Score(_playerName, _score));
        ScoreManager.SaveScores();
    }
    
    #region Grid
    public void AddToGrid(Transform tetrisForm) {
        // add object to the grid
        foreach (Transform child in tetrisForm) {
            (var x, var y) = GetGridPosition(child.position);

            if (y >= BackgroundHeight) {
                GameOver();
                return;
            }
            
            Grid[x, y] = child;
        }

        // check if object made a line 
        CheckLines();
        
        // spawn a new form
        _spawner.Spawn();
    }

    private void CheckLines() {
        var lineMade = 0;       // line made with last move
        
        for (int i = BackgroundHeight - 1; i >= 0; i--) {
            if (HasLine(i)) {
                DeleteLine(i);
                RowDown(i);
                lineMade += 1;
            }
        }

        if (lineMade > 0) {
            if (lineMade == 4) {
                _score += 1000;
                audioSourceEffects.PlayOneShot(completeTetrisAudio);
            }
            else {
                _score += lineMade * 100;
                audioSourceEffects.PlayOneShot(completeLineAudio);
            }
            
            scoreText.text = _score.ToString();
            
            if (_score > 5000 * _level) {
                NextLevel();
            }
        }
    }

    public static bool HasLine(int i) {
        for (int j = 0; j < BackgroundWidth; j++) {
            if (Grid[j, i] == null) {
                return false;
            }
        }
        
        return true;
    }

    private static void DeleteLine(int i) {
        for (int j = 0; j < BackgroundWidth; j++) {
            Destroy(Grid[j, i].gameObject);
            Grid[j, i] = null;
        }
    }

    private static void RowDown(int i) {
        for (int y = i; y < BackgroundHeight; y++) {
            for (int j = 0; j < BackgroundWidth; j++) {
                if (Grid[j, y] != null) {
                    Grid[j, y - 1] = Grid[j, y];
                    Grid[j, y] = null;
                    Grid[j, y - 1].position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    
    #endregion

    public void SetLevel(int level) {
        _level = level;
        FallTick = Math.Max(0.01, baseFallTick * Math.Pow(0.8, level));
    }
    
    public static (int, int) GetGridPosition(Vector2 position) {
        return ((int)Math.Truncate(position.x), (int)Math.Truncate(position.y));
    }
}
