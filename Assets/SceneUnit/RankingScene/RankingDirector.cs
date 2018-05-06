using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class RankingDirector : MonoBehaviour {

    public Button TitleButton;
    public Button DeleteButton;
    public Button DeleteCancelButton;
    public Button DeleteOkButton;

    public GameObject PopUp;

    public GameObject FirstScore;
    public GameObject SecondScore;
    public GameObject ThirdScore;
    public GameObject ForthScore;
    public GameObject FifthScore;

    // Use this for initialization
    void Start () {

        DisAppearPopUp();

        //////////////////////////////ランキングの表示////////////////////////////////////

        FirstScore.GetComponent<Text>().text = "1st：" + PlayerPrefs.GetInt("1stScore");
        SecondScore.GetComponent<Text>().text = "2nd：" + PlayerPrefs.GetInt("2ndScore");
        ThirdScore.GetComponent<Text>().text = "3rd：" + PlayerPrefs.GetInt("3rdScore");
        ForthScore.GetComponent<Text>().text = "4th：" + PlayerPrefs.GetInt("4thScore");
        FifthScore.GetComponent<Text>().text = "5th：" + PlayerPrefs.GetInt("5thScore");

        //////////////////////////////////////////////////////////////////////////////////


        /////////////////////各ボタンのストリーム////////////////////////////
        
        /////タイトルへ戻る
        TitleButton.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => BackToTitle());
        
        /////データ消去ポップアップ表示
        DeleteButton.onClick.AsObservable()
            .Subscribe(_ => AppearPopUp())
            .AddTo(this);

        /////データ消去_キャンセル
        DeleteCancelButton.onClick.AsObservable()
            .Subscribe(_ => DisAppearPopUp())
            .AddTo(this);

        /////データ消去_OK
        DeleteOkButton.onClick.AsObservable()
            .Subscribe(_ => DataClear())
            .AddTo(this);

        ////////////////////////////////////////////////////////////////////////
    }


    /// <summary>
    /// ------------以下各種メソッド-----------------------------------------///
    /// </summary>

    //タイトルに戻る
    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    //ポップアップ表示
    public void AppearPopUp()
    {
        PopUp.SetActive(true);
    }

    //ポップアップ消去
    public void DisAppearPopUp()
    {
        PopUp.SetActive(false);
    }

    //データ消去処理
    public void DataClear()
    {
        PlayerPrefs.DeleteAll();
        BackToTitle();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
