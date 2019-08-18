using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUseMask3 : MonoBehaviour
{
    public GameObject OBJ_mask = null;
    public GameObject OBJ_front = null;
    public GameObject OBJ_back = null;
    public float frontSize = 1f;
    public bool isLimitDrag = true;
    public int orderSortForShow = 0;
    public int orderSortForHide = 0;

    GameObject mask;
    GameObject front;
    GameObject back;

    public enum TypeDrag {
        NONE,
        TOP,
        RIGHT,
        BOT,
        LEFT,
        TOP_LEFT,
        TOP_RIGHT,
        BOT_LEFT,
        BOT_RIGHT
    };
    TypeDrag drag = TypeDrag.NONE;
    Vector2 start;
    Vector2 origin;
    Vector2 maskScale;
    Vector2 maskPos;
    Vector2 backPos;
    float size;
    
    void Start()
    {
        front = OBJ_front;
        back = OBJ_back;
        mask = OBJ_mask;

        origin = front.transform.position;
        size = frontSize * front.transform.localScale.x;
        maskPos = mask.transform.position;
        

        back.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float halfSize = size * 0.5f;
        if(Input.GetMouseButtonDown(0))
        {
            if( mousePosition.x >= origin.x - halfSize && mousePosition.x <= origin.x + halfSize &&
                mousePosition.y >= origin.y - halfSize && mousePosition.y <= origin.y + halfSize)
            {
                float qsize = halfSize/2;
                if(mousePosition.x > origin.x - qsize && mousePosition.x < origin.x + qsize )
                {
                    if(mousePosition.y > origin.y + qsize) drag = TypeDrag.TOP;
                    else if(mousePosition.y < origin.y - qsize) drag = TypeDrag.BOT;
                } 
                else if(mousePosition.y > origin.y - qsize && mousePosition.y < origin.y + qsize)
                {
                    if(mousePosition.x > origin.x + qsize) drag = TypeDrag.RIGHT;
                    else if(mousePosition.x < origin.x - qsize) drag = TypeDrag.LEFT;
                }
                else if(mousePosition.x < origin.x - qsize)
                {
                    if(mousePosition.y > origin.y + qsize) drag = TypeDrag.TOP_LEFT;
                    else if(mousePosition.y < origin.y - qsize) drag = TypeDrag.BOT_LEFT;
                }
                else if(mousePosition.x > origin.x + qsize)
                {
                    if(mousePosition.y > origin.y + qsize) drag = TypeDrag.TOP_RIGHT;
                    else if(mousePosition.y < origin.y - qsize) drag = TypeDrag.BOT_RIGHT;
                }
                start = mousePosition;
                UpdateType();
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(drag != TypeDrag.NONE && mousePosition != start)
            {
                back.SetActive(true);
                Vector2 offset = mousePosition - start;
                if(drag == TypeDrag.TOP)
                {
                    float offsetY = offset.y;
                    if(isLimitDrag)
                        if(offsetY > 0)offsetY = 0;
                        else if(offsetY < -size)offsetY = -size;

                    Vector2 anchor = new Vector2(origin.x, origin.y + halfSize);
                    mask.transform.position = anchor + new Vector2(0, halfSize + offsetY/2);
                    back.transform.position = new Vector2(origin.x, origin.y + size + offsetY);
                }
                else if(drag == TypeDrag.RIGHT)
                {
                    float offsetX = offset.x;
                    if(isLimitDrag)
                        if(offsetX > 0)offsetX = 0;
                        else if(offsetX < -size)offsetX = -size;

                    Vector2 anchor = new Vector2(origin.x + halfSize, origin.y);
                    mask.transform.position = anchor + new Vector2(halfSize + offsetX/2, 0);
                    back.transform.position = new Vector2(origin.x + size + offsetX, origin.y);
                }
                else if(drag == TypeDrag.BOT)
                {
                    float offsetY = offset.y;
                    if(isLimitDrag)
                        if(offsetY < 0)offsetY = 0;
                        else if(offsetY > size)offsetY = size;

                    Vector2 anchor = new Vector2(origin.x, origin.y - halfSize);
                    mask.transform.position = anchor - new Vector2(0, halfSize - offsetY/2);
                    back.transform.position = new Vector2(origin.x, origin.y - size + Mathf.Abs(offsetY));
                }
                else if(drag == TypeDrag.LEFT)
                {
                    float offsetX = offset.x;
                    if(isLimitDrag)
                        if(offsetX < 0)offsetX = 0;
                        else if(offsetX > size)offsetX = size;

                    Vector2 anchor = new Vector2(origin.x - halfSize, origin.y);
                    mask.transform.position = anchor - new Vector2(halfSize - offsetX/2, 0);
                    back.transform.position = new Vector2(origin.x - size + offsetX, origin.y);
                }
                else if(drag == TypeDrag.TOP_LEFT)
                {
                    float angle = -45;
                    float dy = Mathf.Abs(offset.magnitude);
                    if(isLimitDrag && dy > size)dy = size;
                    float curSize = size * 2;
                    Vector2 anchor = new Vector3(origin.x - halfSize, origin.y + halfSize);
                    mask.transform.position = anchor - new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * size, Mathf.Sin(angle * Mathf.Deg2Rad) * size) + new Vector2(dy, -dy) / 2;
                    mask.transform.localEulerAngles = Vector3.forward * angle;

                    back.transform.position = new Vector2(origin.x - size + dy, origin.y + size - dy);
                    back.transform.eulerAngles = Vector3.forward * -90;
                }
                else if(drag == TypeDrag.TOP_RIGHT)
                {
                    float angle = 45;
                    float dy = Mathf.Abs(offset.magnitude);
                    if(isLimitDrag && dy > size)dy = size;
                    float curSize = size * 2;
                    Vector2 anchor = new Vector3(origin.x + halfSize, origin.y + halfSize);
                    mask.transform.position = anchor + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * size, Mathf.Sin(angle * Mathf.Deg2Rad) * size) + new Vector2(-dy, -dy) / 2;
                    mask.transform.localEulerAngles = Vector3.forward * angle;

                    back.transform.position = new Vector2(origin.x + size - dy, origin.y + size - dy);
                    back.transform.eulerAngles = Vector3.forward * 90;
                }
                else if(drag == TypeDrag.BOT_LEFT)
                {
                    float angle = 45;
                    float dy = Mathf.Abs(offset.magnitude);
                    if(isLimitDrag && dy > size)dy = size;
                    float curSize = size * 2;
                    Vector2 anchor = new Vector3(origin.x - halfSize, origin.y - halfSize);
                    mask.transform.position = anchor - new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * size, Mathf.Cos(angle * Mathf.Deg2Rad) * size) + new Vector2(dy, dy) / 2;
                    mask.transform.localEulerAngles = Vector3.forward * angle;

                    back.transform.position = new Vector2(origin.x - size + dy, origin.y - size + dy);
                    back.transform.eulerAngles = Vector3.forward * -90;                    
                }
                else if(drag == TypeDrag.BOT_RIGHT)
                {
                    float angle = -45;
                    float dy = Mathf.Abs(offset.magnitude);
                    if(isLimitDrag && dy > size)dy = size;
                    float curSize = size * 2;
                    Vector2 anchor = new Vector3(origin.x + halfSize, origin.y - halfSize);
                    mask.transform.position = anchor - new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * size, Mathf.Cos(angle * Mathf.Deg2Rad) * size) + new Vector2(-dy, dy) / 2;
                    mask.transform.localEulerAngles = Vector3.forward * angle;

                    back.transform.position = new Vector2(origin.x + size - dy, origin.y - size + dy);
                    back.transform.eulerAngles = Vector3.forward * 90;    
                }
                
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            drag = TypeDrag.NONE;
            NewTurn();
        }
    }

    void NewTurn()
    {
        orderSortForShow++;
        orderSortForHide++;
        mask = Instantiate(OBJ_mask);
        mask.transform.position = maskPos;
        mask.transform.eulerAngles = Vector3.zero;
        mask.transform.localScale = new Vector2(1, 1);
        // mask.GetComponent<SpriteMask>().frontSortingOrder = orderSortForShow;
        // mask.GetComponent<SpriteMask>().backSortingOrder = orderSortForHide;

        back = Instantiate(OBJ_back);
        back.transform.position = backPos;
        back.SetActive(false);
        // back.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        // back.GetComponent<SpriteRenderer>().sortingOrder = orderSortForShow;

        front.transform.position = origin;
    }

    void Flip(bool x, bool y)
    {
        var sprite = back.GetComponent<SpriteRenderer>();
        sprite.flipX = x;
        sprite.flipY = y;
    }

    void UpdateType()
    {
        switch(drag)
        {
            case TypeDrag.TOP:
                Flip(false, true);
                mask.transform.localScale = new Vector2(1, 1);
                mask.transform.eulerAngles = Vector3.zero;
            break;
            case TypeDrag.RIGHT:
                Flip(true, false);
                mask.transform.localScale = new Vector2(1, 1);
                mask.transform.eulerAngles = Vector3.zero;
            break;
            case TypeDrag.BOT:
                Flip(false, true);
                mask.transform.localScale = new Vector2(1, 1);
                mask.transform.eulerAngles = Vector3.zero;
            break;
            case TypeDrag.LEFT:
                Flip(true, false);
                mask.transform.localScale = new Vector2(1, 1);
                mask.transform.eulerAngles = Vector3.zero;
            break;
            case TypeDrag.TOP_LEFT:
                Flip(true, false);
                mask.transform.localScale = new Vector2(2, 2);
            break;
            case TypeDrag.TOP_RIGHT:
                Flip(true, false);
                mask.transform.localScale = new Vector2(2, 2);
            break;
            case TypeDrag.BOT_LEFT:
                Flip(true, true);
                mask.transform.localScale = new Vector2(2, 2);
            break;
            case TypeDrag.BOT_RIGHT:
                Flip(true, true);
                mask.transform.localScale = new Vector2(2, 2);
            break;
            default:
            break;
        }
    }
}
