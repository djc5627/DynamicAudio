using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using XNode.Examples.MathNodes;
using XNodeEditor;

[NodeEditor.CustomNodeEditorAttribute(typeof(AudioClipNode))]
public class AudioClipEditor : NodeEditor
{
   
    /// <summary> Draws standard field editors for all public fields </summary>
    public override void OnBodyGUI() {
        base.OnBodyGUI();

        AudioClipNode node = target as AudioClipNode;
        EditorGUILayout.LabelField("Play");
        if (GUILayout.Button("Play")) node.Play();
    }
    
    public override int GetWidth()
    {
        return 400;
    }
    
    public override Color GetTint()
    {
        Color tint = new Color(184f/255f, 175f/255f, 253f/255f, 1f);
        return tint;
    }
}
