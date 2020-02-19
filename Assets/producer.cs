using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
]
public class producer : MonoBehaviour
{

    public GameObject dot_prefab;
    public int total_dots;
    public int interval_sec;
    public float start_frac;
    public float step_add_frac;
    public float last_frac;


    private Vector3 topright;
    private Vector3 leftbottom;
    private int duration_sec;


    private bool finished;
    private Dictionary<int, bool> Result;
    private int current_lvl;
    private DateTime next_time;
    private int total_lvls;
    // Start is called before the first frame update
    void Start()
    {
        LoadConfig();
        Result = new Dictionary<int, bool>();
        next_time = DateTime.Now;
        finished = false;
        current_lvl = 0;
        total_lvls = (int)((last_frac - start_frac) / step_add_frac);
    }
   
    void LoadConfig()
    {
        string path = "Resources/config.txt";

        //Read the text from directly from the test.txt file
        var lines = File.ReadLines(path).ToList();
        Debug.Log(string.Join("", lines));

        Dictionary<string, string> config = lines.Select(x => x.Split()).ToDictionary(x => x[0], x => x[1]);

        int.TryParse(config["total_dots"], out int Rtotal_dots);
        int.TryParse(config["interval_sec"], out int Rinterval_sec);
        float.TryParse(config["start_frac"], out float Rstart_frac);
        float.TryParse(config["step_add_frac"], out float Rstep_add_frac);
        float.TryParse(config["last_frac"], out float Rlast_frac);

        total_dots = Rtotal_dots;
        interval_sec = Rinterval_sec;
        start_frac = Rstart_frac;
        step_add_frac = Rstep_add_frac;
        last_frac = Rlast_frac;

    }


    // Update is called once per frame
    void Update()
    {
        if (DateTime.Now > next_time && total_lvls > current_lvl)
        {
            if (current_lvl>0 && !Result.ContainsKey(current_lvl))
            {
                Result.Add(current_lvl, false);
            }
            current_lvl++;
            next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
            Exprement.Start_new(total_dots, start_frac + step_add_frac * current_lvl, dot_prefab);
           

        }
        if (Input.anyKeyDown && total_lvls >= current_lvl)
        {
            if (Input.GetKeyDown(Exprement.Answer_keycode()))
            {
                Debug.Log($"true: {Exprement.answer.ToString()} is pressed");
                Result.Add(current_lvl, true);
                current_lvl++;
                next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
                Exprement.Start_new(total_dots, start_frac + step_add_frac * current_lvl, dot_prefab);
            }
            else
            {
                if (current_lvl > 0 && !Result.ContainsKey(current_lvl))
                {
                    Result.Add(current_lvl, false);
                }
                current_lvl++;
                next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
                Exprement.Start_new(total_dots, start_frac + step_add_frac * current_lvl, dot_prefab);
                Debug.Log($"irrelevant key pressed");
            }
        }
        if (DateTime.Now > next_time && total_lvls <= current_lvl & !finished)
        {
            Exprement.Finish();
            FileUtil.save_result(Result, "Username");
            finished = true;
        }

    }
}
