using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
   
    public static class ExtentionHelper
    {
        public static Vector2 Arrow2direction(this arrow Arrow)
        {
            switch (Arrow)
            {
                case arrow.left:
                    return new Vector2(-1, 0);
                case arrow.right:
                    return new Vector2(1, 0);

               
                default:
                    Debug.Log("arrow not set");
                    return UnityEngine.Vector2.zero;

            }
        }
        public static KeyCode Answer_keycode(this arrow Arrow)
        {
            switch (Arrow)
            {
                case arrow.left:
                    return KeyCode.LeftArrow;
                
                case arrow.right:
                    return KeyCode.RightArrow;

                  
              

                   
                default:
                    Debug.Log("not relevant key");
                    return KeyCode.Space;
                    

            }
        }
    }
}
