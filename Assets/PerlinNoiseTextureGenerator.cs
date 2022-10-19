using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PerlinNoiseTextureGenerator : MonoBehaviour
{

    public int Resolution = 256;
    public int Scale = 1;
    public Vector3 Offset = Vector3.zero;

    Material m_material;
    Texture2D m_texture;

    void OnValidate() {
        m_material = GetComponent<Renderer>().sharedMaterial;
        m_texture = new Texture2D(Resolution, Resolution);
        for (int i = 0; i < Resolution; i++) {
            for (int j = 0; j < Resolution; j++) {
                m_texture.SetPixel(i,j, Color.white * Mathf.PerlinNoise((float)i/Resolution * Scale + Offset.x, (float)j/Resolution * Scale + Offset.y));
            }
        }
        m_texture.Apply();
        m_material.mainTexture = m_texture;
    }
}
