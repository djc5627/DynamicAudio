using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable, CreateAssetMenu(fileName = "New Audio Graph", menuName = "Graphs/Audio Graph")]
public class AudioGraph : NodeGraph {
	public void PlayGroove()
	{
		foreach (var node in nodes)
		{
			if (node is Groove grooveNode)
			{
				//var previous = grooveNode.GetInputValue<Groove>("previous");
				//Debug.Log($"Prev is: {previous}");
				grooveNode.StartGroove();
			}
		}
	}
}