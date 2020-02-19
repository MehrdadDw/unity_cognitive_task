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
        public Exprement(float start_frac, GameObject dot_prefab, int tot_dots, float scale)
        {
            _LeftExprement = new MiniExprement(dot_prefab, tot_dots, .25f, (1f / 8f), (5f / 8f));
            _RightExprement = new MiniExprement(dot_prefab, tot_dots, .25f, (5f / 8f), (5f / 8f));
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
        public void submit_answer(arrow user_answer)
        {
            var is_correct_answer = current_answer == user_answer;
            if (is_correct_answer != last_is_correct)
            {
                //Inverse detected!
                Inverse_counter++;
            }
            if (Inverse_counter == Max_inverse)
            {
                Debug.LogWarning("level done");
            }
            else
            {
                next_level(is_correct_answer);
            }


        }
        /// <summary>
        /// Graphic and logic new lvl
        /// </summary>
        /// <param name="Correct"></param>
        public void next_level(bool Correct)
        {
            current_answer = (arrow)(UnityEngine.Random.Range(0, 2));
            //TODO factor 3 change
            if (Correct)
                _current_frac -= _step;
            else//wrong
                _current_frac += 3 * _step;

            switch (current_answer)
            {
                case arrow.left:
                    _LeftExprement.Start_new(_current_frac, current_answer);
                    break;
                case arrow.right:
                    _RightExprement.Start_new(_current_frac, current_answer);
                    break;
                default:
                    Debug.LogError("step is not set");
                    break;
            }
            level++;
        }


    }
}
