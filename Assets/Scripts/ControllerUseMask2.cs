﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerUseMask2 : MonoBehaviour
{
    public GameObject mask;
    public GameObject front;
    public float frontSize = 1f;
    public GameObject back;

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
    float size;
    
    void Start()
    {
        origin = front.transform.position;
        size = frontSize * front.transform.localScale.x;
        maskScale = mask.transform.localScale;
        maskPos = mask.transform.localPosition;
        // maskPos = mask.transform.position;
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
                Debug.Log(drag);
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(drag != TypeDrag.NONE && mousePosition != start)
            {
                if(drag == TypeDrag.TOP)
                {

                }
                else if(drag == TypeDrag.RIGHT)
                {
                    
                }
                else if(drag == TypeDrag.BOT)
                {
                    
                }
                else if(drag == TypeDrag.LEFT)
                {
                    
                }
                else if(drag == TypeDrag.TOP_LEFT)
                {
                    
                }
                else if(drag == TypeDrag.TOP_RIGHT)
                {
                    
                }
                else if(drag == TypeDrag.BOT_LEFT)
                {
                    
                }
                else if(drag == TypeDrag.BOT_RIGHT)
                {
                    
                }
                
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            drag = TypeDrag.NONE;
        }
    }
}