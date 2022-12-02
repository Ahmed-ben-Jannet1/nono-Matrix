using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class video : MonoBehaviour
{
    private float startWait = 21f;
    public GameObject videoo;
    public GameObject musicaaa;
    public GameObject popa;


    void Start()
    {


        StartCoroutine(ExecuteAfterTime(4f));
        Invoke("Initialize", startWait);
    }

    void Initialize()
    {
        // Your code
        videoo.SetActive(false);
        popa.SetActive(true);


    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        musicaaa.SetActive(true);
    }
}