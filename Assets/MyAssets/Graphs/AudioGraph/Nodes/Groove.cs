using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using XNode;
using System.Linq;
using System.Runtime.CompilerServices;
using TreeEditor;
using Random = UnityEngine.Random;

public class Groove : Node
{
	public string GrooveName;
	[Range(1, 3)] public int intensity = 1;
	[Header("---------------------------------------------------------------------------")]
	public int startPlayCount;
	public GrooveClip[] startClips;
	[Header("---------------------------------------------------------------------------")]
	public int middlePlayCount;
	public GrooveClip[] middleClips;
	[Header("---------------------------------------------------------------------------")]
	public int endPlayCount;
	public GrooveClip[] endClips;
	[Header("---------------------------------------------------------------------------")]
	public Groove[] nextGrooves;
	
	private int currentStartCount = 0;
	private int currentMiddleCount = 0;
	private int currentEndCount = 0;
	
	[Serializable]
	public class GrooveClip
	{
		public AudioClip clip = null;
		public bool randomIndex = true;
		public int playIndex = 0;
	}
	
	public delegate void OnFinishGroove(Groove nextGroove);

	public event OnFinishGroove onFinishGroove;

	// Use this for initialization
	protected override void Init() {
		base.Init();

		SortClips();

		currentStartCount = 0;
		currentMiddleCount = 0;
		currentEndCount = 0;

		name = GrooveName;
	}

	private void SortClips()
	{
		startClips = GetSortedClips(startClips);
		middleClips = GetSortedClips(middleClips);
		endClips = GetSortedClips(endClips);
	}

	private GrooveClip[] GetSortedClips(GrooveClip[] unsortedClips)
	{
		GrooveClip[] sortedClips = new GrooveClip[unsortedClips.Length];
		List<GrooveClip> orderedClips = unsortedClips.ToList();
		List<GrooveClip> randomClips = unsortedClips.ToList();
		orderedClips.RemoveAll(c => { return c.randomIndex;});
		randomClips.RemoveAll(c => { return !c.randomIndex;});

		//Place ordered clips in their index
		foreach (var grooveClip in orderedClips)
		{
			sortedClips[grooveClip.playIndex] = grooveClip;
		}
		
		//Random sort clips and fill remaining indices
		randomClips = randomClips.OrderBy(_ => Guid.NewGuid()).ToList();
		foreach (var grooveClip in randomClips)
		{
			for (int i = 0; i < sortedClips.Length; i++)	//Find and fill first null
			{
				if (sortedClips[i] == null)
				{
					sortedClips[i] = grooveClip;
					break;
				}
			}
		}

		return sortedClips;
	}
	

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null;
	}

	public AudioClip GetNextClip()
	{
		AudioClip nextClip = null;
		if (currentStartCount < startPlayCount)
		{
			nextClip = startClips[currentStartCount].clip;
			currentStartCount++;
		}
		else if (currentMiddleCount < middlePlayCount)
		{
			nextClip = middleClips[currentMiddleCount].clip;
			currentMiddleCount++;
		}
		else if (currentEndCount < endPlayCount)
		{
			nextClip = endClips[currentEndCount].clip;
			currentEndCount++;
		}

		//If this is last clip
		if (currentStartCount == startPlayCount &&
		    currentMiddleCount == middlePlayCount &&
		    currentEndCount == endPlayCount)
		{
			GrooveFinished();
		}

		return nextClip;
	}

	//Reset values here
	private void GrooveFinished()
	{
		if (onFinishGroove != null)
		{
			onFinishGroove.Invoke(PickRandomGroove(nextGrooves));
			Init();
		}
	}

	private void OnValidate()
	{
		//Dont allow play count > clips[] length
		if (startPlayCount > startClips.Length)
		{
			startPlayCount = startClips.Length;
		}
		if (middlePlayCount > middleClips.Length)
		{
			middlePlayCount = middleClips.Length;
		}
		if (endPlayCount > endClips.Length)
		{
			endPlayCount = endClips.Length;
		}

		//Dont let Play Index out of bounds
		for (int i = 0; i < startClips.Length; i++)
		{
			if (startClips[i].playIndex >= startPlayCount)
			{
				startClips[i].playIndex = startPlayCount;
			}
		}
		for (int i = 0; i < middleClips.Length; i++)
		{
			if (middleClips[i].playIndex >= middlePlayCount)
			{
				middleClips[i].playIndex = middlePlayCount;
			}
		}
		for (int i = 0; i < endClips.Length; i++)
		{
			if (endClips[i].playIndex >= endPlayCount)
			{
				endClips[i].playIndex = endPlayCount;
			}
		}
		
		//Debug.
		SortClips();
	}

	private AudioClip PickRandomClip(AudioClip[] clipArray)
	{
		int randomIndex = Random.Range(0, clipArray.Length);
		return clipArray[randomIndex];
	}
	
	private Groove PickRandomGroove(Groove[] grooveArray)
	{
		int randomIndex = Random.Range(0, grooveArray.Length);
		return grooveArray[randomIndex];
	}
}