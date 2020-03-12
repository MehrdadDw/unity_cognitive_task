using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu_manager : MonoBehaviour
{
    public GameObject Input;
  public  void start_Train()
    {

        SceneManager.LoadScene("Train");
    }
   public void start_Task()
    {
        if (Input.GetComponent<InputField>().text == "")
        {
            Input.GetComponent<InputField>().Select();
            Input.GetComponent<InputField>().ActivateInputField();
            return;
        }
        Save.Name = Input.GetComponent<InputField>().text;
        SceneManager.LoadScene("Task");

    }
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(Save.Name))
            Input.GetComponent<InputField>().text = Save.Name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
