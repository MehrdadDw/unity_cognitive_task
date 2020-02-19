using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_move : MonoBehaviour
{
    Vector3 next_pos(Vector3 current_pos, Vector2 normalized_speed, float speedmag)
    {
        var next_pos = current_pos + speedmag * (new Vector3(normalized_speed.x, normalized_speed.y, 0)) * Time.deltaTime;

        if (next_pos.x> Parent_Exprement.maxX)
            
            next_pos.x= Parent_Exprement.minX;
        else if (next_pos.x < Parent_Exprement.minX)

            next_pos.x = Parent_Exprement.maxX;
        else if (next_pos.y < Parent_Exprement.minY)

            next_pos.y = Parent_Exprement.maxY;
        else if (next_pos.y > Parent_Exprement.maxY)

            next_pos.y = Parent_Exprement.minY;
        return next_pos;
    }
    // Start is called before the first frame update
    public Vector2 speed_vector;
    public float speed_mag;
    private Vector2 normalized_speed;
    public MiniExprement Parent_Exprement;
    void Start()
    {
        normalized_speed = speed_vector.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (null!= Parent_Exprement)
        transform.position = next_pos(transform.position, normalized_speed, speed_mag);
    }
}
