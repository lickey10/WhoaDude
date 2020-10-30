using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class webcamClass : MonoBehaviour {
	private GameObject[] cameraTargets;
	string deviceName;
	WebCamTexture wct;
#if UNITY_ANDROID
	private AndroidJavaObject camera;
	AndroidPhotoGalleryHelperClass androidPhotoGalleryHelperClass;
#endif
	private bool Active;
	// For saving to the _savepath
	//private string _SavePath = "C:/WebcamSnaps/"; //Change the path here!
	private string _SavePath;// = Application.persistentDataPath +"/WhoaDude/"; //Change the path here!
	int _CaptureCounter = 0;
	System.Diagnostics.Stopwatch sw;
	private long doubleClickDelay = 300;//the delay in milliseconds allowed between clicks for it to be considered a doubleclick

	// For photo varibles
	//public Texture2D heightmap;
	public Vector3 size = new Vector3(100, 10, 100);
    

	// Use this for initialization
	void Start () {
		//_SavePath = Application.persistentDataPath +"/WhoaDude/";
		_SavePath = Application.persistentDataPath +"/";
		sw = new System.Diagnostics.Stopwatch();

        cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");

        

#if UNITY_WEBPLAYER
			Application.RequestUserAuthorization (UserAuthorization.WebCam | UserAuthorization.Microphone);

			if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) 
			{
				startWebcam();
			}
			else
			{
				//we don't have permission
				Debug.Log("We don't have permission");

				return;

	//				devices = WebCamTexture.devices;
	//				deviceName = devices[0].name;
	//				wct = new WebCamTexture(deviceName, 640, 360, 15);
	//				renderer.material.mainTexture = wct;
	//				wct.Play();
	//				resultString = "no problems";
	//			} else {
	//				resultString = "no permission!";
			}
#endif

#if UNITY_ANDROID
		androidPhotoGalleryHelperClass = new AndroidPhotoGalleryHelperClass();

		if(_SavePath.ToLower().Contains("/sdcard/"))
		{
			_SavePath = _SavePath.Substring(0,_SavePath.ToLower().IndexOf("/sdcard/"));
			_SavePath = _SavePath + "/sdcard/WhoaDude/";
		}

			startWebcam();
#endif
    }

	void Update()
	{
        #if UNITY_WEBPLAYER
		if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone) && cameraTargets == null)
            updateKaleidescope
			//startWebcam();
        #endif

        //if(backgroundTextures.Length > 0)
        //{
        //    //updateKaleidescope(currentBackground);
        //}
    }

    void OnGUI() {      
		//if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
		//	TakeSnapshot();
	}

    private void updateKaleidescope(Texture kaleidescopeTexture)
    {
        //foreach (GameObject cameraTarget in cameraTargets)
        //{
        //    cameraTarget.GetComponent<Renderer>().material.mainTexture = kaleidescopeTexture;

        //    //			if(x < 6)//the first set of squares
        //    //			{
        //    cameraTarget.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(-.5f, -.5f));
        //    //			}
        //    //			else
        //    //			{
        //    //				CameraTargets[x].GetComponent<Renderer>().material.SetTextureOffset("_MainTex",new Vector2(.25f,.25f));
        //    //			}
        //}

        Vector2 changeDelta = Vector2.Scale((Time.deltaTime * new Vector2(.02f, .02f)), new Vector2(.01f, .01f));

        foreach (GameObject cameraTarget in cameraTargets)
            cameraTarget.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", changeDelta);
    }


    private void startWebcam()
	{

		WebCamDevice[] devices = WebCamTexture.devices;
		deviceName = devices[0].name;
		wct = new WebCamTexture(deviceName, 400, 300, 12);
		
		//cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");

        //updateKaleidescope(wct);

        foreach (GameObject cameraTarget in cameraTargets)
        {
            cameraTarget.GetComponent<Renderer>().material.mainTexture = wct;

            //			if(x < 6)//the first set of squares
            //			{
            cameraTarget.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(-.5f, -.5f));
            //			}
            //			else
            //			{
            //				CameraTargets[x].GetComponent<Renderer>().material.SetTextureOffset("_MainTex",new Vector2(.25f,.25f));
            //			}
        }

        wct.Play();
		
		//Lean.LeanTouch.OnFingerHeldDown += OnFingerHeldDown;

		Lean.LeanTouch.OnFingerTap += OnFingerTap;
	}

	public void TakeSnapshot()
	{
		//this gets what the camera sees
//		Texture2D snap = new Texture2D(wct.width, wct.height);
//		snap.SetPixels(wct.GetPixels());
//		snap.Apply();


		# if UNITY_WEBPLAYER

		Application.CaptureScreenshot(_SavePath + (_CaptureCounter).ToString() + ".png");

		_CaptureCounter++;

		#else

		//System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
		
		while(System.IO.File.Exists(_SavePath + _CaptureCounter.ToString() + ".png"))
			++_CaptureCounter;
		
		// Create the folder beforehand if not exists
		if(!System.IO.Directory.Exists(_SavePath))
			System.IO.Directory.CreateDirectory(_SavePath);
		
		//Application.CaptureScreenshot(_SavePath + (_CaptureCounter).ToString() + ".png");
		
		//capture screen
		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
		tex.Apply();
		
		//save to file
		System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", tex.EncodeToPNG());
		
		#if UNITY_ANDROID
		androidPhotoGalleryHelperClass.InsertPhoto(_SavePath + _CaptureCounter.ToString() + ".png");
		#endif
		
		//System.IO.Directory.GetFiles(_SavePath);


		#endif



		


		Debug.Log("Image save to " + _SavePath + (_CaptureCounter).ToString() + ".png");
	}

	public void OnFingerHeldDown(Lean.LeanFinger finger)
	{
		Debug.Log("Finger " + finger.Index + " began touching the screen for a long time");

		TakeSnapshot();
	}

	public void OnFingerTap(Lean.LeanFinger finger)
	{
		Debug.Log("Finger " + finger.Index + " tapped");

		long elapsedMilliseconds = 0;
		elapsedMilliseconds = sw.ElapsedMilliseconds;

		if(!sw.IsRunning || elapsedMilliseconds > doubleClickDelay)//this is the first tap
		{
			sw.Reset();
			sw.Start();
		}
		else
		{
			sw.Stop();

			if(elapsedMilliseconds < doubleClickDelay)//this is a double tap
				TakeSnapshot();
		}
	}
}
