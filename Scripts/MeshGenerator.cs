using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public struct simpleQuad {
        public Vector3[] verticies { get; set; }
        public int[] triangles { get; set; }

        public simpleQuad(Vector3[] verticies, int[] triangles)
        {
            this.verticies = verticies;
            this.triangles = triangles;
        }
    }

    Mesh mesh;

    public int depth = 5;
    public int width = 5;
    public int height = 5;

    private List<simpleQuad> quads = new List<simpleQuad>();

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        createShape();
        UpdateMesh();
    }

    private void createShape()
    {
        Vector3[] verticies = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0) };

        int[] triangles = new int[] { 0, 1, 2, 1, 3, 2 };


        simpleQuad SQ = new simpleQuad(verticies, triangles);


        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < depth; j++)
            {
                //quads.Add(new simpleQuad(new Vector3(depth,0,width),));
            }
        }


    }

    private void UpdateMesh()
    {
        mesh.Clear();

        //mesh.vertices = verticies;
        //mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        foreach (simpleQuad s in quads)
        {
            
        }
    }
}
