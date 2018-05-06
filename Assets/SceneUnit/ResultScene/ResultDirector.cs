using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;


public class ResultDirector : MonoBehaviour {

    public GameObject ResultScoreTex;
    public GameObject HighScoreTex;
                  
    // Use this for initialization
	void Start () {
                
        /////////////////////スコアの集計処理///////////////////////

        //計算用変数に保存されているスコアを代入
        int highScore = PlayerPrefs.GetInt("1stScore", 0);
        int secondScore = PlayerPrefs.GetInt("2ndScore", 0);
        int thirdScore = PlayerPrefs.GetInt("3rdScore", 0);
        int forthScore = PlayerPrefs.GetInt("4thScore", 0);
        int fifthScore = PlayerPrefs.GetInt("5thScore", 0);
        
        //過去のスコアと現在のスコアをまとめて集計してソート
        var CalcScoreList = new List<int>
          { highScore, secondScore, thirdScore, forthScore, fifthScore,GameDirector.score };
        var OrderScoreList = CalcScoreList.OrderByDescending(x => x);
        
        //要素を取り出すため配列に変換
        int[] OrderScoreArray = OrderScoreList.ToArray();
        
        //リザルトスコアを表示
        ResultScoreTex.GetComponent<Text>().text = "YourScore：" + GameDirector.score.ToString();
        //ベストスコアを表示
        HighScoreTex.GetComponent<Text>().text = "BestScore：" + OrderScoreArray[0];

        ///ランキングスコアの更新
        PlayerPrefs.SetInt("1stScore", OrderScoreArray[0]);
        PlayerPrefs.SetInt("2ndScore", OrderScoreArray[1]);
        PlayerPrefs.SetInt("3rdScore", OrderScoreArray[2]);
        PlayerPrefs.SetInt("4thScore", OrderScoreArray[3]);
        PlayerPrefs.SetInt("5thScore", OrderScoreArray[4]);

        ///////////////////////////////////////////////////////////




        //////////タップ時にタイトル遷移するストリーム///////////

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))
            .FirstOrDefault()
            .Subscribe(_ => ResultEnd());
            
        /////////////////////////////////////////////////////////

    }



    /// <summary>
    /// ------------以下各種メソッド-----------------------------------------///
    /// </summary>

    //スコアの初期化とタイトルへの遷移のメソッド
    public void ResultEnd()
    {
        GameDirector.score = 0;
        SceneManager.LoadScene("TitleScene");
    }


	// Update is called once per frame
	void Update () {
		
	}
}
