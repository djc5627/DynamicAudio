using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AudioGraphTest : MonoBehaviour
{
    public AudioGraph audioGraph;

    [Button("Test")]
    public void TestGraphPlay()
    {
        audioGraph.PlayGroove();
    }
}
