using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Slider slider;
    public Text sliderText;
    public GameObject bloc;
    public GameObject img;
    public GameObject sounding;
    public GameObject Helpp;
    private static bool musicon;
    public GameObject backgMusic;
    public GameObject Sm;
    public GameObject Hm;
    public GameObject vid;
    public GameObject skipbut;


    public GameObject handsText;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Execute(23f));
        musicon = true;

    
       
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void Blocc()
    {
        bloc.SetActive(true);
        img.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }


    public void Player()
    {
        PlayerPrefs.SetInt("Row", 10);
        PlayerPrefs.SetInt("Column", 10);

        SceneManager.LoadScene("Game");
    }

    public void Player02()
    {
        PlayerPrefs.SetInt("Row", 20);
        PlayerPrefs.SetInt("Column", 20);

        SceneManager.LoadScene("Game");
    }

    public void Sound()
    {
        sounding.SetActive(true);
        
    }

    public void Help()
    {
        Helpp.SetActive(true);

    }

    public void Hardness()
    {
        Hm.SetActive(true);

    }

    public void Settmenu()
    {
        Sm.SetActive(true);

    }

    public void muting()
    {
        if (musicon == true)
        {
            backgMusic.SetActive(false);
            musicon = false;
        }
        else
        {
            backgMusic.SetActive(true);
            musicon = true;
        }
    }

    IEnumerator Execute(float time)
    {
        yield return new WaitForSeconds(time);

        handsText.SetActive(true);
        skipbut.SetActive(false);
    }

    public void skip()
    {
        img.SetActive(false);
        vid.SetActive(false);
        skipbut.SetActive(false);

    }

}
