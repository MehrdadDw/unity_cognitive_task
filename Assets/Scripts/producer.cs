using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class producer : MonoBehaviour
{
    public bool is_Train;
    public bool is_Debug;
    public GameObject dot_prefab;
    public int total_dots;
    public int interval_sec;
    public float start_frac;
    public float step_add_frac;
    public float scale;
    public int Max_Limit_Train;
    public int Max_Limit_Task;
    private Image _Blur_Image;
    public GameObject Blur_Image_obj;

    private Vector3 topright;
    private Vector3 leftbottom;
    private int duration_sec;


    private bool finished;
    private Dictionary<int, bool> Result;
    private int current_lvl;
    private DateTime next_time;


    private Exprement Exprement;
    private MiniExprement Right_Exprement;
    // Start is called before the first frame update
    void Start()
    {
        LoadConfig();
        if (is_Train)
            Exprement = new Exprement(start_frac, dot_prefab, total_dots, Max_Limit_Train, scale, is_Train, is_Debug);
        else
            Exprement = new Exprement(start_frac, dot_prefab, total_dots, Max_Limit_Task, scale, is_Train, is_Debug);

        Result = new Dictionary<int, bool>();
        next_time = DateTime.Now;
        finished = false;
        current_lvl = 0;

        _Blur_Image = Blur_Image_obj.GetComponent<Image>();
    }

    void LoadConfig()
    {
        try
        {
            string path = $"D:\\config.txt";
            //Read the text from directly from the test.txt file
            var lines = File.ReadLines(path).ToList();
            Debug.Log(string.Join("", lines));

            Dictionary<string, string> config = lines.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Split()).ToDictionary(x => x[0], x => x[1]);
            //TODO read config file
            int.TryParse(config["total_dots"], out int Rtotal_dots);
            int.TryParse(config["interval_sec"], out int Rinterval_sec);
            float.TryParse(config["start_frac"], out float Rstart_frac);
            float.TryParse(config["step_add_frac"], out float Rstep_add_frac);
            float.TryParse(config["last_frac"], out float Rlast_frac);

            bool.TryParse(config["is_Debug"], out bool Ris_Debug);
            int.TryParse(config["Max_Limit_Train"], out int RMax_Limit_Train);
            int.TryParse(config["Max_Limit_Task"], out int RMax_Limit_Task);

            total_dots = Rtotal_dots;
            interval_sec = Rinterval_sec;
            start_frac = Rstart_frac;
            step_add_frac = Rstep_add_frac;
            Max_Limit_Task = RMax_Limit_Task;
            Max_Limit_Train = RMax_Limit_Train;

            is_Debug = Ris_Debug;


        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
      

    }
    private IEnumerator blink_blur()
    {
        for (int i = 0; i < 3; i++)
        {
            _Blur_Image.enabled = false;
            yield return new WaitForSeconds(.01f);

            _Blur_Image.enabled = true;
            yield return new WaitForSeconds(.04f);
            _Blur_Image.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Exprement == null) return;
        //time out
        if (DateTime.Now > next_time && Exprement.IsRoofOK())
        {
            if (Exprement.level > 0)
            {
                Result.Add(Exprement.level, false);
            }
            next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
            Exprement.next_level(false);
        }
        if (Input.anyKeyDown && Exprement.IsRoofOK())
        {
            if (Input.GetKeyDown(Exprement.Correct_keyCode()))
            {
                //correct
                Debug.Log($"true: {Exprement.Correct_keyCode().ToString()} is pressed");
                Result.Add(Exprement.level, true);
                current_lvl++;
                next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
                Exprement.next_level(true);

            }
            else
            {

                if (is_Train)
                    StartCoroutine(blink_blur());
                //pressed wrong
                if (current_lvl > 0)
                {
                    Result.Add(Exprement.level, false);
                }
                current_lvl++;
                next_time = DateTime.Now + TimeSpan.FromSeconds(interval_sec);
                Exprement.next_level(false);


                Debug.Log($"irrelevant key pressed");
            }
        }
        if (DateTime.Now > next_time && Exprement.IsRoofOK() & !finished)
        {
            //Exprement.Finish();
            //FileUtil.save_result(Result, "Username");
            finished = true;
            SceneManager.LoadScene("Menu");

        }

    }
}
