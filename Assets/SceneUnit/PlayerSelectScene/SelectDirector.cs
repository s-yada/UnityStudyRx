using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UniRx;
using UniRx.Triggers;

public class SelectDirector : MonoBehaviour {
  
    public Button CharacterSetButton0;
    public Button CharacterSetButton1;
    public Button CharacterSetButton2;
    public Button TitleButton;
    public Button ConfButton;

    public GameObject PopUp;


    // Use this for initialization
    void Start () {

        //ポップアップの初期化
        PopUp.SetActive(false);

        /////////////////////各ボタンのストリーム////////////////////////////

        //タイトルへ戻る
        TitleButton.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => BackToTitle());

        //タイトルへ戻る（ポップアップのボタン）
        ConfButton.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => BackToTitle2());

        //キャラクター設定1
        CharacterSetButton0.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => SetCharacter0());

        //キャラクター設定2
        CharacterSetButton1.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => SetCharacter1());

        //キャラクター設定3
        CharacterSetButton2.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => SetCharacter2());

        ////////////////////////////////////////////////////////////////////////

    }


    /// <summary>
    /// ------------以下各種メソッド-----------------------------------------///
    /// </summary>

    //1番目のキャラクター（デフォルト）に設定する
    public void SetCharacter0()
    {
        PlayerPrefs.SetInt("Avatar", 0);
        AppearPopUp();
    }

    //2番目のキャラクターに設定する
    public void SetCharacter1()
    {
        PlayerPrefs.SetInt("Avatar", 1);
        AppearPopUp();
    }

    //3番目のキャラクターに設定する
    public void SetCharacter2()
    {
        PlayerPrefs.SetInt("Avatar", 2);
        AppearPopUp();
    }

    //タイトルへ遷移する
    public void BackToTitle()
    {        
        SceneManager.LoadScene("TitleScene");
    }

    //タイトルへ遷移する（キャラ選択時）
    public void BackToTitle2()
    {
        Advertisement.Show();
        SceneManager.LoadScene("TitleScene");
    }


    //ポップアップ表示
    public void AppearPopUp()
    {
        PopUp.SetActive(true);
    }



    // Update is called once per frame
    void Update () {
		
	}
}
