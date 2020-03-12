using Assets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniExprement
{
    private GameObject _dot_prefab;
    private int tot_num;


    public List<GameObject> dot_objcts;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    private readonly int _totalDots;
    float _scale;
    bool _debug;
    bool left_position;
    public MiniExprement(GameObject dot_prefab, int total_dots, float scale, bool left,bool debug=false)
    {
        dot_objcts = new List<GameObject>();
        _dot_prefab = dot_prefab;
        _totalDots = total_dots;
        _scale = scale;
        left_position = left;
        set_bounds();
        _debug = debug;
    }

    public void set_bounds()
    {

        var vertExtent = Camera.main.GetComponent<Camera>().orthographicSize * _scale;
        var horzExtent = (vertExtent * Screen.width / Screen.height) ;
        var _x_offset = ((1 - _scale) * horzExtent) / 2;
        var _y_offset = (vertExtent) ;

        // Calculations assume map is position at the origin
        if (left_position)
            minX = -_x_offset + (float)(-horzExtent);
        else
            minX = _x_offset ;

        maxX = minX + horzExtent;
        maxY = _y_offset;
        minY = -_y_offset;

        //measure screen
        //lowerLeft.x will be the X coordinate of the leftmost visible pixel
        //leftbottom = Camera.main.ScreenToWorldPoint(new Vector3(minX, minY, 0));
        //lowerRight.x will be the X coordinate of the rightmost visible pixel
        //topright = Camera.main.ScreenToWorldPoint(new Vector3(maxX, maxY, 0));
    }
    public void Finish()
    {
        clear();
        Debug.Log("finished");
        SceneManager.LoadScene("Menu");
    }
    private void create_coherents(float coherent_dts_num, arrow Direction)
    {
        var const_speed_mag = Random.Range(3, 5);

        Debug.Log(Direction.ToString());
        for (int i = 0; i < coherent_dts_num; i++)
        {
            //random pos
            var const_x_pos = Random.Range((float)minX, (float)maxX);
            var const_y_pos = Random.Range((float)minY, (float)maxY);
            var dot = GameObject.Instantiate(_dot_prefab, new Vector3(const_x_pos, const_y_pos, 0), Quaternion.identity);

            dot.GetComponent<p_move>().Parent_Exprement = this;
            dot.GetComponent<p_move>().speed_vector = Direction.Arrow2direction();
            dot.GetComponent<p_move>().speed_mag = const_speed_mag;

            if (_debug)
                dot.GetComponent<Renderer>().material.color = Color.red;
            dot_objcts.Add(dot);

        }
    }

    private void create_random()
    {
        var const_x_speed = Random.Range(-10, 10);
        var const_y_speed = Random.Range(-10, 10);
        //random pos
        var x_pos = Random.Range((float)minX, (float)maxX);
        var y_pos = Random.Range((float)minY, (float)maxY);
        var dot = GameObject.Instantiate(_dot_prefab, new Vector3(x_pos, y_pos, 0), Quaternion.identity);

        dot.GetComponent<p_move>().Parent_Exprement = this;
        dot.GetComponent<p_move>().speed_vector = new Vector2(const_x_speed, const_y_speed);
        dot.GetComponent<p_move>().speed_mag = Random.Range(1, 5); ;
        dot_objcts.Add(dot);
    }
    public void Start_new(float Coherent_Frac, arrow folow_direction)
    {
        clear();
        Coherent_Frac = Mathf.Clamp(Coherent_Frac, 0, 1);
        var coherent_dts_num = _totalDots * Coherent_Frac;
        var random_dts_num = _totalDots - coherent_dts_num;
        for (int i = 0; i < random_dts_num; i++)
        {
            create_random();
        }
        create_coherents(coherent_dts_num, folow_direction);
    }
    private void clear()
    {
        foreach (var item in dot_objcts)
        {

            GameObject.Destroy(item);


        }
        dot_objcts = new List<GameObject>();
    }
}