using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject front;
    public GameObject back;

    [Header("Front")]
    public Vector3[] verticesFront;
    public Vector2[] uvFront;
    public int[] trianglesFront;

    [Header("Back")]
    public Vector3[] verticesBack;
    public Vector2[] uvBack;
    public int[] trianglesBack;


    Vector2 mouseStart;
    
    void Start()
    {
        front.GetComponent<MeshFilter>().mesh = new Mesh();
        back.GetComponent<MeshFilter>().mesh = new Mesh();
    }

    void Update()
    {
        UpdateControl();
        UpdateCoord();
    }

    private void UpdateControl()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            mouseStart = mousePos;
        }
        else if(Input.GetMouseButton(0))
        {
            Vector2 offset = (mousePos - mouseStart) * 0.5f;
            Vector2 midPoint = mouseStart + offset;
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            
        }

    }

    void UpdateCoord()
    {
        Mesh mesh = front.GetComponent<MeshFilter>().mesh;
        mesh.vertices = verticesFront;
        uvFront = new Vector2[verticesFront.Length];
        for(int i = 0; i < verticesFront.Length; i++) uvFront[i] = new Vector2(verticesFront[i].x, verticesFront[i].y);
        mesh.uv = uvFront;
        mesh.triangles = trianglesFront;
        mesh.RecalculateNormals();

        mesh = back.GetComponent<MeshFilter>().mesh;
        mesh.vertices = verticesBack;
        uvBack = new Vector2[verticesBack.Length];
        for(int i = 0; i < verticesBack.Length; i++) uvBack[i] = new Vector2(verticesBack[i].x, verticesBack[i].y);
        mesh.uv = uvBack;
        mesh.triangles = trianglesBack;
        mesh.RecalculateNormals();
    }
}
