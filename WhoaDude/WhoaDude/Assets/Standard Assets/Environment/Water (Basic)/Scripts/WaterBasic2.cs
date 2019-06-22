using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class WaterBasic2 : MonoBehaviour
    {
		Vector4 waveSpeed;
		Vector4 offsetClamped;

		void Start()
		{
			waveSpeed = new Vector4(10,10,10,10);
		}
        void Update()
        {
            Renderer r = GetComponent<Renderer>();
            if (!r)
            {
                return;
            }

            Material mat = r.sharedMaterial;
            if (!mat)
            {
                return;
            }

			//Vector4 waveSpeed = new Vector4(10,10,10,10);//mat.GetVector("WaveSpeed");
			float waveScale = 0.15f;//mat.GetFloat("_WaveScale");
            float t = Time.time / 20.0f;

            Vector4 offset4 = waveSpeed * (t * waveScale);
            offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1.0f), Mathf.Repeat(offset4.y, 1.0f),
                Mathf.Repeat(offset4.z, 1.0f), Mathf.Repeat(offset4.w, 1.0f));
            mat.SetVector("_WaveOffset", offsetClamped);


        }
    }
}