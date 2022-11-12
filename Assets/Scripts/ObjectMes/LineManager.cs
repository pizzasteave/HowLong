
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
using UnityEngine.SceneManagement;

public class LineManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ARPlacementInteractable placementInteractable;

    public TextMeshPro text;
    public TextMeshProUGUI toggle;

    public bool continuous;


    private bool wasContinous;
    private int pointCount = 0;
    LineRenderer Line;
    // Start is called before the first frame update
    void Start()
    {
        placementInteractable.objectPlaced.AddListener(DrawLine);
    }

    public void Toggle()
    {
        continuous = !continuous;

        if (continuous)
        {
            wasContinous = false;
            toggle.text = "Continuous";
        }
        else
        {
            wasContinous = true;
            toggle.text = "Discrete";
        }     
    }



    private void DrawLine(ARObjectPlacementEventArgs arg0)
    {
        pointCount++;  

        if(pointCount < 2)
        {
            Line = Instantiate(lineRenderer);
            Line.positionCount = 1;
        }else
        {

            if (!wasContinous)
            {
                Line.positionCount = pointCount;

                //vrai si on n'est pas dans le mode continious
                if (!continuous)
                    pointCount = 0;
            }
                  
            else
            {
                Line = Instantiate(lineRenderer);
                Line.positionCount = 1;
                pointCount = 1;
                wasContinous = false;
            }
          
        }

        //let the points location in the line renderer
        Line.SetPosition(Line.positionCount - 1, arg0.placementObject.transform.position);

        if(Line.positionCount > 1)
        {
            Vector3 pointA = Line.GetPosition(Line.positionCount - 1);
            var pointB = Line.GetPosition(Line.positionCount - 2);

            float distance = Vector3.Distance(pointB, pointA);

            //un nouveau text
            TextMeshPro distText = Instantiate(text);  
            // F2 = to have 2 digits ,xx
            distText.text = distance.ToString("F2");

            Vector3 directionVector = (pointB - pointA);
            Vector3 normal = arg0.placementObject.transform.up;

            Vector3 upd = Vector3.Cross(directionVector, normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(-normal, upd);

            distText.transform.rotation = rotation;
            distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.05f;

        }
    }

    public void OnBack()
    {
        SceneManager.LoadScene("Menu");
    }

}
