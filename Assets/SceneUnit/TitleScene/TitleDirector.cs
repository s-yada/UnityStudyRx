﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleDirector : MonoBehaviour {
    
    //ゲームオブジェクトの変数
    public Button StartButton;
    public Button RankingButton;
    public Button SettingButton;
    public GameObject FadeObj;
    public GameObject SeManager;

    //フェードに関わる変数
    float fadeSpeed = 0.02f;
    float red, green, blue, alpha;
    public bool IsFadeOut = false;
    public bool IsFadeIn = true;

    //SE用変数
    AudioSource StartSE;
    AudioSource MoveSE;

    //プレイヤーアバターを決定するステータス


    // Use this for initialization
    void Start()
    {
        //BGMを流す
        BgmManager.Instance.Play("TitleBGM");

        ////////////////////SEの準備////////////////////

        AudioSource[] TitleSEs = SeManager.GetComponents<AudioSource>();
        StartSE = TitleSEs[0];
        MoveSE = TitleSEs[1];
        
        ////////////////////////////////////////////////


        ////////////////////フェードの準備////////////////////

        red = FadeObj.GetComponent<Image>().color.r;
        green = FadeObj.GetComponent<Image>().color.g;
        blue = FadeObj.GetComponent<Image>().color.b;
        alpha = FadeObj.GetComponent<Image>().color.a;

        //////////////////////////////////////////////////////

        
        ////////////////////フェード用ストリーム////////////////////

        /////フェードイン
        this.UpdateAsObservable()
            .Where(_ => IsFadeIn == true)
            .Subscribe(_ => FadeIn());

        /////フェードアウト
        this.UpdateAsObservable()
            .Where(_ => IsFadeOut == true)
            .Subscribe(_ => FadeOut());

        ////////////////////////////////////////////////////////////


        ////////////////////ボタン押下時のストリーム////////////////////

        /////スタートボタン
        StartButton.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => StartCoroutine(GameStartCoroutine()));

        /////ランキングボタン
        RankingButton.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => StartCoroutine(RankingStart()));

        /////セッティングボタン
        SettingButton.onClick.AsObservable()
            .FirstOrDefault()
            .Subscribe(_ => StartCoroutine(SettingStart()));

        ///////////////////////////////////////////////////////////////
    }


    /// <summary>
    /// ------------------以下、各種メソッド---------------------------------///
    /// </summary>
    
    //フェードインメソッド
    void FadeIn()
    {
        alpha -= fadeSpeed;
        SetColor();
        if (alpha <= 0)
        {                    
            IsFadeIn = false;
            FadeObj.GetComponent<Image>().enabled = false;
        }
    }
    
    //フェードアウトメソッド
    public void FadeOut()
    {
        FadeObj.GetComponent<Image>().enabled = true;
        alpha += fadeSpeed;
        SetColor();
        if (alpha >= 1)
        {
            IsFadeOut = false;
        }
    }
    
    //色、アルファをセットするメソッド
    public void SetColor()
    {
        FadeObj.GetComponent<Image>().color = new Color(red, green, blue, alpha);
    }

    //ゲーム準備シーンへ遷移するメソッド
    public void GameStart()
    {
        SceneManager.LoadScene("PreGameScene");
    }

    //ランキングシーンへ遷移するメソッド
    IEnumerator RankingStart()
    {
        MoveSE.Play();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("RankingScene");
    }

    //キャラ選択シーンへ遷移するメソッド
    IEnumerator SettingStart()
    {
        MoveSE.Play();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PlayerSelectScene");
    }


    //スタートボタン押下時のコルーチン
    IEnumerator GameStartCoroutine()
    {
        IsFadeOut = true;
        StartSE.Play();
        BgmManager.Instance.Stop();
        yield return new WaitForSeconds(1.5f);
        GameStart();
    }

    // Update is called once per frame
    void Update () {
        
	}
}
