using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.SceneManagement;

public class ImageDetection : MonoBehaviour
{

    #region inspector's attributes
    [SerializeField]


    public GameObject[] objetsPrefabs;
    public Dictionary<string,GameObject> objects = new Dictionary<string, GameObject>();

    public TextMeshProUGUI imageName;
    public GameObject helper;
    #endregion

    ARTrackedImage currentImage;
    public ARTrackedImageManager m_TrackedImageManager;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;


    private void Awake()
    {
        if (objetsPrefabs != null)
        {
            for (int i = 0; i < objetsPrefabs.Length; i++) { 
            
                GameObject newGameObject = Instantiate(objetsPrefabs[i],Vector3.zero,Quaternion.identity);
                newGameObject.name = objetsPrefabs[i].name;
                newGameObject.SetActive(false);
                objects.Add(newGameObject.name,newGameObject);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        helper.SetActive(false);
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            Excute(trackedImage);

        }

        foreach (var trackedImage in eventArgs.updated)
        {
            Excute(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            Excute(trackedImage);
        }

    }


    private void ClearCurrentImage(ARTrackedImage trackedImage)
    {
        imageName.text = "Trying to find a target";
        objects[trackedImage.referenceImage.name].SetActive(false);
    }

    private void GetCurrentImage(ARTrackedImage trackedImage)
    {
        string currentName =  trackedImage.referenceImage.name;
        Vector3 currentPose = trackedImage.transform.position;
        imageName.text = "Current target is " + currentName;
        AssignObjectToCurrentImage(currentName, currentPose);
    }

    private void AssignObjectToCurrentImage(string currentName, Vector3 currentPose)
    {
        if(objetsPrefabs != null)
        {
            objects[currentName].SetActive(true); 
            objects[currentName].transform.position = currentPose;

            print("here");
            foreach (GameObject obj in objects.Values)
            {
                if (obj.name != currentName)
                {
                    obj.SetActive(false); 
                }
            }

        }
    }

    private void Excute(ARTrackedImage trackedImage)
    {
        currentImage = trackedImage;

        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            ClearCurrentImage(trackedImage);
        }
        else
        {
            GetCurrentImage(trackedImage);
        }
    }


    public void OnBack()
    {
        SceneManager.LoadScene("Menu");
    }
}
