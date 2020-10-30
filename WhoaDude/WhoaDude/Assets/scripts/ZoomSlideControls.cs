using UnityEngine;
using System.Collections;

public class ZoomSlideControls : MonoBehaviour {
	private GameObject[] cameraTargets;
	//scroll main texture based on time
	
	public float scrollSpeed = 2.5f;
	float offset = 0f;
	float rotate = 0f;
	bool movingRight = true;
    public Texture[] backgroundTextures;
    private Texture currentBackground;

    // The minimum field of view value we want to zoom to
    public float MinFov = 10.0f;
	
	// The maximum field of view value we want to zoom to
	public float MaxFov = 60.0f;

	// Use this for initialization
	void Start () {
		cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");

        setImage();

        //		foreach(GameObject cameraTarget in cameraTargets)
        //			cameraTarget.GetComponent<Renderer>().material.mainTexture.wrapMode = TextureWrapMode.Clamp;
    }

    // Update is called once per frame
    void Update() {
        if (offset > 1)
            movingRight = false;

        if (offset < -1)
        {
            movingRight = true;

            setImage();
        }

        if (offset > 1.3)
        {
            movingRight = true;

            setImage();
        }

        if (offset < -1.3)
            movingRight = false;

		if(movingRight)
			offset+= (Time.deltaTime*scrollSpeed)/10.0f;
		else
			offset-= (Time.deltaTime*scrollSpeed)/10.0f;

		foreach(GameObject cameraTarget in cameraTargets)
			cameraTarget.GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(offset,0));
	}

	protected virtual void OnEnable()
	{
		Lean.LeanTouch.OnSoloDrag += OnSoloDrag;
		Lean.LeanTouch.OnPinch += OnPinch;
	}

	protected virtual void OnDisable()
	{
		Lean.LeanTouch.OnSoloDrag -= OnSoloDrag;
		Lean.LeanTouch.OnPinch -= OnPinch;
	}

    private void setImage()
    {
        if (backgroundTextures.Length > 0)
            currentBackground = backgroundTextures[Random.Range(0, backgroundTextures.Length - 1)];

        foreach (GameObject cameraTarget in cameraTargets)
        {
            cameraTarget.GetComponent<Renderer>().material.mainTexture = currentBackground;

            //			if(x < 6)//the first set of squares
            //			{
            cameraTarget.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(-.5f, -.5f));
            //			}
            //			else
            //			{
            //				CameraTargets[x].GetComponent<Renderer>().material.SetTextureOffset("_MainTex",new Vector2(.25f,.25f));
            //			}
        }
    }

	public void OnSoloDrag(Vector2 pixels)
	{
		Debug.Log("One finger moved " + pixels + " across the screen");

		Vector2 changeDelta = Vector2.Scale((Time.deltaTime*Lean.LeanTouch.DragDelta),new Vector2(.01f,.01f));

		foreach(GameObject cameraTarget in cameraTargets)
			cameraTarget.GetComponent<Renderer>().material.SetTextureOffset("_MainTex",changeDelta);
	}

	public void OnPinch(float scale)
	{
		Debug.Log("Many fingers pinched " + scale + " percent");

		if (Camera.main != null)
		{
			// Make sure the pinch scale is valid
			if (Lean.LeanTouch.PinchScale > 0.0f)
			{
				// Scale the FOV based on the pinch scale
				Camera.main.fieldOfView /= Lean.LeanTouch.PinchScale;
				
				// Make sure the new FOV is within our min/max
				Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, MinFov, MaxFov);
			}
		}
	}
}
