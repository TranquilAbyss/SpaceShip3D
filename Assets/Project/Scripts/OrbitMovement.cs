using UnityEngine;
using System.Collections;

public class OrbitMovement : MonoBehaviour {

    public GameObject OrbitCenterTarget;
    public Camera ZoomCamera;

    private float yVelocity = 0;
    private float xVelocity = 0;

    public float xDrag = 10;
    public float xMinSpeed = 0;
    public float xMaxSpeed = 5;

    public float yDrag = 10;
    public float yMinSpeed = 0;
    public float yMaxSpeed = 5;

    public float zoomSpeed = 1;
    public float zoomMin = 1;
    public float zoomMax = 60;


    public bool lockZAxis = false;

    // Use this for initialization
    void Start () {
        OrbitCenterTarget = GameObject.Find("Hull");
        transform.LookAt(OrbitCenterTarget.transform);
    }

    // Update is called once per frame
    void Update() {
        //orbit
        float xMagnitude = 0;
        float yMagnitude = 0;

        if (Input.GetMouseButton(2)) { 

            xMagnitude = Input.GetAxis("Mouse X");
            yMagnitude = Input.GetAxis("Mouse Y");
            
        }

        //acclerate speed
        xVelocity += xMagnitude;
        yVelocity += yMagnitude;

        //update rotation around target
        transform.RotateAround(Vector3.zero, transform.up, Time.deltaTime * xVelocity * 20);
        transform.RotateAround(Vector3.zero, -transform.right, Time.deltaTime * yVelocity * 20);

        //apply drag
        float xDir = Mathf.Sign(xVelocity);
        float yDir = Mathf.Sign(yVelocity);
        float xSpeed = Mathf.Abs(xVelocity);
        float ySpeed = Mathf.Abs(yVelocity);


        xSpeed = Mathf.Clamp(xSpeed - xDrag * Time.deltaTime, xMinSpeed, xMaxSpeed);
        ySpeed = Mathf.Clamp(ySpeed - yDrag * Time.deltaTime, yMinSpeed, yMaxSpeed);

        xVelocity = xSpeed * xDir;
        yVelocity = ySpeed * yDir;


        if (lockZAxis)
            transform.transform.eulerAngles = new Vector3(transform.transform.eulerAngles.x, transform.transform.eulerAngles.y, 0);

        //zoom
        float scrollVelocity = Input.GetAxis("Mouse ScrollWheel");
        if (scrollVelocity != 0)
        {
            //transform.position += scrollVelocity * zoomSpeed * transform.forward;
            ZoomCamera.fieldOfView = Mathf.Clamp(ZoomCamera.fieldOfView + -1 * scrollVelocity * zoomSpeed, zoomMin, zoomMax);

        }
    }

}
