using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable, CreateAssetMenu(fileName = "New Audio Graph", menuName = "Graphs/Audio Graph")]
public class AudioGraph : NodeGraph {
	public void Start()
	{
		foreach (var node in nodes)
		{
			if (node is AudioClipNode audioNode)
			{
				var previous = audioNode.GetInputValue<AudioClipNode>("previous");
				Debug.Log($"Prev is: {previous}");
				if (previous == null)
				{
					audioNode.Play();
				}
			}
		}
	}
}