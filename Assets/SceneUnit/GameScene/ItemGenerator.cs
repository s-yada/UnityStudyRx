using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class ItemGenerator : MonoBehaviour {

    public GameObject ArrowPrefab;
    public GameObject RarrowPrefab;
    public GameObject RescuePrefab;

    public GameObject HpGauge;

    float arrowSpan;
    float arrowSpan2;
    float arrowSpan3;
    float rarrowSpan;
    float rarrowSpan2;
    float rescueSpan;

    int px;

	// Use this for initialization
	void Start () {

        arrowSpan = 1.5f;
        arrowSpan2 = 3.2f;
        arrowSpan3 = 4.3f;
        rarrowSpan = 2.5f;
        rarrowSpan2 = 4.2f;
        rescueSpan = 5.0f;
        

        //矢の生成_ゲームレベル1
        Observable.Interval(TimeSpan.FromSeconds(arrowSpan))
            .Subscribe(_ =>CreateItem(ArrowPrefab))
            .AddTo(this);

        //強矢の生成_ゲームレベル2
        Observable.Interval(TimeSpan.FromSeconds(rarrowSpan))
            .Where(_ => GameDirector.score >= 10)
            .Subscribe(_ => CreateItem(RarrowPrefab))
            .AddTo(this);

        //矢の生成_ゲームレベル3
        Observable.Interval(TimeSpan.FromSeconds(arrowSpan2))
            .Where(_ => GameDirector.score >= 20)
            .Subscribe(_ => CreateItem(ArrowPrefab))
            .AddTo(this);

        //強矢の生成_ゲームレベル4
        Observable.Interval(TimeSpan.FromSeconds(rarrowSpan2))
            .Where(_ => GameDirector.score >= 30)
            .Subscribe(_ => CreateItem(RarrowPrefab))
            .AddTo(this);

        //矢の生成_ゲームレベル5
        Observable.Interval(TimeSpan.FromSeconds(arrowSpan3))
            .Where(_ => GameDirector.score >= 40)
            .Subscribe(_ => CreateItem(ArrowPrefab))
            .AddTo(this);

        //矢の生成_ゲームレベル6
        Observable.Interval(TimeSpan.FromSeconds(arrowSpan))
            .Where(_ => GameDirector.score >= 50)
            .Subscribe(_ => CreateItem(ArrowPrefab))
            .AddTo(this);
                       
        //回復アイテムの生成
        Observable.Interval(TimeSpan.FromSeconds(rescueSpan))
            .Where(_ => HpGauge.GetComponent<Image>().fillAmount < 0.5f)
            .Subscribe(_ => CreateItem(RescuePrefab))
            .AddTo(this);

        
    }
	
    
    
    //アイテム生成メソッド
    void CreateItem(GameObject Prefab)
    {
        GameObject go = Instantiate(Prefab) as GameObject;
        px = UnityEngine.Random.Range(-6, 7);
        go.transform.position = new Vector3(px, 7, 0);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
