using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGen))]
public class MapGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapGent = (MapGen)target;
        if(DrawDefaultInspector())
        {
            if(mapGent.autoUpdate)
            {
                mapGent.DrawMapInEditor();
            }
        }
        if(GUILayout.Button("Generate"))
        {
            mapGent.DrawMapInEditor();
        }
    }
}
