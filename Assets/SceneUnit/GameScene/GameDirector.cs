using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;
using System;

public class GameDirector : MonoBehaviour {

    //***主にHPやスコアに関わる内容***//

    //オブジェクト用の変数
    public GameObject player;
    public GameObject HpGauge;
    public GameObject TimeCounta;
    public GameObject GameSeManager;

    public Button LeftButton;
    public Button RightButton;

    public static int score;
    float outTime;
    public int PlayerAvaFlg;

    public float speed = 5;

    AudioSource OneDamageSE;
    AudioSource TwoDamageSE;
    AudioSource CureSE;

    public Sprite PlayerSprite;

    // Use this for initialization
    void Start () {

        //プレイヤーアバターの設定
        PlayerAvaFlg = PlayerPrefs.GetInt("Avatar", 0);
                
        SetPlayer();
                
        //スコア・表示初期化
        TimeCounta.GetComponent<Text>().text = "0";
        score = 0;

        //SEの設定
        AudioSource[] GameSEs = GameSeManager.GetComponents<AudioSource>();
        OneDamageSE = GameSEs[0];
        TwoDamageSE = GameSEs[1];
        CureSE = GameSEs[2];

        ////////////////////衝突判定に関わるストリーム////////////////////
        //衝突判定_矢
        player.OnCollisionEnter2DAsObservable()
        .Select(x => x.gameObject.tag)
        .Where(x => x == "Arrow")
        .Subscribe(_ => DecreaseHp())
        .AddTo(this);
        
        //衝突判定_強矢
        player.OnCollisionEnter2DAsObservable()
        .Select(x => x.gameObject.tag)
        .Where(x => x == "Rarrow")
        .Subscribe(_ => DecreaseHp2())
        .AddTo(this);

        //衝突判定_回復
        player.OnCollisionEnter2DAsObservable()
        .Select(x => x.gameObject.tag)
        .Where(x => x == "Rescue")
        .Subscribe(_ => IncreaseHp())
        .AddTo(this);
        ////////////////////////////////////////////////////////////////



        //////////1秒ごとにスコア加算するストリーム///////////

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(_ => CountUp())
            .AddTo(player);

        /////////////////////////////////////////////////////



        /////////一定時間画面外に出ていると被ダメするストリーム群/////////

        Vector3 playerPos = player.transform.position;

        //画面外に出た時に画面外Ptを加算する
        Observable.EveryUpdate()
            .Subscribe(_ => OutWindow())
            .AddTo(player);

        //画面外Ptが一定以上貯まった時にダメージを与える
        Observable.EveryUpdate()
            .Where(_ => outTime > 0.7f)
            .Subscribe(_ => DecreaseHpOutWindow())
            .AddTo(this);

        /////////////////////////////////////////////////////////////////



        /////////残りのHPに応じてゲージの色を変えるストリーム////////////

        HpGauge.UpdateAsObservable()
            .Where(_ => HpGauge.GetComponent<Image>().fillAmount <= 0.2f)
            .Subscribe(_ => HpGauge.GetComponent<Image>().color = Color.red)
            .AddTo(this);

        /////////////////////////////////////////////////////////////////



        //////////HPが0になった時に終了処理を行うストリーム//////////////

        HpGauge.UpdateAsObservable()
            .Where(_ => HpGauge.GetComponent<Image>().fillAmount < 0.09f)
            .FirstOrDefault()
            .Subscribe(_ => StartCoroutine(EndCoroutine()));

        ////////////////////////////////////////////////////////////////



        /////////////////プレイヤーの操作に関わるストリーム//////////////

        //ボタンを押すと左へ行く
        LeftButton.OnPointerDownAsObservable()
            .Subscribe(player => LeftMove())
            .AddTo(this);

        //ボタンを押すと右へ行く
        RightButton.OnPointerDownAsObservable()
            .Subscribe(player => RightMove())
            .AddTo(this);

        ////////////////////////////////////////////////////////////////

    }

    /// <summary>
    /// ------------以下各種メソッド-----------------------------------------///
    /// </summary>

    //ダメージメソッド_矢
    public void DecreaseHp()
    {
        OneDamageSE.Play();
        HpGauge.GetComponent<Image>().fillAmount -= 0.1f;
    }
    
    //ダメージメソッド_強矢
    public void DecreaseHp2()
    {
        TwoDamageSE.Play();
        HpGauge.GetComponent<Image>().fillAmount -= 0.2f;
    }

    //ダメージメソッド_画面外
    public void DecreaseHpOutWindow()
    {
        DecreaseHp();
        outTime = 0;
    }

    //回復メソッド
    public void IncreaseHp()
    {
        CureSE.Play();
        HpGauge.GetComponent<Image>().fillAmount += 0.1f;
    }
        
    //スコア加算メソッド
    public void CountUp()
    {
        score += 1;
        TimeCounta.GetComponent<Text>().text = score.ToString();
    }

    //リザルト画面へ遷移させる
    public void CallResultScene()
    {
        SceneManager.LoadScene("ResultScene");
    }

    //プレイヤーのオブジェクトを破壊する
    public void PlayerDestroy()
    {
        Destroy(player);
    }

    //画面外にいるときPtを加算するメソッド
    public void OutWindow()
    {
        Vector3 playerPos = player.transform.position;
        if (playerPos.x < -9.9f || playerPos.x > 9.9f)
        {
            outTime += Time.deltaTime;
        }
    }

    //終了処理コルーチン
    IEnumerator EndCoroutine()
    {
        PlayerDestroy();
        yield return new WaitForSeconds(1.5f);
        CallResultScene();
    }

    //プレイヤーを左に動かすメソッド
    public void LeftMove()
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * -1.0f, 0);
    }

    //プレイヤーを右に動かすメソッド
    public void RightMove()
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 1.0f, 0);
    }

    //プレイヤーのアバターをシーン上に生成するメソッド
    public void SetPlayer()
    {
        if (PlayerAvaFlg == 1)
        {
            PlayerSprite = Resources.Load("TestPlay2", typeof(Sprite)) as Sprite;
            player.GetComponent<SpriteRenderer>().sprite = PlayerSprite;
        }
        else if (PlayerAvaFlg == 2)
        {
            PlayerSprite = Resources.Load("TestPlay3", typeof(Sprite)) as Sprite;
            player.GetComponent<SpriteRenderer>().sprite = PlayerSprite;
        }
        //else if (PlayerAvaFlg == 0)
        //{
        //    PlayerSprite = Resources.Load("player", typeof(Sprite)) as Sprite;
        //    player.GetComponent<SpriteRenderer>().sprite = PlayerSprite;
        //}
        else
        {
            PlayerAvaFlg = 0;
            PlayerSprite = Resources.Load("player", typeof(Sprite)) as Sprite;
            player.GetComponent<SpriteRenderer>().sprite = PlayerSprite;
        }
        
    }


    // Update is called once per frame
    void Update () {
        
        
    }
}

