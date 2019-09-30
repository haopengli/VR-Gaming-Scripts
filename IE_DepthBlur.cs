using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class IE_DepthBlur : MonoBehaviour {

    private Shader m_shader;
    private Material m_material;
    [Range(0f, 1f)] public float depthFade; 

    void Awake()
    {
	    // force the camera to render the depth texture. That's usually true anyway but just in case...
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        m_shader = Shader.Find("Hidden/DepthBlur");
        m_material = new Material(m_shader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        CustomBlit(source, dest, m_material);
    }
    
    void CustomBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // When applying an image effect we shade a Quad with our rendered screen output (rendertexture) on it.
 
        // Set new rendertexture as active and feed the source texture into the material
        RenderTexture.active = dest;
        mat.SetTexture("_MainTex", source);
        mat.SetFloat("_DepthFade", depthFade);
 
        // Low-Level Graphics Library calls
 
        GL.PushMatrix(); // Calculate MVP Matrix and push it to the GL stack
        GL.LoadOrtho(); // Set up Ortho-Perspective Transform
              
        m_material.SetPass(0); // start the first rendering pass
 
        GL.Begin(GL.QUADS); // Begin rendering quads
 
        GL.MultiTexCoord2(0, 0.0f, 0.0f); // prepare input struct (Texcoord0 (UV's)) for this vertex
        GL.Vertex3(0.0f, 0.0f, 0.0f); // Finalize and submit this vertex for rendering (bottom left)
 
        GL.MultiTexCoord2(0, 1.0f, 0.0f); // prepare input struct (Texcoord0 (UV's)) for this vertex
        GL.Vertex3(1.0f, 0.0f, 0.0f); // Finalize and submit this vertex for rendering  (bottom right)
 
        GL.MultiTexCoord2(0, 1.0f, 1.0f); // prepare input struct (Texcoord0 (UV's)) for this vertex
        GL.Vertex3(1.0f, 1.0f, 0.0f); // Finalize and submit this vertex for rendering  (top right)
 
        GL.MultiTexCoord2(0, 0.0f, 1.0f); // prepare input struct (Texcoord0 (UV's)) for this vertex
        GL.Vertex3(0.0f, 1.0f, 0.0f); // Finalize and submit this vertex for rendering (top left)
 
        // Finalize drawing the Quad
        GL.End();
        // Pop the matrices off the stack
        GL.PopMatrix();
    }
}