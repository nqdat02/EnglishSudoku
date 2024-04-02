using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button EasyBtn, MediumBtn, HardBtn;

    public void ClickOnEasy()
    {
        SceneManager.LoadScene("Game");
        Setting.EasyMediumHard_Number = 1;
    }
    public void ClickOnMedium()
    {
        SceneManager.LoadScene("Game");
        Setting.EasyMediumHard_Number = 2;

    }
    public void ClickOnHard()
    {
        SceneManager.LoadScene("Game");
        Setting.EasyMediumHard_Number = 3;
    }
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
}
