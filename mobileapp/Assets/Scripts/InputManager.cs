using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GlobeManager globe;
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject addPinButton;

    [SerializeField]
    ButtonActions BA;
    [SerializeField]
    GameObject ContactCanvas;

    public bool addPin = false;

    GameObject tempPin = null;

    public void PauseButton()
    {
        globe.isRotating = false;
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void PlayButton()
    {
        if(addPin)
        {
            addPin = false;
            addPinButton.SetActive(true);
        }

        globe.Recenter();
        globe.isRotating = true;

        pauseButton.SetActive(true);
        playButton.SetActive(false);
    }

    public void AddPinButton()
    {
        addPin = true;
        tempPin = null;
    }

    public void setAddPin(bool b)
    {
        addPin = b;
    }

    public void CancelAddPinButton()
    {
        addPin = false;
        if (tempPin != null)
        {
            Destroy(tempPin);
            tempPin = null;
        }
    }

    public void ConfirmAddPinButton()
    {
        if(tempPin != null)
        {
            globe.Recenter();
            string XYZ = tempPin.transform.position.x.ToString() + "," + tempPin.transform.position.y.ToString() + "," + tempPin.transform.position.z.ToString();
            //Debug.Log(XYZ);
            BA.SetContactCoord(XYZ);
            ContactCanvas.SetActive(true);
        }
    }

    private Vector3 fp1 = Vector3.zero;   //First touch position
    private Vector3 lp1 = Vector3.zero;   //Last touch position
    private Vector3 fp2 = Vector3.zero;   //First touch position
    private Vector3 lp2 = Vector3.zero;   //Last touch position

    bool wasZoomingLastFrame;
    Vector2[] lastZoomPositions;

    void Update()
    {
        switch (Input.touchCount)
        {
            case 1: // swiping and tapping
                Touch touch = Input.GetTouch(0); // get the touch

                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp1 = touch.position;
                    lp1 = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) // update the last position based on where they moved
                {
                    lp1 = touch.position;

                    float xChange = lp1.x - fp1.x;
                    float yChange = lp1.y - fp1.y;

                    // tap
                    if (xChange == 0 && yChange == 0)
                    {
                        if (addPin)
                        {
                            if (Input.touchCount == 1) // user is touching the screen with a single touch
                            {
                                RaycastHit hit;
                                var ray = Camera.main.ScreenPointToRay(touch.position);

                                if (Physics.Raycast(ray, out hit) && hit.rigidbody != null)
                                {
                                    tempPin = globe.AddPin(hit.point.x, hit.point.y, hit.point.z, tempPin);
                                }
                            }
                        }
                    }
                    else globe.Rotate(new Vector3(yChange, -xChange, 0));
                }
                break;
            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    globe.Scale(offset);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }
}
