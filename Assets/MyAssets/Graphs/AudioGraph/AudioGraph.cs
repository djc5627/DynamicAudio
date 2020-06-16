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
		Debug.Log($"Start Groove is {currentGroove}");
	}

	public Groove GetStartGroove()
	{
		Groove startGroove = null;
		foreach (var node in nodes)
		{
			if (node is Groove grooveNode)
			{
				//var previous = grooveNode.GetInputValue<Groove>("previous");
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
}