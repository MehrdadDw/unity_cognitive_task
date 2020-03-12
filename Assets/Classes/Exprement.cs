using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    public class Exprement
    {
        private Dictionary<int, bool> Result;
        public readonly MiniExprement _LeftExprement;
        public readonly MiniExprement _RightExprement;
        public bool _TrainMode { get; set; }
        public int level { get; set; }
      
        private int EndLimit_counter { get; set; }
        private bool last_is_correct { get; set; }
        private arrow flowSide { get; set; }
        private arrow Show_side { get; set; }
        private float _step { get; set; }
        private float _current_frac { get; set; }
        private readonly int _Max_Limit;
        public KeyCode Correct_keyCode()
        {
            return Show_side.Answer_keycode();
        }
        public Exprement(float start_frac, GameObject dot_prefab, int tot_dots,int Max_Limit, float scale,bool isTrain,bool debug=false)
        {
            Result = new Dictionary<int, bool>();
            _LeftExprement = new MiniExprement(dot_prefab, tot_dots, .6f,true, debug);
            _RightExprement = new MiniExprement(dot_prefab, tot_dots, .6f,false, debug);
            level = 0;
            _step = .03f;
            _Max_Limit = Max_Limit;
            _current_frac = start_frac;
            _TrainMode = isTrain;
            //initiate
            next_level(true);
        }
        public bool IsRoofOK()
        {
            return EndLimit_counter <= _Max_Limit;
        }
        public void button_submit(bool is_correct_answer)
        {

            if (_TrainMode)
            {
                if (!is_correct_answer )
                {
                    //Inverse detected!
                    EndLimit_counter++;
                    Debug.Log($"inverse {EndLimit_counter}");
                } 
            }
            else
            {
                if (is_correct_answer != last_is_correct)
                {
                    //Inverse detected!
                    EndLimit_counter++;
                    Debug.Log($"inverse {EndLimit_counter}");
                }
            }
            if (EndLimit_counter > _Max_Limit)
            {
                Debug.LogWarning("level done");
                if (!_TrainMode)
                FileUtil.save_result(Result, Save.Name);
                _LeftExprement.Finish();
                _RightExprement.Finish();
            }
            last_is_correct = is_correct_answer;


        }
        /// <summary>
        /// Graphic and logic new lvl
        /// </summary>
        /// <param name="Correct"></param>
        public void next_level(bool Correct)
        {
            Result.Add(level, Correct);

            flowSide = (arrow)(UnityEngine.Random.Range(0, 2));
             Show_side =(arrow) (UnityEngine.Random.Range(0, 2));
            //TODO factor 3 change
            if (Correct)
                _current_frac =Mathf.Clamp(_current_frac- _step, _step,1);
            else//wrong
                _current_frac += 3 * _step;

            Debug.LogWarning($"farc is {_current_frac}");
            switch (flowSide)
            {
                case arrow.left:
                    if (Show_side == arrow.left)
                    {
                        _LeftExprement.Start_new(_current_frac, flowSide);
                        _RightExprement.Start_new(0, flowSide);
                    }
                    else
                    {
                        _RightExprement.Start_new(_current_frac, flowSide);
                        _LeftExprement.Start_new(0, flowSide);
                    }

                    break;
                case arrow.right:
                    if (Show_side ==arrow.right)
                    {
                        _RightExprement.Start_new(_current_frac, flowSide);
                        _LeftExprement.Start_new(0, flowSide); 
                    }
                    else
                    {
                        _LeftExprement.Start_new(_current_frac, flowSide);
                        _RightExprement.Start_new(0, flowSide);
                    }
                    break;
                default:
                    Debug.LogError("step is not set");
                    break;
            }
            if (_current_frac < 0)
                _current_frac = 0;
            else if (_current_frac > 1)
                _current_frac = 1;
            
            level++;

            button_submit(Correct);

        }


    }
}
