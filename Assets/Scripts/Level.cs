using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject back;
    public Vector2 condition;

    void Update()
    {
        if(isIn(back.transform.position, condition, 0.2f))
        {
            Debug.Log("Win");
        }

    }

    bool isIn(Vector2 position, Vector2 posIn, float radius)
    {
        if((position - posIn).magnitude <= radius)return true;
        else return false;
    }
}