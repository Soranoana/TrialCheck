using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class Check : MonoBehaviour {

	public GameObject textObj;
	private Trial trial;

	private void Awake() {
		trial = Trial.Instance.GetComponent<Trial>();
		if (trial.IsTrial()) {
			textObj.GetComponent<Text>().text = "This is Trial version.";
		} else {
			textObj.GetComponent<Text>().text = "This is Master version.";
		}
	}
}
