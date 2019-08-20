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
    public float PRP_changeX = 0.5f;
    public float PRP_changeY = 0.5f;
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
            backs.Add(backobj.GetComponent<MeshFilter>().mesh);
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 offset = mousePosition - startHold;
            
            if(isDragging)
            {
                //Find vertex are changed
                for(int i = 0; i < vertices.Length; i++)
                {
                    Vector3 v = vertices[i];
                    if(Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
                    {
                        if((offset.x < 0 && v.x > mousePosition.x) || (offset.x > 0 && v.x < mousePosition.x))
                        {
                            v.x += offset.x;
                        }
                    }
                    else
                    {
                        if((offset.y < 0 && v.y > mousePosition.y) || (offset.y > 0 && v.y < mousePosition.y))
                        {
                            v.y += offset.y;
                        }
                    }
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
                if(vertices[j].y > vertices[i].y)
                {
                    Vector2 p = vertices[i];
                    vertices[i] = vertices[j];
                    vertices[j] = p;
                }
                // else if(vertices[j].y == vertices[i].y && vertices[j].x > vertices[i].x)
                // {
                //     Vector2 p = vertices[i];
                //     vertices[i] = vertices[j];
                //     vertices[j] = p;
                // }
            }
            Debug.Log(vertices[i] + ", ");
        }

    }
    void ReCalculateTriangle()
    {
        
    }

    public Vector2[] CopyVerticesToUV(Vector3[] p_vertices)
    {
        Vector2[] newUV = new Vector2[p_vertices.Length];
        for(int i = 0; i < p_vertices.Length; i++) newUV[i] = new Vector2(p_vertices[i].x + PRP_changeX, p_vertices[i].y + PRP_changeY);
        return newUV;
    }
}
