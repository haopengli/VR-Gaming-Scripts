using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject healthBar;
    public float MaxHP;
    public float CurrentHP;
    public float AC;
    public int score;
    public GameObject scoreboard;
    private Text scoreText;

	// Use this for initialization
	void Start () {
        CurrentHP = MaxHP;
        score = 0;
        scoreText = scoreboard.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        scoreText.text = score.ToString();
        float ratio = CurrentHP / MaxHP;
        if (ratio < 0)
        {
            ratio = 0;
        }
        healthBar.transform.localScale = new Vector3(ratio, 1, 1);
	}
}
