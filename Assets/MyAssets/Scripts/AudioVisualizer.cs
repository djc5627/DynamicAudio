using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Singleton class
/// </summary>
[RequireComponent(typeof (AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    public static AudioVisualizer Instance;
    
    public bool EnableDebugGUI = true;
    public bool UseBuffer = true;
    public AudioClip AudioClip;
    public int SampleSize = 512;
    public FrequencyBand[] FrequencyBands;
    public float fallFactor = .5f;
    public float MaxCubeScale = 5f;
    public float MaxIntensity = 3f;
    public Transform[] BandCubes;
    public MeshRenderer[] BandCubeRends;
    
    
    private float[] samples;
    private float[] sampleAverages;
    private BandState[] bandStates;
    private Color[] cubeEmissionStartColors;
    private AudioSource audioSource;
    

    [System.Serializable]
    public struct FrequencyBand {
        public string Name;
        public int startFrequency;
        public int endFrequency;
    }
    
    private class BandState
    {
        public float averageAmp = 0f;
        public float maxAmp = 0f;
        public float lastAmp = 0f;
        public float percentToMaxAmp = 0f;
        public int sampleCount = 0;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        
        audioSource = GetComponent<AudioSource>();
        samples = new float[SampleSize];
        sampleAverages = new float[SampleSize];
        bandStates = new BandState[FrequencyBands.Length];
        cubeEmissionStartColors = new Color[FrequencyBands.Length];
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            bandStates[b] = new BandState();
            cubeEmissionStartColors[b] = new Color();
        }
    }

    private void Start() {
        InitCubeStartColors();
        audioSource.PlayOneShot(AudioClip);
    }

    private void Update()
    {
        GetSpectrumData();
        PopulateFrequencyBands();
        if (UseBuffer) ApplyBuffer();
        VisualizeBands();
    }

    private void InitCubeStartColors()
    {
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            cubeEmissionStartColors[b] = BandCubeRends[b].material.GetColor("_EmissionColor");
        }
    }

    private void GetSpectrumData() {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void PopulateFrequencyBands() {
        //Range of a spectrum sample is : (sample rate of clip / number of samples)
        float sampleWidth = AudioClip.frequency / SampleSize;

        //Get average frequency of each spectrum sample
        for (int i = 0; i < SampleSize; i++)
        {
            float startFrequency = i * sampleWidth;
            float endFrequency = (i + 1) * sampleWidth;
            sampleAverages[i] = (startFrequency + endFrequency) / 2f;
        }

        #warning Can optimize this to do all in 1 pass instead of each bands!
        //See which band the ave freq of each sample falls into
        //Add value to that band until all checked, divide by len for average
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            BandState bs = bandStates[b];
            bs.averageAmp = 0f;
            bs.sampleCount = 0;
            for (int i = 0; i < SampleSize; i++)
            {
                if (sampleAverages[i] >= FrequencyBands[b].startFrequency
                    && sampleAverages[i] < FrequencyBands[b].endFrequency) 
                {
                    bs.averageAmp += samples[i];
                    bs.sampleCount++;
                }
            }
            if (bs.sampleCount > 0) bs.averageAmp /= bs.sampleCount;
            if (bs.averageAmp > bs.maxAmp) bs.maxAmp = bs.averageAmp;

            bs.percentToMaxAmp = Mathf.InverseLerp(0f, bs.maxAmp, bs.averageAmp);
        }
    }

    private void ApplyBuffer()
    {
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            BandState bs = bandStates[b];
            if (Mathf.Approximately(bs.lastAmp ,0f)) bs.lastAmp = bs.averageAmp;
            if (bs.averageAmp < bs.lastAmp)
            {
                float bufferedAmp = bs.lastAmp * (1 - fallFactor * Time.deltaTime);
                bs.percentToMaxAmp = Mathf.InverseLerp(0f, bs.maxAmp, bufferedAmp);
                bs.lastAmp = bufferedAmp;
            }
            else
            {
                bs.lastAmp = bs.averageAmp;
            }
        }
    }

    private void VisualizeBands()
    {
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            BandCubes[b].localScale = new Vector3 (1f, bandStates[b].percentToMaxAmp * MaxCubeScale, 1f);
            float intensityFactor = Mathf.Pow(bandStates[b].percentToMaxAmp, 2f) * MaxIntensity;
            Color newColor = cubeEmissionStartColors[b] * intensityFactor;
            BandCubeRends[b].material.SetColor("_EmissionColor", newColor);
        }
    }

    public float[] GetBandValues()
    {
        //get percent to max for each band
        float[] bandValues = new float[FrequencyBands.Length];
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            bandValues[b] = bandStates[b].percentToMaxAmp;
        }

        return bandValues;
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    public void SetCurrentClip(AudioClip clip)
    {
        this.AudioClip = clip;
    }

    private void OnGUI()
    {
        if (!EnableDebugGUI) return;

        string bandsInfo = "# | Start | End | Samples | %Max \n\n";
        for (int b = 0; b < FrequencyBands.Length; b++)
        {
            var band = FrequencyBands[b];
            var bs = bandStates[b];
            bandsInfo += b + " | " + band.startFrequency + " | " + band.endFrequency + " | " 
                + bs.sampleCount + " | " + bs.percentToMaxAmp.ToString("F2") + "\n";
        }
        
        GUI.TextArea(new Rect(10, 10, 250, 30 * 7), bandsInfo);
    }
}
