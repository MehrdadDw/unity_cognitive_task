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
        public int level { get; set; }
        private int Inverse_counter { get; set; }
        private bool last_is_correct { get; set; }
        private arrow current_answer { get; set; }
        private float _step { get; set; }
        private float _current_frac { get; set; }
        private readonly int Max_inverse;
        public KeyCode Correct_keyCode()
        {
            return current_answer.Answer_keycode();
        }
        public Exprement(float start_frac, GameObject dot_prefab, int tot_dots, float scale,bool debug=false)
        {
            Result = new Dictionary<int, bool>();
            _LeftExprement = new MiniExprement(dot_prefab, tot_dots, .6f,true, debug);
            _RightExprement = new MiniExprement(dot_prefab, tot_dots, .6f,false, debug);
            level = 0;
            _step = .03f;
            Max_inverse = 8;
            _current_frac = start_frac;
            //initiate
            next_level(true);
        }
        public bool IsInversionOK()
        {
            return Inverse_counter < Max_inverse;
        }
        public void button_submit(bool is_correct_answer)
        {
            
            if (is_correct_answer != last_is_correct)
            {
                //Inverse detected!
                Inverse_counter++;
                Debug.Log($"inverse {Inverse_counter}");
            }
            if (Inverse_counter == Max_inverse)
            {
                Debug.LogWarning("level done");
                FileUtil.save_result(Result, "Username");
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

            current_answer = (arrow)(UnityEngine.Random.Range(0, 2));
            var Show_side = (UnityEngine.Random.Range(0, 2));
            //TODO factor 3 change
            if (Correct)
                _current_frac -=Mathf.Clamp(_current_frac, _step,1);
            else//wrong
                _current_frac += 3 * _step;


            switch (current_answer)
            {
                case arrow.left:
                    if (Show_side == 0)
                    {
                        _LeftExprement.Start_new(_current_frac, current_answer);
                        _RightExprement.Start_new(0, current_answer);
                    }
                    else
                    {
                        _RightExprement.Start_new(_current_frac, current_answer);
                        _LeftExprement.Start_new(0, current_answer);
                    }

                    break;
                case arrow.right:
                    if (Show_side == 0)
                    {
                        _RightExprement.Start_new(_current_frac, current_answer);
                        _LeftExprement.Start_new(0, current_answer); 
                    }
                    else
                    {
                        _LeftExprement.Start_new(_current_frac, current_answer);
                        _RightExprement.Start_new(0, current_answer);
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
