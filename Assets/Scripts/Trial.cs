using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;

public class Trial : MonoBehaviour/*, IPreprocessBuildWithReport, IPostprocessBuildWithReport, IProcessSceneWithReport*/ {

	[Tooltip("体験版ならtrue\n正規版ならfalse")]
	public bool isTrial;    //体験版判定

	private Component master;   //体験版アクティブ判定用スクリプト
	public MonoScript[] deleteList;  //ビルド時に削除するスクリプト 
	private static GameObject myInstance;   //DontDestroyOnload後に別シーンから呼び出される用

	public static GameObject Instance {
		get {
			return myInstance;
		}
	}

	public void OnPreprocessBuild(UnityEditor.BuildTarget target, string path) {
		Debug.Log("ビルドするよ！");
		//体験版なら
		if (isTrial) {
			//削除リストが空でないなら
			if (deleteList.Length > 0) {
				//順番に中身のパスを取得し削除していく
				for (int i = 0; i < deleteList.Length; i++) {
					//パスワード取得
					string l_path = AssetDatabase.GetAssetPath(deleteList[i]);
					//パスを使って削除
					AssetDatabase.DeleteAsset(l_path);
				}
			}
		}
	}

	// ビルド後処理
	public void OnPostprocessBuild(BuildTarget target, string path) {
		Debug.Log("ビルドしたよ！");
	}

	private void Awake() {

		if (myInstance == null) {
			DontDestroyOnLoad(this.gameObject);
			myInstance = this.gameObject;
		} else {
			Destroy(this.gameObject);
		}
	}

	void Start() {
		SceneManager.LoadScene("TrialCheck");
	}

	void Update() {

	}

	public bool IsTrial() {
		//取得に失敗するか、取得しても中身が空なら体験版
		try {
			Master master = GetComponent<Master>();
			if (master != null) {
				Debug.Log("Product version");
				return false;
			} else {
				Debug.Log("Trial version");
				return true;
			}
		} catch (UnityException) {
			Debug.Log("Trial version");
			return true;
		}
	}

	//public void
}
