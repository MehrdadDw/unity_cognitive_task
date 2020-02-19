using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class producer : MonoBehaviour
{
    public GameObject dot_prefab;
    public int total_dots;
    public int interval_sec;
    public float start_frac;
    public float step_add_frac;
    public float scale;


    private Vector3 topright;
    private Vector3 leftbottom;
    private int duration_sec;


    private bool finished;
    private List<Tuple<int, bool>> Result;
    private int current_lvl;
    private DateTime next_time;
    

    private Exprement Exprement;
    private MiniExprement Right_Exprement;
    // Start is called before the first frame update
    void Start()
    {
        Exprement = new Exprement(start_frac,dot_prefab,total_dots,scale);
     
        LoadConfig();
        Result = new List<Tuple<int, bool>>();
        next_time = DateTime.Now;
        finished = false;
        current_lvl = 0;
        
    }
   
    void LoadConfig()
    {
        string path = "Resources/config.txt";

        //Read the text from directly from the test.txt file
        var lines = File.ReadLines(path).ToList();
        Debug.Log(string.Join("", lines));

        Dictionary<string, string> config = lines.Select(x => x.Split()).ToDictionary(x => x[0], x => x[1]);
        //TODO read config file
        //int.TryParse(config["total_dots"], out int Rtotal_dots);
        //int.TryParse(config["interval_sec"], out int Rinterval_sec);
        //float.TryParse(config["start_frac"], out float Rstart_frac);
        //float.TryParse(config["step_add_frac"], out float Rstep_add_frac);
        //float.TryParse(config["last_frac"], out float Rlast_frac);

        //total_dots = Rtotal_dots;
        //interval_sec = Rinterval_sec;
        //start_frac = Rstart_frac;
        //step_add_frac = Rstep_add_frac;
        

    }


    // Update is called once per frame
    void Update()
    {
        //time out
        if (DateTime.Now > next_time && Exprement.IsInversionOK() )
        {
            if (Exprement.level>0 )
            {
                Result.Add(Tuple.Create( Exprement.level, false));
            }
            next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
            Exprement.next_level(false);
        }
        if (Input.anyKeyDown && Exprement.IsInversionOK())
        {
            if (Input.GetKeyDown(Exprement.Correct_keyCode()))
            {
                //correct
                Debug.Log($"true: {Exprement.Correct_keyCode().ToString()} is pressed");
                Result.Add(Tuple.Create(Exprement.level, true));
                current_lvl++;
                next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
                Exprement.next_level(true);
               
            }
            else
            {
                //pressed wrong
                if (current_lvl > 0)
                {
                    Result.Add(Tuple.Create(Exprement.level, false));
                }
                current_lvl++;
                next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
                Exprement.next_level(false);
                

                Debug.Log($"irrelevant key pressed");
            }
        }
        if (DateTime.Now > next_time && Exprement.IsInversionOK() & !finished)
        {
            //Exprement.Finish();
           // FileUtil.save_result(Result, "Username");
            finished = true;
        }

    }
}
