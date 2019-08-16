using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUseMask : MonoBehaviour
{
    public RectTransform front;
    public RectTransform mask;
    public RectTransform back;
    public Canvas canvas;

    public enum TypeDrag {
        NONE,
        TOP_LEFT,
        TOP_RIGHT,
        BOT_LEFT,
        BOT_RIGHT
    };
    TypeDrag drag = TypeDrag.NONE;
    Vector2 start;

    float frontX;
    float frontY;
    float frontW;
    float frontH;

    void Start()
    {
        frontX = front.transform.position.x;
        frontY = front.transform.position.y;
        frontW = front.rect.width * canvas.scaleFactor;
        frontH = front.rect.height * canvas.scaleFactor;
    }

    public void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        if(Input.GetMouseButtonDown(0))
        {
            if( mousePosition.x >= frontX - frontW/2 && mousePosition.x <= frontX + frontW/2 &&
                mousePosition.y >= frontY - frontH/2 && mousePosition.y <= frontY + frontH/2)
            {
                if(mousePosition.x < frontX && mousePosition.y > frontY) drag = TypeDrag.TOP_LEFT;
                else if(mousePosition.x > frontX && mousePosition.y > frontY) drag = TypeDrag.TOP_RIGHT;
                else if(mousePosition.x < frontX && mousePosition.y < frontY) drag = TypeDrag.BOT_LEFT;
                else if(mousePosition.x > frontX && mousePosition.y < frontY) drag = TypeDrag.BOT_RIGHT;
                start = mousePosition;
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(drag != TypeDrag.NONE && mousePosition != start)
            {
                Vector2 offset = mousePosition - start;
                // Vector2 offset = mousePosition - new Vector2(frontX - frontW/2, frontY - frontH/2);
                float angle = Mathf.Atan2(offset.x, offset.y);
                mask.transform.eulerAngles = Vector3.forward * (angle * Mathf.Rad2Deg);
                float distanceToMask = Mathf.Sin(angle) * offset.y;

                if(drag == TypeDrag.TOP_LEFT)
                {
                    
                }
                else if(drag == TypeDrag.TOP_RIGHT)
                {
                    
                }
                else if(drag == TypeDrag.BOT_LEFT)
                {
                    mask.pivot = new Vector2(0, 0.5f);
                    Vector2 maskPos = mousePosition - new Vector2(offset.x, offset.y) * 0.5f;
                    // Vector2 maskPos = mousePosition;
                    mask.transform.position = maskPos;

                    back.pivot = new Vector2(1f, 1f);
                    back.transform.position = mousePosition;
                }
                else if(drag == TypeDrag.BOT_RIGHT)
                {
                    
                }

                start = mousePosition;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            drag = TypeDrag.NONE;
        }
    }

   

    void LateUpdate()
    {
        front.eulerAngles = Vector3.zero;
        front.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);

        // Vector3 pos = front.localPosition;
        // float theta = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;

        // if(theta <= 0f || theta >= 90f) return;

        
    }
}
