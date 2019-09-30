using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementShader : MonoBehaviour
{

    public Shader replacementS;

	// Use this for initialization
    private void OnEnable()
    {
        if (replacementS != null)
        {
            GetComponent<Camera>().SetReplacementShader(replacementS, "");
        }
    }

    private void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
