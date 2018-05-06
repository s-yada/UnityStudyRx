using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleDirector : MonoBehaviour {

    public Button StartButton;
    public Button RankingButton;
    public GameObject FadeObj;
    public GameObject SeManager;

    float fadeSpeed = 0.02f;
    float red, green, blue, alpha;
    public bool IsFadeOut = false;
    public bool IsFadeIn = true;

    AudioSource StartSE;
    AudioSource RankingSE;

    // Use this for initialization
    void Start()
    {
        //BGMを流す
        BgmManager.Instance.Play("BarabaraKokoro");

        ////////////////////SEの準備////////////////////

        AudioSource[] TitleSEs = SeManager.GetComponents<AudioSource>();
        StartSE = TitleSEs[0];
        RankingSE = TitleSEs[1];
        
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
        RankingSE.Play();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("RankingScene");
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
