using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	public bool isColliding;
    private ConstructionSelection constructionSelection;
    private Color32 green = new Color32(51, 255, 0, 82);
    private Color32 red = new Color32(255, 34, 0, 82);

    void Start()
    {
        constructionSelection = GameObject.Find("Constructor").GetComponentInChildren<ConstructionSelection>();
    }

    void OnTriggerStay(Collider other) {
        if (other.attachedRigidbody)
        {
            isColliding = true;
            if (constructionSelection.IsBuilding)
            {
                GetComponent<MeshRenderer>().materials[0].SetColor("_Color", red);
            }        
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            NotColliding();
        }
    }

    public void NotColliding()
    {
        isColliding = false;
        GetComponent<MeshRenderer>().materials[0].SetColor("_Color", green);
    } 

}
