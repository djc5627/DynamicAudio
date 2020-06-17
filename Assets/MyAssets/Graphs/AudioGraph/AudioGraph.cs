using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using XNode;

[Serializable, CreateAssetMenu(fileName = "New Audio Graph", menuName = "Graphs/Audio Graph")]
public class AudioGraph : NodeGraph
{
	private Groove currentGroove;

	private void OnEnable()
	{
		currentGroove = GetStartGroove();
		currentGroove.onFinishGroove += ChangeGroove;
		Debug.Log($"Start Groove is {currentGroove}");
	}

	public Groove GetStartGroove()
	{
		Groove startGroove = null;
		foreach (var node in nodes)
		{
			if (node is Groove grooveNode)
			{
				//For now just picking first node in list
				startGroove = grooveNode;
				break;
			}
		}
		return startGroove;
	}

	public AudioClip GetNextClip()
	{
		return currentGroove.GetNextClip();
	}

	public void ChangeGroove(Groove nextGroove)
	{
		currentGroove.onFinishGroove -= ChangeGroove;
		currentGroove = nextGroove;
		currentGroove.onFinishGroove += ChangeGroove;
		Debug.Log($"Next Groove is {nextGroove.name}");
	}
}