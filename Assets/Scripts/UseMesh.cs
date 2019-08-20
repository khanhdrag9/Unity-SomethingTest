using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// - từ start đến mouse tạo ra 1 direct, 1 angle
// - lấy các đỉnh vía sau ( dùng direct tìm điểm phía sau)
// - chỉ cần 1 điểm gần nhất
// - các đỉnh còn lại sẽ liên kết qua angle
// - bố cục điểm ko thay đổi
// - các render triangle: 
//     - đầu -> cuối -> cuối trừ 1,
//     - cuối trừ 1 -> đầu  -> đầu + 1
//     - đầu + 1 -> cuối trừ 1 -> đầu + 2.

public class UseMesh : MonoBehaviour
{
    public MeshFilter OBJ_fontPage;
    public Material MAT_backPage;
    public float PRP_left = -0.5f;
    public float PRP_right = 0.5f;
    public float PRP_top = 0.5f;
    public float PRP_bot = -0.5f;
    float PRP_changeX = 0.5f;
    float PRP_changeY = 0.5f;
    public Vector2 PRP_size = new Vector2(7, 7);

    public enum DirectDrag{
        TOP_LEFT,
        TOP_RIGHT,
        BOT_LEFT,
        BOT_RIGHT
    };

    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    Vector2 startHold;
    bool isDragging = false;
    DirectDrag dragCurrent;

    List<Mesh> backs;

    void Start()
    {
        backs = new List<Mesh>();
        SquareOrigin();
        ApplyToFront(vertices, uv, triangles);
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            startHold = mousePosition;
            isDragging = true;

            GameObject backobj = new GameObject("Back", typeof(MeshFilter), typeof(MeshRenderer));
            backobj.GetComponent<MeshRenderer>().material = MAT_backPage;
            backobj.GetComponent<MeshFilter>().mesh = new Mesh();
            backs.Add(backobj.GetComponent<MeshFilter>().mesh);
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 offset = mousePosition - startHold;
            
            if(isDragging)
            {
                List<Vector3> backVetices = new List<Vector3>();

                //Find vertex are changed
                for(int i = 0; i < vertices.Length; i++)
                {
                    Vector3 v = vertices[i];
                    if(Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
                    {
                        if((offset.x < 0 && v.x > mousePosition.x) || (offset.x > 0 && v.x < mousePosition.x))
                        {
                            v.x += offset.x/2;
                            backVetices.Add(v);
                        }
                    }
                    else
                    {
                        if((offset.y < 0 && v.y > mousePosition.y) || (offset.y > 0 && v.y < mousePosition.y))
                        {
                            v.y += offset.y/2;
                            backVetices.Add(v);
                        }
                    }
                    vertices[i] = v;

                   
                }

                int[] backTri = new int[backVetices.Count * 2];
                
                if(backVetices.Count == 2)
                {
                    backVetices.Add(backVetices[0] + new Vector3(offset.x/2, offset.y/2, 0));
                    backVetices.Add(backVetices[1] + new Vector3(offset.x/2, offset.y/2, 0));
                    backTri = new int[]{0,1,3, 3, 2, 0};
                }
                else if(backVetices.Count == 1)
                {

                }

                // Mesh current = backs[backs.Count - 1];
                if(backTri.Length > 0)
                {
                    backs[backs.Count - 1].Clear(false);
                    backs[backs.Count - 1].vertices = backVetices.ToArray();
                    backs[backs.Count - 1].uv = CopyVerticesToUV(backs[backs.Count - 1].vertices);
                    backs[backs.Count - 1].triangles = backTri;
                    backs[backs.Count - 1].RecalculateNormals();
                }

                
                
                ReorderVertex();
                uv = CopyVerticesToUV(vertices);
                ReCalculateTriangle();
                // triangles = triangles;                
                ApplyToFront(vertices, uv, triangles);    
            }

        }
        else if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

    }

    public void Reset()
    {
        foreach(var b in backs) b.Clear(false);
        backs.Clear();

        SquareOrigin();
        ApplyToFront(vertices, uv, triangles);
    }

    void SquareOrigin()
    {
        this.vertices = new Vector3[]
        {
            new Vector3(PRP_left, PRP_top, 0),
            new Vector3(PRP_right, PRP_top, 0),
            new Vector3(PRP_right, PRP_bot, 0),
            new Vector3(PRP_left, PRP_bot, 0)
        };

        this.uv = new Vector2[]
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };

        this.triangles = new int[] {
            0, 1, 2, 
            2, 3, 0
        };
    }

    void ApplyToFront(Vector3[] vertices, Vector2[] uv, int[] triangles)
    {
        OBJ_fontPage.mesh.Clear(false);
        OBJ_fontPage.mesh.vertices = vertices;
        OBJ_fontPage.mesh.uv = uv;
        OBJ_fontPage.mesh.triangles = triangles;
        OBJ_fontPage.mesh.RecalculateNormals();
    }

    void ApplyToBack(Vector3[] vertices, Vector2[] uv, int[] triangles)
    {

    }

    bool CheckVertex(Vector3[] verticesCheck)
    {
        float d = 1*Mathf.Sqrt(2);
        for(int i = 0; i < verticesCheck.Length - 1; i++)
        {
            for(int j = i + 1; j < verticesCheck.Length; j++)
            {
                if((verticesCheck[i] - verticesCheck[j]).magnitude > d)return false;
            }
        }
        return true;
    }

    void ReorderVertex()
    {
        Vector3[] newVerties = new Vector3[vertices.Length];
        for(int i = 0; i < vertices.Length - 1; i++)
        {
            for(int j = i + 1; j < vertices.Length; j++)
            {
                if(vertices[j].y > vertices[i].y || vertices[j].x < vertices[i].x)
                {
                    Vector2 p = vertices[i];
                    vertices[i] = vertices[j];
                    vertices[j] = p;
                }
            }
            // Debug.Log(vertices[i] + ", ");
        }

    }
    void ReCalculateTriangle(int[] tri)
    {
        int end = tri.Length - 1;
        List<int> newTri = new List<int>();
        int subStart = 0;
        int subEnd = 0;
        while(true)
        {
            newTri.Add(subStart);
            newTri.Add(end - subEnd - 1);
            newTri.Add(end - subEnd);
            if(end - subEnd - 1 <= subStart)break;

            newTri.Add(end - subEnd);
            newTri.Add(subStart + 1);
            newTri.Add(subStart);

            subEnd++;
            subStart++;
            if(end - subEnd <= subStart + 1)break;
        }

        triangles = newTri.ToArray();
    }

    public Vector2[] CopyVerticesToUV(Vector3[] p_vertices)
    {
        Vector2[] newUV = new Vector2[p_vertices.Length];
        for(int i = 0; i < p_vertices.Length; i++) newUV[i] = new Vector2(p_vertices[i].x/PRP_size.x + PRP_changeX, p_vertices[i].y/PRP_size.y + PRP_changeY);
        return newUV;
    }
}
