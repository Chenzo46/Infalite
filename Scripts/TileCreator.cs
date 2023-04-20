using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreator : MonoBehaviour
{

    public float size;
    public GameObject tile;

    public PhysicMaterial colMat;

    private GameObject[] tiles;

    public bool finishedDrawing = false;
    public bool clear = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        tiles = GameObject.FindGameObjectsWithTag("dTile");

        if (!finishedDrawing)
        {
            placeTiles();
        }

        if (!clear)
        {
            foreach (GameObject g in tiles)
            {
                DestroyImmediate(g);
            }
            clear = true;
        }
        
    }

    private void placeTiles()
    {

        Vector3 currentPos = new Vector3(0,0,0);


        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {


                GameObject t = Instantiate(tile, currentPos, transform.rotation, transform);
                t.tag = "dTile";
                t.transform.localPosition = currentPos;

                if (i == Mathf.Floor(size / 2) && j == Mathf.Floor(size / 2))
                {
                    t.AddComponent<BoxCollider>().size = new Vector3(size, size, 1);
                    t.GetComponent<BoxCollider>().material = colMat;
                    if (size % 2 == 0)
                    {
                        t.GetComponent<BoxCollider>().center = new Vector3(-0.5f, -0.5f, 0);
                    }

                    
                    
                }

                currentPos += Vector3.right;
            }
            currentPos += Vector3.up;
            currentPos = new Vector3(0,currentPos.y,0);
            
        }
        tiles = GameObject.FindGameObjectsWithTag("dTile");

        finishedDrawing = true;
    }
}
