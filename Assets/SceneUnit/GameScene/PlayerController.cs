using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class PlayerController : MonoBehaviour {

    public Button LeftButton;
    public Button RightButton;

    public float speed = 5;

    // Use this for initialization
    void Start () {

        //ボタンを押すと左へ行く
        LeftButton.OnPointerDownAsObservable()
            .Subscribe(player => LeftMove())
            .AddTo(this);

        //ボタンを押すと右へ行く
        RightButton.OnPointerDownAsObservable()
            .Subscribe(player => RightMove())
            .AddTo(this);

    }

    //左に動くメソッド
    public void LeftMove()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * -1.0f, 0);
    }

    //右に動くメソッド
    public void RightMove()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * 1.0f, 0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
