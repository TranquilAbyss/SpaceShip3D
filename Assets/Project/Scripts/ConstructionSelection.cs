using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConstructionSelection : MonoBehaviour {

	public GameObject laser;

	private GameObject laserInstance;
	public float offset = 0.1f;
	//private int buildIndex = 1;
	public GameObject thruster;
	public GameObject customShip;
	private bool isBuilding = false;
	public GameObject BuildingOutline;
	private GameObject selectedObject;
	private bool disableContruction = false;
    public Camera constructorCamera;

    private bool didSelect = false;
	private bool didRayHit;
	private RaycastHit hit;

	//starting orientation
    [SerializeField]
	private List<GameObject> ShipParts = new List<GameObject>();
	private List<Vector3> location = new List<Vector3>();
	private List<Quaternion> rotaion = new List<Quaternion>();

    public bool IsBuilding
    {
        get { return isBuilding; }
    }

    // Use this for initialization
    void Start () {
        customShip = GameObject.Find("CustomShip");
		BuildingOutline.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if(disableContruction == true)
		{
            if (Input.GetKeyDown(KeyCode.Return))
                StartConstruction();
			return;
		}
		

		if(Input.GetKeyDown(KeyCode.Return))
		{
			CompleteConstruction();
		}

        UpdateLaserPointer();

		StartBuild ();

		EndBuild();

		//If ray hits not selected object

		if (didRayHit)
		{
			if(selectedObject == null)
			{
				if(hit.transform.tag == "Thruster" || hit.transform.tag == "Hull")
				{
					Select();
				}
			}
			else if(selectedObject != null)
			{
				if((hit.transform.tag == "Thruster" || hit.transform.tag == "Hull") && hit.transform.gameObject.GetHashCode() != selectedObject.GetHashCode())
				{
					Select();
				}
			}
		}

		if(!didSelect)
		{
		    RotateThruster ();
		}

		RemoveThruster ();

		if (didRayHit)
		{
			SnapBuild();
			DragHullToScale();
		}

		didSelect = false;
	}

    void UpdateLaserPointer()
    {
        Vector3 mouseOnScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        Ray mouseRay = constructorCamera.ScreenPointToRay(mouseOnScreen);

        didRayHit = Physics.Raycast(mouseRay, out hit, 1000.0f);

        //laser position
        if (laserInstance == null)
        {
            laserInstance = Instantiate(laser, hit.point, Quaternion.identity) as GameObject;
        }
        else if (didRayHit)
        {
            laserInstance.SetActive(true);
            laserInstance.transform.position = hit.point + hit.normal * offset;
        }
        else
        {
            laserInstance.SetActive(false);
        }
    }

	void Select()
	{
		//select GameObject
		if(Input.GetMouseButtonDown(0) && !isBuilding)
		{
			selectedObject = hit.transform.gameObject;
			BuildingOutline.SetActive(true);
			BuildingOutline.transform.position = selectedObject.transform.position;
			BuildingOutline.transform.localScale = selectedObject.transform.localScale * 1.1f;
			BuildingOutline.transform.localRotation = selectedObject.transform.localRotation;
			didSelect = true;
		}
	}

	void RotateThruster ()
	{
		if(selectedObject != null)
		{
			if(selectedObject.transform.tag == "Thruster")
			{
				//rotate 90 degrees
				if(Input.GetMouseButtonDown(0))
				{
					selectedObject.transform.Rotate(new Vector3(0,90,0),Space.World);
					BuildingOutline.transform.localRotation = selectedObject.transform.localRotation;
				}
				if(Input.GetMouseButtonDown(1))
				{
					selectedObject.transform.Rotate(new Vector3(90,0,0),Space.World);
					BuildingOutline.transform.localRotation = selectedObject.transform.localRotation;
				}
			}
		}
	}

	void DragHullToScale ()
	{
        if (selectedObject == null)
        {
            return;
        }
		//scale Ship if selected hull and ray is hitting hull
		if(selectedObject.transform.tag == "Hull" && hit.transform.gameObject.GetHashCode() == selectedObject.GetHashCode())
		{
			if(Input.GetMouseButton(0))
			{
					
				if(Mathf.Abs(Vector3.Dot(hit.normal, hit.transform.forward)) == 1)
				{
					if(Input.GetAxis("Mouse X") > .05f)
						ChangeHullSize(selectedObject.transform, new Vector3(1,0,0));
					if(Input.GetAxis("Mouse X") < -.05f)
						ChangeHullSize(selectedObject.transform, new Vector3(-1,0,0));
					if(Input.GetAxis("Mouse Y") > .05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,1,0));
					if(Input.GetAxis("Mouse Y") < -.05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,-1,0));
				}
					
				if(Mathf.Abs(Vector3.Dot(hit.normal, hit.transform.up)) == 1)
				{
					if(Input.GetAxis("Mouse X") > .05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,0,1));
					if(Input.GetAxis("Mouse X") < -.05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,0,-1));
					if(Input.GetAxis("Mouse Y") > .05f)
						ChangeHullSize(selectedObject.transform, new Vector3(1,0,0));
					if(Input.GetAxis("Mouse Y") < -.05f)
						ChangeHullSize(selectedObject.transform, new Vector3(-1,0,0));
				}
					
				if(Mathf.Abs(Vector3.Dot(hit.normal, hit.transform.right)) == 1)
				{
					if(Input.GetAxis("Mouse X") > .05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,0,1));
					if(Input.GetAxis("Mouse X") < -.05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,0,-1));
					if(Input.GetAxis("Mouse Y") > .05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,1,0));
					if(Input.GetAxis("Mouse Y") < -.05f)
						ChangeHullSize(selectedObject.transform, new Vector3(0,-1,0));
				}
		    		
		    }
			
		}
	}

	void StartBuild()
	{
		if(Input.GetKeyDown(KeyCode.E) && !isBuilding)
		{
			isBuilding = true;
			BuildingOutline.SetActive(true);
			selectedObject = null;
			BuildingOutline.transform.localRotation = Quaternion.identity;
		}
	}

    void EndBuild()
    {
        if (isBuilding)
        {

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
            {
                BuildingOutline.GetComponent<CollisionCheck>().NotColliding();
                BuildingOutline.SetActive(false);
                isBuilding = false;
            }

            if (Input.GetMouseButtonUp(0) && !BuildingOutline.GetComponent<CollisionCheck>().isColliding)
            {
                GameObject tempThruster;
                isBuilding = false;
                tempThruster = Instantiate(thruster) as GameObject;
                tempThruster.transform.parent = customShip.transform;
                tempThruster.GetComponent<Thruster>().active = false;
                tempThruster.GetComponent<Thruster>().throttle = 0;
                tempThruster.transform.position = BuildingOutline.transform.position;

                //tempThruster.GetComponent<FixedJoint>().connectedBody = customShip.transform.FindChild("Hull").GetComponent<Rigidbody>();

                selectedObject = tempThruster;
            }
        }
    }

    void RemoveThruster()
    {
        if (selectedObject != null)
        {
            if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.R)) && selectedObject.transform.tag == "Thruster")
            {
                BuildingOutline.GetComponent<CollisionCheck>().NotColliding();
                Destroy(selectedObject);
                selectedObject = null;
                BuildingOutline.SetActive(false);
            }
           
        }
    }

    void SnapBuild()
	{
		if(hit.transform.tag == "Hull" && isBuilding)
		{
			
			Vector3 modVector = new Vector3((Mathf.Round((hit.point.x ) / .25f) * .25f),
			                                (Mathf.Round((hit.point.y ) / .25f) * .25f),
			                                (Mathf.Round((hit.point.z ) / .25f) * .25f)); 
			modVector += hit.normal * .6f;
			
			BuildingOutline.transform.position = modVector;
			
			//Debug.Log(hit.point + hit.normal*.6f +" modified "+ modVector);
			BuildingOutline.transform.localScale = 1.1f * thruster.transform.localScale;
		}
	}

    void StartConstruction()
    {
        Thruster[] thrusters = customShip.GetComponentsInChildren<Thruster>();

        foreach (Thruster thruster in thrusters)
        {
            thruster.active = false;
            thruster.throttle = 0;
            thruster.Disassemble();
        }

        for (int i = 0; i < ShipParts.Count; ++i)
        {
            ShipParts[i].GetComponent<Rigidbody>().isKinematic = true;

            ShipParts[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            ShipParts[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ShipParts[i].transform.position = location[i];
            ShipParts[i].transform.rotation = rotaion[i];
        }

        GameObject shipUI = GameObject.Find("FlightUI");
        if (shipUI != null)
        {
            shipUI.GetComponent<CanvasGroup>().alpha = 0;
        }
        GameObject constructionUI = GameObject.Find("ConstructionUI");
        if (shipUI != null)
        {
            constructionUI.GetComponent<CanvasGroup>().alpha = 1;
        }

        customShip.transform.GetChild(1).transform.position = new Vector3(0, 2, 0);

        Transform hull = customShip.transform.GetChild(0);
        Destroy(hull.gameObject.GetComponent<ThrusterControls>());
        hull.GetChild(0).gameObject.SetActive(false);
        hull.GetChild(1).gameObject.SetActive(false);

        gameObject.GetComponent<Camera>().enabled = true;
        gameObject.GetComponent<AudioListener>().enabled = true;

        gameObject.transform.parent.GetComponent<OrbitMovement>().enabled = true;
        Cursor.visible = true;

        laserInstance.SetActive(true);

        disableContruction = false;
    }

    void CompleteConstruction ()
	{
        GameObject.Find("ShipStats").GetComponent<Text>().text = "Press O to turn on thrusters.";
        BuildingOutline.GetComponent<CollisionCheck>().NotColliding();

        Thruster[] thrusters = customShip.GetComponentsInChildren<Thruster>();
		ShipParts.Clear ();
		location.Clear ();
		rotaion.Clear ();
		
		Transform hull = customShip.transform.GetChild(0);
		ShipParts.Add (hull.gameObject);
		location.Add(hull.position);
		rotaion.Add(hull.rotation);
		
        //swap UI
		GameObject shipUI = GameObject.Find("FlightUI");
		if(shipUI != null)
		{
			shipUI.GetComponent<CanvasGroup>().alpha = 1;
		}
		GameObject constructionUI = GameObject.Find("ConstructionUI");
		if(shipUI != null)
		{
			constructionUI.GetComponent<CanvasGroup>().alpha = 0;
		}
		
        //setup cameras
		hull.gameObject.AddComponent<ThrusterControls>();
		hull.GetChild(0).gameObject.SetActive(true);
		hull.GetChild(1).gameObject.SetActive(true);
		
		gameObject.GetComponent<Camera>().enabled = false;
		gameObject.GetComponent<AudioListener>().enabled = false;
		
		gameObject.transform.parent.GetComponent<OrbitMovement>().enabled = false;
		Cursor.visible = true;

		BuildingOutline.SetActive (false);
		laserInstance.SetActive (false);

		disableContruction = true;
		
		foreach (Thruster thruster in thrusters)
		{
			ShipParts.Add(thruster.gameObject);
			location.Add(thruster.transform.position);
			rotaion.Add(thruster.transform.rotation);
			thruster.Assemble();
		}

        for (int i = 0; i < ShipParts.Count; ++i)
        {
            ShipParts[i].GetComponent<Rigidbody>().isKinematic = false;
        }
    }

	void ChangeHullSize(Transform hull, Vector3 amount)
	{

		Vector3 tempScale = new Vector3(0,0,0);
		if(amount.x != 0)
		{
			if((hull.localScale.x + amount.x) > .5f && (hull.localScale.x + amount.x) < 10)
			{
				tempScale.x = amount.x;
			}
		}
		if(amount.y != 0)
		{
			if((hull.localScale.y + amount.y) > .5f && (hull.localScale.y + amount.y) < 10)
			{
				tempScale.y = amount.y;
			}
		}
		if(amount.z != 0)
		{
			if((hull.localScale.z + amount.z) > .5f && (hull.localScale.z + amount.z) < 10)
			{
				tempScale.z = amount.z;
			}
		}

		hull.localScale += tempScale;
		Thruster[] thrusters = customShip.GetComponentsInChildren<Thruster>();
		foreach (Thruster thruster in thrusters)
		{
			if(thruster.transform.position.y > customShip.transform.position.y)
				thruster.transform.position += new Vector3(0,(tempScale.y * .5f),0);

			if(thruster.transform.position.y < customShip.transform.position.y)
				thruster.transform.position += new Vector3(0,(tempScale.y * -.5f),0);

			if(thruster.transform.position.x > customShip.transform.position.x)
				thruster.transform.position += new Vector3((tempScale.x * .5f),0,0);
			
			if(thruster.transform.position.x < customShip.transform.position.x)
				thruster.transform.position += new Vector3((tempScale.x * -.5f),0,0);

			if(thruster.transform.position.z > customShip.transform.position.z)
				thruster.transform.position += new Vector3(0, 0,(tempScale.z * .5f));
			
			if(thruster.transform.position.z < customShip.transform.position.z)
				thruster.transform.position += new Vector3(0, 0,(tempScale.z * -.5f));
		}
		BuildingOutline.transform.localScale = selectedObject.transform.localScale * 1.1f;
	}

}
