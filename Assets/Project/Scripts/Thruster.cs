using UnityEngine;
using System.Collections;


public class Thruster : MonoBehaviour {

	public float m_maxThrust = 1;
    [SerializeField]
    private float m_throttle = 0.0f;
    [SerializeField]
    private bool m_active = false;
	private GameObject particleEmitter;
	private float maxParticleLifetime;

	public float maxThrust
	{
		get { return m_maxThrust; }
		set { m_maxThrust = value; }
	}
	public float throttle
	{
		get { return m_throttle; }
		set { m_throttle = value; }
	}

	public bool active
	{
		get { return m_active; }
		set { m_active = value; }
	}
	// Use this for initialization
	void Start () {
        particleEmitter = transform.GetChild(0).gameObject;
        maxParticleLifetime = particleEmitter.GetComponent<ParticleSystem>().main.startLifetime.constant;
	}

	public void Assemble () {

		if(GetComponent<FixedJoint>() == null)
		{
			gameObject.AddComponent<FixedJoint>();
		}

        FixedJoint fJoint = GetComponent<FixedJoint>();
        fJoint.connectedBody = transform.parent.GetChild(0).GetComponent<Rigidbody>();
        fJoint.enablePreprocessing = false;

	}

	public void Disassemble () {

		if(GetComponent<FixedJoint>() != null)
		{
			Destroy(gameObject.GetComponent<FixedJoint>());
		}

	}

	// Update is called once per frame
	void Update () {

		m_throttle = Mathf.Clamp(m_throttle, 0.0f, 1);

		if(m_active)
		{

			if(m_throttle > 0)
			{
				GetComponent<Rigidbody>().AddForce(transform.forward * (m_maxThrust * m_throttle));	
			}

		}

		if(m_throttle > 0 & m_active)
		{
			particleEmitter.SetActive(true);
            ParticleSystem.MainModule psm = particleEmitter.GetComponent<ParticleSystem>().main;
            psm.startLifetime = new ParticleSystem.MinMaxCurve(maxParticleLifetime * m_throttle);
		}
		else
		{
			particleEmitter.SetActive(false);
		}
	}
}
