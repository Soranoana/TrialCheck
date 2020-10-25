using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.Build;
//using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;

public class Trial : MonoBehaviour/*, IPreprocessBuildWithReport, IPostprocessBuildWithReport, IProcessSceneWithReport*/
{

	[Tooltip("体験版ならtrue\n正規版ならfalse")]
	public bool isTrial;    //体験版判定

	private static bool? isTrialStatic = null;  //一度でも検索したら覚えとく用

	//private Component master;   //体験版アクティブ判定用スクリプト
	public Object[] deleteList;  //ビルド時に削除するスクリプト 
	private string[] deleteObjectPath;  //削除されるオブジェクトのパス
	private string[] tmpObjectPath; //削除されるオブジェクトの退避先
	private static GameObject myInstance;   //DontDestroyOnload後に別シーンから呼び出される用

	public static GameObject Instance {
		get {
			return myInstance;
		}
	}

	public void OnPreprocessBuild(/*UnityEditor.BuildTarget target, string path*/) {
		deleteObjectPath = new string[deleteList.Length];
		tmpObjectPath = new string[deleteList.Length];
		Debug.Log("ビルドするよ！");
		//体験版なら
		if (isTrial) {
			//削除リストが空でないなら
			if (deleteList.Length > 0) {
				//順番に中身のパスを取得し削除していく
				for (int i = 0; i < deleteList.Length; i++) {
					//拡張子を取得
					string fileExtension = System.IO.Path.GetExtension(AssetDatabase.GetAssetPath(deleteList[i]));
					//パスを取得
					//string l_path = AssetDatabase.GetAssetPath(deleteList[i]);
					//元のパスを取得
					deleteObjectPath[i] = AssetDatabase.GetAssetPath(deleteList[i]) + fileExtension;
					//退避先のパスを取得
					tmpObjectPath[i] = "StreamingAssetsTemp/" + deleteList[i].name + fileExtension;

					//パスを使って削除
					//AssetDatabase.DeleteAsset(l_path);
					//パスを使って退避
					System.IO.Directory.Move(deleteObjectPath[i], tmpObjectPath[i]);
				}
			}
		}
	}

	// ビルド後処理
	public void OnPostprocessBuild(/*BuildTarget target, string path*/) {
		Debug.Log("ビルドしたよ！");
		//退避先から元の場所に戻す
		//体験版なら
		if (isTrial) {
			//削除リストが空でないなら
			if (deleteList.Length > 0) {
				//順番に中身のパスを取得し戻していく
				for (int i = 0; i < deleteList.Length; i++) {
					System.IO.Directory.Move(tmpObjectPath[i], deleteObjectPath[i]);
				}
			}
		}
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
		//保存してなかったら確認
		if (isTrialStatic == null) {
			//すべてのコンポーネントを取得
			MonoBehaviour[] monoBehaviours = GetComponents<MonoBehaviour>();
			//その中にMasterという名前があったらfalse
			foreach (var monoBehaviour in monoBehaviours) {
				if (monoBehaviour.GetType().Name == "Master") {
					isTrialStatic = false;
					return false;
				}
			}
			//なかったらtrue
			isTrialStatic = true;
			return true;
		} else {
			//保存していたらそれを返す
			return (bool)isTrialStatic;
		}

		/*
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
				}*/
	}

}
