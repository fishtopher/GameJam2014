using UnityEngine;
using System.Collections;

public static class SoundManager {

	public static void PlaySound(string soundName) {
		GameObject gameObj = GameObject.Find(soundName);
		AudioSource audioSource = gameObj.GetComponent<AudioSource>();
		audioSource.Play();
	}

	public static void Stop(string soundName) {
		GameObject gameObj = GameObject.Find(soundName);
		AudioSource audioSource = gameObj.GetComponent<AudioSource>();
		audioSource.Stop();
	}
}
