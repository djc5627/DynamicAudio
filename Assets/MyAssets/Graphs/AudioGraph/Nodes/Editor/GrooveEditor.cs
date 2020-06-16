using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using XNode.Examples.MathNodes;
using XNodeEditor;

[NodeEditor.CustomNodeEditorAttribute(typeof(Groove))]
public class GrooveEditor : NodeEditor
{
    
    /// <summary> Draws standard field editors for all public fields </summary>
    public override void OnBodyGUI() {
        // Unity specifically requires this to save/update any serial object.
        // serializedObject.Update(); must go at the start of an inspector gui, and
        // serializedObject.ApplyModifiedProperties(); goes at the end.
        serializedObject.Update();
        string[] excludes = { "m_Script", "graph", "position", "ports" };

        // Iterate through serialized properties and draw them like the Inspector (But with ports)
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        EditorGUIUtility.labelWidth = 110;
        while (iterator.NextVisible(enterChildren)) {
            enterChildren = false;
            if (excludes.Contains(iterator.name)) continue;
            NodeEditorGUILayout.PropertyField(iterator, true);
        }

        // Iterate through dynamic ports and draw them in the order in which they are serialized
        foreach (XNode.NodePort dynamicPort in target.DynamicPorts) {
            if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
            NodeEditorGUILayout.PortField(dynamicPort);
        }

        serializedObject.ApplyModifiedProperties();
        
        Groove node = target as Groove;
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
