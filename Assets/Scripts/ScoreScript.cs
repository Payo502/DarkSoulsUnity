using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    public TextMeshProUGUI score;

    private void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        score.text = "Score: " + scoreValue; 
    }
}
