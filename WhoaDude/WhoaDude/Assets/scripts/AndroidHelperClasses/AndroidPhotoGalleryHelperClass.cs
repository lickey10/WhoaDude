using UnityEngine;
using System.Collections;

# if !UNITY_WEBPLAYER
public class AndroidPhotoGalleryHelperClass : MonoBehaviour {

	private static AndroidPhotoGalleryHelperClass _instance;
	private AndroidJavaObject playerActivityContext = null;

	public AndroidJavaObject AndroidPhotoGalleryControls;

	public static AndroidPhotoGalleryHelperClass Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new AndroidPhotoGalleryHelperClass();

				//forces unity to write camera permissions to AndroidManifest.xml
				WebCamTexture wtc = new WebCamTexture();
			}

			return _instance;
		}
	}

	public AndroidPhotoGalleryHelperClass()
	{
		AndroidPhotoGalleryControls = new AndroidJavaObject("com.snicklefritz.utility.AndroidPhotoGallery");

		// First, obtain the current activity context
		using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			playerActivityContext = actClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}

	public void InsertPhoto(string PhotoPath)
	{
		AndroidPhotoGalleryControls.Call("Insert", playerActivityContext, PhotoPath);
	}

	void OnApplicationQuit()
	{
		_instance = null;
	}
}
#endif
