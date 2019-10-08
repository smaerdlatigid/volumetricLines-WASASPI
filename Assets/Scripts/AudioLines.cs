using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class AudioLines : MonoBehaviour
{
    public LoopbackAudio Audio;
    VisualEffect visualEffect;
    public float spawnScale;
    public int bufferSmooth = 0;

    public float textureSwitch = 4f;

    Texture2D texture;

    public RenderTexture position; 

    public Gradient gradient; 
    
    public Texture2D CreateTexture(float[,] data, int width, int height)
    {
        texture = new Texture2D(height, width, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(Audio.PostScaledSpectrumData[0] < textureSwitch)
                {
                    //texture.SetPixel(width - 1 - j, height - 1 - i, new Color(data[i, j] / 8f, data[i, j] / 8f, 1 - data[i, j] / 10f, data[i, j] / 10f));
                    texture.SetPixel(width - 1 - j, height - 1 - i, gradient.Evaluate(data[i,j]/10f));
                }
                else
                {
                    texture.SetPixel(width - 1 - j, i, gradient.Evaluate(data[i, j] / 10f));
                    //texture.SetPixel(width-1 - j, i, new Color(data[i, j] / 8f, data[i, j] / 8f, 1 - data[i, j] / 10f, data[i, j] / 10f));
                }
            }
        }
        texture.Apply();
        return texture;
    }

    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        float[,] data = new float[Audio.audioQueue.Count, Audio.SpectrumSize];
        data = Audio.getBuffer(bufferSmooth);
        Audio.bufferRecordPeriod = Mathf.Clamp(0.01f/Audio.WeightedAverage,0.0001f,0.1f) ;

        visualEffect.SetTexture("AudioTexture", CreateTexture(data, Audio.audioQueue.Count, Audio.SpectrumSize)  );
        visualEffect.SetFloat("ScanScale", Audio.WeightedPostScaledSpectrumData[0] * spawnScale);
        visualEffect.SetFloat("WeightedVolume", Audio.WeightedAverage);
    }
}
