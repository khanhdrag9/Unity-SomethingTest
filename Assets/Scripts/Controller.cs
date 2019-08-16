using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
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

    float width;
    float height;
    float rateScale = 7;
    Vector2 startHold;
    Mesh currentBack = null;
    bool isDragging = false;
    DirectDrag dragCurrent;

    List<GameObject> backs;

    void Start()
    {
        backs = new List<GameObject>();
        SquareOrigin();
        ApplyToFront(vertices, uv, triangles);
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if( mousePosition.x >= PRP_left * PRP_size.x && mousePosition.x <= PRP_right * PRP_size.x &&
            mousePosition.y >= PRP_bot * PRP_size.y && mousePosition.y <= PRP_top * PRP_size.y)
        {
            Vector2 mousePosIn = mousePosition / rateScale;
            if(Input.GetMouseButtonDown(0))
            {
                //Create new Back paper
                startHold = mousePosIn;
                GameObject backObj = new GameObject("Back", typeof(MeshFilter), typeof(MeshRenderer));
                backs.Add(backObj);
                backObj.transform.localScale = PRP_size;
                backObj.GetComponent<MeshRenderer>().material = MAT_backPage;
                MeshFilter mf = backObj.GetComponent<MeshFilter>();
                mf.mesh = new Mesh();
                currentBack = mf.mesh;
                isDragging = true;

                if(mousePosIn.x < 0 && mousePosIn.y > 0) dragCurrent = DirectDrag.TOP_LEFT;
                else if(mousePosIn.x > 0 && mousePosIn.y > 0) dragCurrent = DirectDrag.TOP_RIGHT;
                else if(mousePosIn.x < 0 && mousePosIn.y < 0) dragCurrent = DirectDrag.BOT_LEFT;
                else if(mousePosIn.x > 0 && mousePosIn.y < 0) dragCurrent = DirectDrag.BOT_RIGHT;
            }
        }
            if(Input.GetMouseButton(0))
            {
                Vector2 mousePosIn = mousePosition / rateScale;
                if(isDragging)
                {
                    Vector2 offset = mousePosIn - startHold;
                    float x = 0, y = 0;
                    float half = offset.magnitude * 0.5f;

                    Vector3[] verticesBack;
                    Vector2[] uvBack;
                    int[] trianglesBack;

                    Vector2 pointPull = new Vector2(Mathf.Clamp(mousePosIn.x, PRP_left, PRP_right), Mathf.Clamp(mousePosIn.y, PRP_bot, PRP_top));

                    if(dragCurrent == DirectDrag.TOP_LEFT)
                    {
                        float angleForY = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg + 90;
                        float angleForX = 90 - angleForY;    
                        x = half / Mathf.Cos(angleForY * Mathf.Deg2Rad);
                        y = half / Mathf.Cos(angleForX * Mathf.Deg2Rad);

                        x = Mathf.Clamp(PRP_left - x, PRP_left, PRP_right);
                        y = Mathf.Clamp(PRP_top + y, PRP_bot, PRP_top);

                        //Back
                        verticesBack = new Vector3[]
                        {
                            new Vector3(PRP_left, y, 0),
                            new Vector3(pointPull.x, pointPull.y, 0),
                            new Vector3(x, PRP_top, 0)
                        };
                        if(!CheckVertex(verticesBack))return;
                        uvBack = CopyVerticesToUV(verticesBack);
                        trianglesBack = new int[]
                        {
                            0,1,2
                        };
                        ApplyToBack(verticesBack, uvBack, trianglesBack);

                        //Front
                        vertices = new Vector3[]
                        {
                            new Vector3(PRP_left, y, 0),
                            new Vector3(x, PRP_top, 0),
                            new Vector3(PRP_right, PRP_top, 0),
                            new Vector3(PRP_right, PRP_bot, 0),
                            new Vector3(PRP_left, PRP_bot, 0),
                        };
                        uv = CopyVerticesToUV(vertices);
                        triangles = new int[]
                        {
                            0, 4, 3,
                            3, 0, 1,
                            1, 2, 3
                        };
                        ApplyToFront(vertices, uv, triangles);
                    }
                    else if(dragCurrent == DirectDrag.TOP_RIGHT)
                    {
                        float angleForY = Mathf.Atan2(-offset.x, offset.y) * Mathf.Rad2Deg + 90;
                        float angleForX = 90 - angleForY;    
                        x = half / Mathf.Cos(angleForY * Mathf.Deg2Rad);
                        y = half / Mathf.Cos(angleForX * Mathf.Deg2Rad);

                        x = Mathf.Clamp(PRP_right + x, PRP_left, PRP_right);
                        y = Mathf.Clamp(PRP_top + y, PRP_bot, PRP_top);

                        //Front
                        vertices = new Vector3[]
                        {
                            new Vector3(PRP_left, PRP_top, 0),
                            new Vector3(x, PRP_top, 0),
                            new Vector3(PRP_right, y, 0),
                            new Vector3(PRP_right, PRP_bot, 0),
                            new Vector3(PRP_left, PRP_bot, 0),
                        };
                        uv = CopyVerticesToUV(vertices);
                        triangles = new int[]
                        {
                            0, 4, 1,
                            1, 4, 2,
                            2, 3, 4
                        };
                        ApplyToFront(vertices, uv, triangles);

                        //Back
                        verticesBack = new Vector3[]
                        {
                            new Vector3(PRP_right, y, 0),
                            new Vector3(pointPull.x, pointPull.y, 0),
                            new Vector3(x, PRP_top, 0)
                        };
                        uvBack = CopyVerticesToUV(verticesBack);
                        trianglesBack = new int[]
                        {
                            0,1,2
                        };
                        ApplyToBack(verticesBack, uvBack, trianglesBack); 

                    }
                    else if(dragCurrent == DirectDrag.BOT_LEFT)
                    {
                        float angleForY = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                        float angleForX = 90 - angleForY;    
                        x = half / Mathf.Cos(angleForY * Mathf.Deg2Rad);
                        y = half / Mathf.Cos(angleForX * Mathf.Deg2Rad);

                        x = Mathf.Clamp(x - PRP_changeX, PRP_left, PRP_right);
                        y = Mathf.Clamp(y - PRP_changeY, PRP_bot, PRP_top);

                        //Front
                        vertices = new Vector3[]
                        {
                            new Vector3(PRP_left, PRP_top, 0),
                            new Vector3(PRP_right, PRP_top, 0),
                            new Vector3(PRP_right, PRP_bot, 0),
                            new Vector3(x, PRP_bot, 0),
                            new Vector3(PRP_left, y, 0),
                        };
                        uv = CopyVerticesToUV(vertices);
                        triangles = new int[]
                        {
                            4, 0, 1,
                            1, 4, 3,
                            3, 1, 2
                        };
                        ApplyToFront(vertices, uv, triangles);

                        // Back
                        verticesBack = new Vector3[]
                        {
                            new Vector3(PRP_left, y, 0),
                            new Vector3(pointPull.x, pointPull.y, 0),
                            new Vector3(x, PRP_bot, 0)
                        };
                        uvBack = CopyVerticesToUV(verticesBack);
                        trianglesBack = new int[]
                        {
                            0,1,2
                        };
                        ApplyToBack(verticesBack, uvBack, trianglesBack); 
                    }
                    else if(dragCurrent == DirectDrag.BOT_RIGHT)
                    {
                        float angleForY = Mathf.Atan2(offset.y, -offset.x) * Mathf.Rad2Deg;
                        float angleForX = 90 - angleForY;    
                        x = half / Mathf.Cos(angleForY * Mathf.Deg2Rad);
                        y = half / Mathf.Cos(angleForX * Mathf.Deg2Rad);

                        x = Mathf.Clamp(PRP_right - x, PRP_left, PRP_right);
                        y = Mathf.Clamp(y - PRP_changeY, PRP_bot, PRP_top);

                        //Front
                        vertices = new Vector3[]
                        {
                            new Vector3(PRP_left, PRP_top, 0),
                            new Vector3(PRP_right, PRP_top, 0),
                            new Vector3(PRP_right, y, 0),
                            new Vector3(x, PRP_bot, 0),
                            new Vector3(PRP_left, PRP_bot, 0),
                        };
                        uv = CopyVerticesToUV(vertices);
                        triangles = new int[]
                        {
                            3, 4, 0,
                            0, 3, 2,
                            2, 1, 0
                        };
                        ApplyToFront(vertices, uv, triangles);

                        //Back
                        verticesBack = new Vector3[]
                        {
                            new Vector3(PRP_right, y, 0),
                            new Vector3(pointPull.x, pointPull.y, 0),
                            new Vector3(x, PRP_bot, 0)
                        };
                        uvBack = CopyVerticesToUV(verticesBack);
                        trianglesBack = new int[]
                        {
                            0,1,2
                        };
                        ApplyToBack(verticesBack, uvBack, trianglesBack); 
                    }
                    Debug.Log($"Size {x} - {y}");

                }

            }
            else if(Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                currentBack = null;
            }

    }

    public void Reset()
    {
        foreach(var b in backs) Destroy(b);
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
        currentBack.Clear(false);
        currentBack.vertices = vertices;
        currentBack.uv = uv;
        currentBack.triangles = triangles;
        currentBack.RecalculateNormals();
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

    public Vector2[] CopyVerticesToUV(Vector3[] p_vertices)
    {
        Vector2[] newUV = new Vector2[p_vertices.Length];
        for(int i = 0; i < p_vertices.Length; i++) newUV[i] = new Vector2(p_vertices[i].x + PRP_changeX, p_vertices[i].y + PRP_changeY);
        return newUV;
    }
}
