using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_move : MonoBehaviour
{
    Vector3 next_pos(Vector3 current_pos, Vector2 normalized_speed, float speedmag)
    {
        var next_pos = current_pos + speedmag * (new Vector3(normalized_speed.x, normalized_speed.y, 0)) * Time.deltaTime;

        if (next_pos.x> Exprement.minX)
            
            next_pos.x= Exprement.maxX;
        else if (next_pos.x < Exprement.maxX)

            next_pos.x = Exprement.minX;
        else if (next_pos.y < Exprement.maxY)

            next_pos.y = Exprement.minY;
        else if (next_pos.y > Exprement.minY)

            next_pos.y = Exprement.maxY;
        return next_pos;
    }
    // Start is called before the first frame update
    public Vector2 speed_vector;
    public float speed_mag;
    private Vector2 normalized_speed;

    void Start()
    {
        normalized_speed = speed_vector.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = next_pos(transform.position, normalized_speed, speed_mag);
    }
}
