using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EntryGrooves : Node {
	
	public Groove[] startingGrooves;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	public Groove GetStartingGroove()
	{
		return PickRandomGroove(startingGrooves);
	}
	
	private Groove PickRandomGroove(Groove[] grooveArray)
	{
		int randomIndex = Random.Range(0, grooveArray.Length);
		return grooveArray[randomIndex];
	}
}