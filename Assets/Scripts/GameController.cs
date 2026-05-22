using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Text scoreText;
    int score;
    float multiValue = 1f; // FIX #5: default multiplier is 1 (not 0) so score always displays correctly
    public int sceneIndex;
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void UpdateScore(int valueIn)
    {
        score += valueIn;
        // FIX #5: always apply current multiplier when displaying score
        scoreText.text = Mathf.RoundToInt(score * multiValue).ToString();
    }
    public void UpdateMulti(float valueIn)
    {
        if (valueIn <= multiValue)
        {
            return;
        }
        multiValue = valueIn;
        // FIX #5: update display with new multiplier applied to current score
        scoreText.text = Mathf.RoundToInt(score * multiValue).ToString();
    }
}
