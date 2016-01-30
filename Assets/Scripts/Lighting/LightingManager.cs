﻿using UnityEngine;
using System.Collections;

namespace GGJ2016.Lighting {
	public class LightingManager : MonoBehaviour {

		[SerializeField]
		private float flickerPeriod = 0.05f;
		[SerializeField]
		private FlickerLight[] flickerLights;
		private bool flickerOn;
		
		public float lightningLength = 0.05f;
		public Color skyboxBaseColour = new Color(0.3f, 0.3f, 0.4f);
		public Color skyboxFlashColour = new Color(0.7f, 0.7f, 0.85f);
		[SerializeField]
		private LightningLight[] lightningLights;

		private bool flickerNow = true;

		// Use this for initialization
		void Start() {
			flickerLights = FindObjectsOfType<FlickerLight>();
			lightningLights = FindObjectsOfType<LightningLight>();
			RenderSettings.ambientSkyColor = skyboxBaseColour;
			RenderSettings.ambientEquatorColor = skyboxBaseColour * 0.5f;
		}

		// Update is called once per frame
		void Update() {
			if(flickerOn && flickerNow) {
				float amount = Random.value;
				foreach(FlickerLight f in flickerLights) f.FlickerAmount = amount;
				StartCoroutine(FlickerWait());
			}

		}

		public void ToggleFlicker() {
			flickerOn = !flickerOn;
			if(!flickerOn) {
				foreach(FlickerLight f in flickerLights) f.FlickerAmount = 1f;
			}
		}

		IEnumerator FlickerWait() {
			flickerNow = false;
			yield return new WaitForSeconds(flickerPeriod * Random.Range(0.75f, 1.25f));
			flickerNow = true;
		}

		public void DoLightning() {
			Debug.Log ("hello");
			StartCoroutine(Lightning());
		}

		IEnumerator Lightning() {
			Debug.Log ("1");
			yield return Flash (lightningLength);
			yield return new WaitForSeconds (lightningLength);
			yield return Flash (lightningLength);
			yield return new WaitForSeconds (3 * lightningLength);
			yield return Flash (lightningLength);
		}

		IEnumerator Flash(float length) {
			Debug.Log ("2");
			foreach(LightningLight l in lightningLights) l.DoLightning(length);
			RenderSettings.ambientSkyColor = skyboxFlashColour;
			RenderSettings.ambientEquatorColor = skyboxFlashColour * 0.5f;
			yield return new WaitForSeconds(length);
			RenderSettings.ambientSkyColor = skyboxBaseColour;
			RenderSettings.ambientEquatorColor = skyboxBaseColour * 0.5f;
		}
	}
}