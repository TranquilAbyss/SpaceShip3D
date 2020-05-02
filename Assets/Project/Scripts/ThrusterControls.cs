using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ThrusterControls : MonoBehaviour {

    private Thruster[] thrusters;

    [SerializeField]
    private List<Thruster> forwardThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> reverseThrusters = new List<Thruster> ();

    [SerializeField]
    private List<Thruster> pitchUpThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> pitchDownThrusters = new List<Thruster> ();

    [SerializeField]
    private List<Thruster> yawLeftThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> yawRightThrusters = new List<Thruster> ();

    [SerializeField]
    private List<Thruster> moveUpThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> moveDownThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> moveLeftThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> moveRightThrusters = new List<Thruster> ();

    [SerializeField]
    private List<Thruster> rollRightThrusters = new List<Thruster> ();
    [SerializeField]
    private List<Thruster> rollLeftThrusters = new List<Thruster> ();

	private Vector3 velocity;
	private Vector3 angularVelocity;
	public GameObject direction;

    public float safeThrottle = .2f;
    public float correctionThrottle = .3f;
    public float breakThrottle = .5f;
    private float maxThrottle = 1;


    public float mouseLeftBound = .35f;
    public float mouseRightBound = .65f;
    public float mouseTopBound = .35f;
    public float mouseBottonBound = .65f;

    private bool areThrustersActive = false; 


    // Use this for initialization
    void Start () {
       
		direction = transform.parent.GetChild(1).gameObject;

        CalibrateThrusters();       
    }

    void ActivateThrusters()
    {
        areThrustersActive = true;
        foreach (Thruster thruster in thrusters)
        {
            thruster.active = true;
        }
    }

    void DeactivateThrusters()
    {
        areThrustersActive = false;
        foreach (Thruster thruster in thrusters)
        {
            thruster.active = false;
            GameObject.Find("ShipStats").GetComponent<Text>().text = "Press O to turn on thrusters.";
        }
    }

    // Update is called once per frame
    void Update () {
		Vector3 localangularvelocity = transform.InverseTransformDirection (GetComponent<Rigidbody>().angularVelocity);
		Vector3 localvelocity = transform.InverseTransformDirection (GetComponent<Rigidbody>().velocity);
		if(GameObject.Find("ShipStats") != null && areThrustersActive)
		{
		    GameObject.Find("ShipStats").GetComponent<Text>().text = "Speed: " + Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude*100)/100
														    + "\nVelocity: " + localvelocity
														    + "\nPitch: " + Mathf.Round(localangularvelocity.x*1000)/1000
														    + " Yaw: " + Mathf.Round(localangularvelocity.y*1000)/1000 
														    + "\nRoll: " + Mathf.Round(localangularvelocity.z*1000)/1000;
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            ActivateThrusters();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            DeactivateThrusters();
        }

		direction.transform.position = (GetComponent<Rigidbody>().velocity.normalized * (transform.localScale.z + 2)) + transform.position;

		//reset yaw and pitch and roll engines to not active, to prevent both directions from firing while using the mouse.
		foreach (Thruster thruster in yawRightThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in yawLeftThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in pitchUpThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in pitchDownThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in rollRightThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in rollLeftThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in moveUpThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in moveDownThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in moveLeftThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in moveRightThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in forwardThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}
		foreach (Thruster thruster in reverseThrusters)
		{
			if(thruster == null)
				break;
			thruster.throttle = 0;
		}

        /*
         * Reverse
         */        
        if (Input.GetKey(KeyCode.S))
		{
            float tempThrottle = safeThrottle;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                tempThrottle = maxThrottle;
            }
            foreach (Thruster thruster in reverseThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = tempThrottle;
			}
		}

		/*
		 * Mouse yaw 
		 */
		if(Input.mousePosition.x/Screen.width > mouseRightBound)
		{
			float tempThrottle = safeThrottle;
			if(angularVelocity.y < 0)
			{
				tempThrottle = 1;
			}
			foreach (Thruster thruster in yawRightThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = tempThrottle;
			}
		}
		else if(Input.mousePosition.x/Screen.width < mouseLeftBound)
		{
			float tempThrottle = safeThrottle;
			if(angularVelocity.y > 0)
			{
				tempThrottle = 1;
			}
			foreach (Thruster thruster in yawLeftThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = tempThrottle;
			}
		}
		else if (Input.mousePosition.x/Screen.width > mouseLeftBound & Input.mousePosition.x/Screen.width <= mouseRightBound)
		{

			if(localangularvelocity.y >= 0.01f)
			{
				foreach (Thruster thruster in yawLeftThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}

			if(localangularvelocity.y <= -0.01f)
			{
				foreach (Thruster thruster in yawRightThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
		}

		/*
		 * Mouse pitch 
		 */
		if(Input.mousePosition.y/Screen.height > mouseBottonBound)
		{
			float tempThrottle = safeThrottle;
			if(angularVelocity.x > 0)
			{
				tempThrottle = 1;
			}
			foreach (Thruster thruster in pitchUpThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = tempThrottle;
			}
		}
		else if(Input.mousePosition.y/Screen.height < mouseTopBound)
		{
			float tempThrottle = safeThrottle;
			if(angularVelocity.x < 0)
			{
				tempThrottle = 1;
			}
			foreach (Thruster thruster in pitchDownThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = tempThrottle;
			}
		}
		else if (Input.mousePosition.y/Screen.height > mouseTopBound & Input.mousePosition.y/Screen.height <= mouseBottonBound)
		{
			if(localangularvelocity.x >= 0.01f)
			{
				foreach (Thruster thruster in pitchUpThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
			
			if(localangularvelocity.x <= -0.01f)
			{
				foreach (Thruster thruster in pitchDownThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
		}
			


		/*
		 * Keyboard move
		 */
		if(Input.GetKey(KeyCode.A))
		{
			
			foreach (Thruster thruster in moveLeftThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = safeThrottle;
			}
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			foreach (Thruster thruster in moveRightThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = safeThrottle;
			}
		}
		
		if(Input.GetKey(KeyCode.Q))
		{
			foreach (Thruster thruster in moveDownThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = safeThrottle;
			}
		}
		
		if(Input.GetKey(KeyCode.E))
		{
			foreach (Thruster thruster in moveUpThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = safeThrottle;
			}
		}

		if(Input.GetKey(KeyCode.C))
		{
			foreach (Thruster thruster in rollLeftThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = safeThrottle;
			}
		}

		if(Input.GetKey(KeyCode.Z))
		{
			foreach (Thruster thruster in rollRightThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = safeThrottle;
			}
		}

		if(!Input.GetKey(KeyCode.Z) & !Input.GetKey(KeyCode.C))
		{
			if(localangularvelocity.z > 0.01f)
			{
				foreach (Thruster thruster in rollLeftThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
			else if(localangularvelocity.z < -0.01f)
			{
				foreach (Thruster thruster in rollRightThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
		}

		if(Input.GetKey(KeyCode.W))
		{
			float tempThrottle = safeThrottle;
			if(Input.GetKey(KeyCode.LeftShift))
			{
				tempThrottle = maxThrottle;
			}

			foreach (Thruster thruster in forwardThrusters)
			{
				if(thruster == null)
					break;
				thruster.throttle = tempThrottle;
			}
			
			//when turning left
			if(localvelocity.x > 0.01f)
			{
				foreach (Thruster thruster in moveLeftThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
			//when turning right
			else if(localvelocity.x < -0.01f)
			{
				foreach (Thruster thruster in moveRightThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
			//when turning up
			if(localvelocity.y > 0.01f)
			{
				foreach (Thruster thruster in moveDownThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
			//when turning down
			else if(localvelocity.y < -0.01f)
			{
				foreach (Thruster thruster in moveUpThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = correctionThrottle;
				}
			}
		}

		//Breaks
		if(Input.GetKey(KeyCode.Space))
		{
			//stop up movment
			if(localvelocity.y > 0.01f)
			{
				foreach (Thruster thruster in moveDownThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = breakThrottle;
				}
			}
			//stop down movement
			else if(localvelocity.y < -0.01f)
			{
				foreach (Thruster thruster in moveUpThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = breakThrottle;
				}
			}
			//stop right movmentment
			if(localvelocity.x > 0.01f)
			{
				foreach (Thruster thruster in moveLeftThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = breakThrottle;
				}
			}
			//stop left movement
			else if(localvelocity.x < -0.01f)
			{
				foreach (Thruster thruster in moveRightThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = breakThrottle;
				}
			}
			//stop forwards
			if(localvelocity.z > 0.01f)
			{
				foreach (Thruster thruster in reverseThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = breakThrottle;
				}
			}
			//stop reverse
			else if(localvelocity.z < -0.01f)
			{
				foreach (Thruster thruster in forwardThrusters)
				{
					if(thruster == null)
						break;
					thruster.throttle = breakThrottle;
				}
			}
		}

		velocity = GetComponent<Rigidbody>().velocity;
		angularVelocity = GetComponent<Rigidbody>().angularVelocity;

	}

     private void CalibrateThrusters()
    {
        thrusters = transform.parent.GetComponentsInChildren<Thruster>();

        //movement thrusters
        foreach (Thruster thruster in thrusters)
        {
            //fowards
            if (Vector3.Dot(thruster.transform.forward, transform.forward) == 1)
            {
                forwardThrusters.Add(thruster);
            }

            //reverse
            if (Vector3.Dot(thruster.transform.forward, transform.forward) == -1)
            {
                reverseThrusters.Add(thruster);
            }

            //move up down left right
            if (Mathf.Approximately(Vector3.Dot(thruster.transform.forward, transform.up), 1))
            {
                moveUpThrusters.Add(thruster);
            }
            if (Mathf.Approximately(Vector3.Dot(thruster.transform.forward, transform.up), -1))
            {
                moveDownThrusters.Add(thruster);
            }
            if (Mathf.Approximately(Vector3.Dot(thruster.transform.forward, transform.right), -1))
            {
                moveLeftThrusters.Add(thruster);
            }
            if (Mathf.Approximately(Vector3.Dot(thruster.transform.forward, transform.right), 1))
            {
                moveRightThrusters.Add(thruster);
            }

        }

        //Yaw and pitch and roll thrusters
        foreach (Thruster thruster in forwardThrusters)
        {
            //turnRight
            if (thruster.transform.localPosition.x < transform.localPosition.x)
            {
                yawRightThrusters.Add(thruster);
            }
            //turnLeft
            if (thruster.transform.localPosition.x > transform.localPosition.x)
            {
                yawLeftThrusters.Add(thruster);
            }
            //pitch
            if (thruster.transform.localPosition.y < transform.localPosition.y)
            {
                pitchUpThrusters.Add(thruster);
            }
            if (thruster.transform.localPosition.y > transform.localPosition.y)
            {
                pitchDownThrusters.Add(thruster);
            }
        }
        foreach (Thruster thruster in reverseThrusters)
        {
            //turnRight
            if (thruster.transform.localPosition.x > transform.localPosition.x)
            {
                yawRightThrusters.Add(thruster);
            }
            //turnLeft
            if (thruster.transform.localPosition.x < transform.localPosition.x)
            {
                yawLeftThrusters.Add(thruster);
            }
            //pitch
            if (thruster.transform.localPosition.y > transform.localPosition.y)
            {
                pitchUpThrusters.Add(thruster);
            }
            if (thruster.transform.localPosition.y < transform.localPosition.y)
            {
                pitchDownThrusters.Add(thruster);
            }
        }
        foreach (Thruster thruster in moveLeftThrusters)
        {
            //turnRight
            if (thruster.transform.localPosition.z < transform.localPosition.z)
            {
                yawRightThrusters.Add(thruster);
            }
            //turnLeft
            if (thruster.transform.localPosition.z > transform.localPosition.z)
            {
                yawLeftThrusters.Add(thruster);
            }
            //roll right
            if (thruster.transform.localPosition.y > transform.localPosition.y)
            {
                rollRightThrusters.Add(thruster);
            }
            //roll left
            if (thruster.transform.localPosition.y < transform.localPosition.y)
            {
                rollLeftThrusters.Add(thruster);
            }

        }
        foreach (Thruster thruster in moveRightThrusters)
        {
            //turnRight
            if (thruster.transform.localPosition.z > transform.localPosition.z)
            {
                yawRightThrusters.Add(thruster);
            }
            //turnLeft
            if (thruster.transform.localPosition.z < transform.localPosition.z)
            {
                yawLeftThrusters.Add(thruster);
            }
            //roll right
            if (thruster.transform.localPosition.y < transform.localPosition.y)
            {
                rollRightThrusters.Add(thruster);
            }
            //roll left
            if (thruster.transform.localPosition.y > transform.localPosition.y)
            {
                rollLeftThrusters.Add(thruster);
            }
        }
        foreach (Thruster thruster in moveUpThrusters)
        {
            //pitch
            if (thruster.transform.localPosition.z > transform.localPosition.z)
            {
                pitchUpThrusters.Add(thruster);
            }
            if (thruster.transform.localPosition.z < transform.localPosition.z)
            {
                pitchDownThrusters.Add(thruster);
            }
            //roll right
            if (thruster.transform.localPosition.x > transform.localPosition.x)
            {
                rollRightThrusters.Add(thruster);
            }
            //roll left
            if (thruster.transform.localPosition.x < transform.localPosition.x)
            {
                rollLeftThrusters.Add(thruster);
            }
        }
        foreach (Thruster thruster in moveDownThrusters)
        {
            //pitch
            if (thruster.transform.localPosition.z < transform.localPosition.z)
            {
                pitchUpThrusters.Add(thruster);
            }
            if (thruster.transform.localPosition.z > transform.localPosition.z)
            {
                pitchDownThrusters.Add(thruster);
            }
            //roll right
            if (thruster.transform.localPosition.x < transform.localPosition.x)
            {
                rollRightThrusters.Add(thruster);
            }
            //roll left
            if (thruster.transform.localPosition.x > transform.localPosition.x)
            {
                rollLeftThrusters.Add(thruster);
            }
        }
    }

}
