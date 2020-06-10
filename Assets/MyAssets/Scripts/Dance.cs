using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour
{
    public int bandIndex = 0;
    
    private Animator anim;
    private float[] bandValues;

    void Awake()
    {
        anim = GetComponent<Animator>();
        bandValues = new float[7];
    }

    void Update()
    {
        bandValues = AudioVisualizer.Instance.GetBandValues();
        anim.SetFloat("Dance", bandValues[bandIndex]);
    }
}
