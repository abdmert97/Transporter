using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradient : MonoBehaviour
{
  
    void Awake()
    {
        var texture = new Texture2D(512,512, TextureFormat.ARGB32,false);
        Color[] colors = { Color.white, Color.white };
      //  texture = GradientTextureMaker.Create(colors);
        var mat = GetComponent<Renderer>().sharedMaterial;


        texture.Apply(false);
        mat.SetTexture("_MainTex", texture);
    }   
}
public static class GradientTextureMaker
{
    const int Width = 128;
    const int Height = 128;

    public static Texture2D Create(Color[] colors, TextureWrapMode textureWrapMode = TextureWrapMode.Clamp, FilterMode filterMode = FilterMode.Point, bool isLinear = false, bool hasMipMap = false)
    {
        if (colors == null || colors.Length == 0)
        {
            Debug.LogError("No colors assigned");
            return null;
        }

        int length = colors.Length;
        if (colors.Length > 8)
        {
            Debug.LogWarning("Too many colors! maximum is 8, assigned: " + colors.Length);
            length = 8;
        }

        // build gradient from colors
        var colorKeys = new GradientColorKey[length];
        var alphaKeys = new GradientAlphaKey[length];

        float steps = length - 1f;
        for (int i = 0; i < length; i++)
        {
            float step = i / steps;
            colorKeys[i].color = colors[i];
            colorKeys[i].time = step;
            alphaKeys[i].alpha = colors[i].a;
            alphaKeys[i].time = step;
        }

        // create gradient
        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);

        // create texture
        Texture2D outputTex = new Texture2D(Width, Height, TextureFormat.ARGB32, false, isLinear);
        outputTex.wrapMode = textureWrapMode;
        outputTex.filterMode = filterMode;

        // draw texture
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                outputTex.SetPixel(i, j, gradient.Evaluate((float)i / (float)Width));
            }
           
        }
        outputTex.Apply(false);

        return outputTex;
    } // BuildGradientTexture

}
