using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

#if (UNITY_EDITOR)
public class RoomManager : EditorWindow
{

    Material setMaterial;

    Vector3 Tiling;

    Transform parentObject;

    GameObject Tile;

    List<GameObject> generatedBlocks = new List<GameObject>();
    List<List<GameObject>> actions = new List<List<GameObject>>();


    [MenuItem("Window/Room Creator")]
    public static void ShowWindow()
    {
        GetWindow<RoomManager>("Room Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("3D Tilemap setter", EditorStyles.whiteLargeLabel);

        EditorGUILayout.Space(20f, true);

        //Set custom Material
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Object Material", GUILayout.MaxWidth(125));
        setMaterial = (Material)EditorGUILayout.ObjectField(setMaterial, typeof(Material), true);

        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //Set Parent to instantiate gameobjects under
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Parent Transform", GUILayout.MaxWidth(125));
        parentObject = (Transform)EditorGUILayout.ObjectField(parentObject, typeof(Transform), true);

        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //Set Tile Prefab to draw
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Tile Prefab", GUILayout.MaxWidth(125));
        Tile = (GameObject)EditorGUILayout.ObjectField(Tile, typeof(GameObject), true);

        GUILayout.EndHorizontal();

        //Set Tiling Vector
        GUILayout.BeginHorizontal();

        Tiling = EditorGUILayout.Vector3Field("Tiling", Tiling, GUILayout.MaxWidth(300));

        GUILayout.EndHorizontal();

        EditorGUILayout.Space(25f);

        //Button to generate the tiles
        GUILayout.BeginHorizontal();

        try
        {
            if (GUILayout.Button("Generate Tiles"))
            {
                generatedBlocks.Clear();
                Vector3 placement = new Vector3();
                //X Axis Extents
                for (int i = 0; i < Tiling.x; i++)
                {
                    for (int j = 0; j < Tiling.y; j++)
                    {
                        for (int k = 0; k < Tiling.z; k++)
                        {
                            placement = new Vector3(i, j, k);
                            GameObject g = Instantiate(Tile, parentObject.position, parentObject.rotation, parentObject);
                            g.transform.localPosition = placement;
                            generatedBlocks.Add(g);
                            
                        }
                    }
                }

                actions.Add(copyOf(generatedBlocks));
            }
        }
        catch (Exception error)
        {
            Debug.LogError(error.Message);
        }

        GUILayout.EndHorizontal();

        //Undo Button
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Undo") && actions.Count > 0)
        {

            for (int i = 0; i < actions[actions.Count - 1].Count; i++)
            {
                DestroyImmediate(actions[actions.Count - 1][i]);
            }
            actions.Remove(actions[actions.Count - 1]);
            Debug.Log("Undo succesful");
        }

        GUILayout.EndHorizontal();

    }

    private void OnValidate()
    {
        //TODO
    }


    private List<GameObject> copyOf(List<GameObject> temp)
    {
        List<GameObject> newList = new List<GameObject>();

        foreach(GameObject g in temp)
        {
            newList.Add(g);
        }

        return newList;
    }
}

#endif
