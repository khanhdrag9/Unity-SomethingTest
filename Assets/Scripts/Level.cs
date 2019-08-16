using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Level : MonoBehaviour
{
    public float blockHeight = 1f;
    public Material blockMaterial;
    public PhysicMaterial blockPhysicsMaterial;
    Vector2 mouseHit;
    bool dragging = false;

    LineRenderer lineRender;

    void Start()
    {   
        lineRender = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) OnMouseDown();
        else if(Input.GetMouseButtonUp(0)) OnMouseUp();

        if(dragging)
        {
            lineRender.enabled = true;
            lineRender.SetPosition(0, mouseHit);
            lineRender.SetPosition(1, MousePosition());
        }
        else 
        {
            lineRender.enabled = false;
        }

    }

    void CreateBox(Vector2 p1, Vector2 p2)
    {
        GameObject newLedge = new GameObject("Block");
        Mesh mesh = new Mesh();
        newLedge.AddComponent<MeshFilter>(); 
        newLedge.AddComponent<MeshRenderer>(); 

        // Vector3[] vertices = new Vector3[]
        // {
        //     new Vector3(0, 0, 0),
        //     new Vector3(0, 0, 0),
        //     new Vector3(0, 0, .5f),
        //     new Vector3(0, 0, .5f),
        //     new Vector3(0, 0, -.5f),
        //     new Vector3(0, 0, -.5f),
        // };

        Vector3 topLeftFront = p1;
        Vector3 topRightFront = p2;
        Vector3 topLeftBack = p1;
        Vector3 topRightBack = p2;
        Vector3 botLeftFront;
        Vector3 botRightFront;

        topRightFront.z = 0.5f;
		topLeftFront.z = 0.5f;
		topLeftBack.z = -0.5f;
		topRightBack.z = -0.5f;

        botLeftFront = topLeftFront;
		botRightFront = topRightFront;
		botLeftFront.y -= blockHeight;
		botRightFront.y -= blockHeight;

        mesh.vertices = new Vector3[]
        {
            topLeftFront,
            topRightFront,
            topLeftBack,
            topRightBack,
            botLeftFront,
            botRightFront
        };

        Vector2[] uvs = new Vector2[mesh.vertices.Length];
		for (int i=0; i< uvs.Length; i++) {
			uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
		}
		mesh.uv = uvs;

        mesh.triangles = new int[]{5, 4, 0, 0, 1, 5, 0, 2, 3, 3, 1, 0};

        mesh.RecalculateNormals();

        newLedge.GetComponent<MeshFilter>().mesh = mesh;
        if(blockMaterial) newLedge.GetComponent<MeshRenderer>().material = blockMaterial;

        newLedge.AddComponent<MeshCollider>();
        if(blockPhysicsMaterial) newLedge.GetComponent<MeshCollider>().material = blockPhysicsMaterial;
    }

    void OnMouseDown()
    {
        mouseHit = MousePosition();
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
        CreateBox(mouseHit, MousePosition());
    }

    public Vector2 MousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
