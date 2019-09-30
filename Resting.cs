using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resting : MonoBehaviour {

    public GameObject healthBarScript;
    public float rateOfHealthIncrease = 1f;
    private float changedRateOfHealthIncrease;
    private HealthBar healthBar;
    private float delay;

	// Use this for initialization
	void Start () {
        healthBar = healthBarScript.GetComponent<HealthBar>();
        changedRateOfHealthIncrease = rateOfHealthIncrease;
        delay = 1 / changedRateOfHealthIncrease;
    }
	
	// Update is called once per frame
	void Update () {
        delay -= Time.deltaTime;
        if (healthBar.CurrentHP < 50)
        {
            changedRateOfHealthIncrease = 4;
        }
        else
        {
            changedRateOfHealthIncrease = 4 * Mathf.Pow(2, -(healthBar.CurrentHP-50)/25);
        }
	}

    private void OnTriggerStay(Collider collider)
    {
        if (delay <= 0)
        {
            if (healthBar.CurrentHP < healthBar.MaxHP)
            {
                healthBar.CurrentHP += 1;
            }
            delay = 1 / changedRateOfHealthIncrease;
        }
    }
}
