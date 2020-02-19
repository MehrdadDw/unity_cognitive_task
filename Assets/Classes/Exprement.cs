using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Exprement
{
    static GameObject dot_prefab;
    static int tot_num;
    static float coherent_frac;
    public static arrow answer;
    public static List<GameObject> dot_objcts = new List<GameObject>();
    public static float minX;
    public static float maxX;
    public static float minY;
    public static float maxY;
    public static void set_screen_bounds()
    {
        var mapX = 0;
        var mapY = 0;
        var vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = (float)(horzExtent - mapX / 2.0);
        maxX = (float)(mapX / 2.0 - horzExtent);
        minY = (float)(vertExtent - mapY / 2.0);
        maxY = (float)(mapY / 2.0 - vertExtent);

        //measure screen
        //lowerLeft.x will be the X coordinate of the leftmost visible pixel
        //leftbottom = Camera.main.ScreenToWorldPoint(new Vector3(minX, minY, 0));

        //lowerRight.x will be the X coordinate of the rightmost visible pixel
        //topright = Camera.main.ScreenToWorldPoint(new Vector3(maxX, maxY, 0));



    }
    public static void Finish()
    {
        clear();
        Debug.Log("finished");
    }
    private static void create_coherents(float coherent_dts_num)
    {
     ;

      
        var const_speed_mag = Random.Range(3, 5);
        answer = (arrow)Random.Range(0, 4);
        Debug.Log(answer.ToString());
        for (int i = 0; i < coherent_dts_num; i++)
        {
            //random pos
            var const_x_pos = Random.Range((float)minX, (float)maxX);
            var const_y_pos = Random.Range((float)minY, (float)maxY);
            var dot = GameObject.Instantiate(dot_prefab, new Vector3(const_x_pos, const_y_pos, 0), Quaternion.identity);
            Exprement.dot_objcts.Add(dot);

            dot.GetComponent<p_move>().speed_vector = answer.Arrow2direction();
            dot.GetComponent<p_move>().speed_mag = const_speed_mag;
        }
    }

    private static void create_random()
    {
        var const_x_speed = Random.Range(-10, 10);
        var const_y_speed = Random.Range(-10, 10);
        //random pos
        var x_pos = Random.Range((float)minX, (float)maxX);
        var y_pos = Random.Range((float)minY, (float)maxY);
        var dot = GameObject.Instantiate(dot_prefab, new Vector3(x_pos, y_pos, 0), Quaternion.identity);
        Exprement.dot_objcts.Add(dot);
       
        dot.GetComponent<p_move>().speed_vector = new Vector2(const_x_speed,const_y_speed) ;
        dot.GetComponent<p_move>().speed_mag = Random.Range(1, 5); ;
    }
    public static void Start_new(int tot_dots, float Coherent_Frac, GameObject dotprefab)
    {
        set_screen_bounds();
        clear();
        dot_prefab = dotprefab;
        tot_num = tot_dots;
        coherent_frac = Coherent_Frac;

        var coherent_dts_num = tot_num * Coherent_Frac;
        var random_dts_num = tot_num - coherent_dts_num;
        for (int i = 0; i < random_dts_num; i++)
        {
            create_random();
        }
        create_coherents(coherent_dts_num);
    }
    private static void clear()
    {
        foreach (var item in dot_objcts)
        {
            try
            {
                GameObject.Destroy(item);
            }
            catch (System.Exception)
            {

            }
        }

    }
    

    static Vector2 Arrow2direction(this arrow Arrow)
    {
        switch (Arrow)
        {
            case arrow.left:
                return new Vector2(-1, 0);
            case arrow.right:
                return new Vector2(1, 0);

            case arrow.up:
                return new Vector2(0, 1);

            case arrow.down:
                return new Vector2(0, -1);

            default:
                Debug.Log("arrow not set");
                return Vector2.zero;

        }
    }
   public static KeyCode Answer_keycode()
    {
        switch (Exprement.answer)
        {
            case arrow.left:
                return KeyCode.LeftArrow;
                break;
            case arrow.right:
                return KeyCode.RightArrow;

                break;
            case arrow.up:
                return KeyCode.UpArrow;

                break;
            case arrow.down:
                return KeyCode.DownArrow;

                break;
            default:
                Debug.Log("not relevant key");
                return KeyCode.Space;
                break;

        }
    }
}
public enum arrow
{
    left, right,up,down
}



