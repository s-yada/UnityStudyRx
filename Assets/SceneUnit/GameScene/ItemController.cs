using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ItemController : MonoBehaviour {
    


    // Use this for initialization
    void Start () {

        //衝突時にアイテムを消す処理
        this.OnCollisionEnter2DAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);
    }
	
	// Update is called once per frame
	void Update () {

        //落下の処理
        transform.Translate(0, -0.1f, 0);

        //画面外に出たときに削除する処理
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }
                
    }
}
