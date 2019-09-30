using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{

    public int dice = 4;
    private ParticleSystem ps;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
	}

    void OnParticleCollision(GameObject other)
    {
        if (other.name != "Terrain")
        {
            Destroy(gameObject);
        }
        
    }
}
