using System.Collections;
using Assets.SimpleSlider.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public GameObject startImageDetectionButton;
    public GameObject startObjectMesButton;
    public GameObject[] otherUI;


    // Update is called once per frame
    void Update()
    {
        if(HorizontalScrollSnap.Instance.GetCurrentPage() == 2)
        {
           foreach(var ui in otherUI)
            {
                ui.SetActive(false);
            }

            startImageDetectionButton.SetActive(true);      
            startObjectMesButton.SetActive(true);
        }
    }

    public void OnNext()
    {
        HorizontalScrollSnap.Instance.SlideNext(); 
    } 

    public void OnSkip()
    {
        var calls = 2 - HorizontalScrollSnap.Instance.GetCurrentPage();

        HorizontalScrollSnap.Instance.SlideNext();
        HorizontalScrollSnap.Instance.SlideNext();
    }

    public void OnStartImageDetection()
    {
        Destroy(HorizontalScrollSnap.Instance);
        SceneManager.LoadScene("ImageDetection");
    }

    public void OnStartObjectMes()
    {
        Destroy(HorizontalScrollSnap.Instance);
        SceneManager.LoadScene("ObjectMesurement");
    }
}
