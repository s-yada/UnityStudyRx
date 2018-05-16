using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class PreGameDirector : MonoBehaviour {

    //シーン内の各オブジェクト
    public GameObject StartTex;
    public GameObject PrePlayer;

    //リソース内のスプライト用の変数
    public Sprite PrePlayerSprite;

    public int PlayerAvaFlg;

	// Use this for initialization
	void Start () {

        Debug.Log(PlayerAvaFlg);

        //BGMを止める
        BgmManager.Instance.StopImmediately();

        //プレイヤーのアバターのタイプは何かをとってくる
        PlayerAvaFlg = PlayerPrefs.GetInt("Avatar", 0);

        //プレイヤーをセットする
        SetPlayer();

        //タップされたときに移行コルーチンを呼び出す。
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))
            .FirstOrDefault()
            .Subscribe(_ => StartCoroutine(GameGo()));

    }
	
    //カウントダウンを行いゲームシーンに移行するコルーチン
    IEnumerator GameGo()
    {
        StartTex.GetComponent<Text>().text = "3";
        yield return new WaitForSeconds(1);
        StartTex.GetComponent<Text>().text = "2";
        yield return new WaitForSeconds(1);
        StartTex.GetComponent<Text>().text = "1";
        yield return new WaitForSeconds(1);
        StartTex.GetComponent<Text>().text = "GO!";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameScene");
    }
    
    void SetPlayer()
    {
        if (PlayerAvaFlg == 1)
        {
            PrePlayerSprite = Resources.Load("TestPlay2", typeof(Sprite)) as Sprite;
        }
        else if(PlayerAvaFlg == 2)
        {
            PrePlayerSprite = Resources.Load("TestPlay3", typeof(Sprite)) as Sprite;
        }
        else
        {
            PrePlayerSprite = Resources.Load("player", typeof(Sprite)) as Sprite;
        }
        
        PrePlayer.GetComponent<SpriteRenderer>().sprite = PrePlayerSprite;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
