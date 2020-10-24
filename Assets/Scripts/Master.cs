using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//特に何もしない。存在しているかだけのチェックが行われる。
public class Master : MonoBehaviour {
	private void Start() {

	}

	private void Update() {

	}
	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}
