using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileUtil : MonoBehaviour
{
    public static void save_result(Dictionary<int, bool> result, string User_name = "")
    {
        string path = $"Resources/{(string.IsNullOrEmpty(User_name)?"":User_name+"_")}{DateTime.Now.ToString("hh-MM_dd-mm-yyyy")}.csv";
        //Write some text to the test.txt file
        File.WriteAllLines(path, result.Select(x => x.Key + ";\t" + x.Value));
    }
}
