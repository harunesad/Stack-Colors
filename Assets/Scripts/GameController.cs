using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Text scoreText;
    int score;
    float multiValue;
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
        scoreText.text = score.ToString();
    }
    public void UpdateMulti(float valueIn)
    {
        if (valueIn <= multiValue)
        {
            return;
        }
        multiValue = valueIn;
        scoreText.text = (score * multiValue).ToString();
    }
}
