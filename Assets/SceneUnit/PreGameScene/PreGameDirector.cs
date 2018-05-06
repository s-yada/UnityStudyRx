using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class PreGameDirector : MonoBehaviour {

    public GameObject StartTex;

	// Use this for initialization
	void Start () {

        BgmManager.Instance.StopImmediately();

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))
            .FirstOrDefault()
            .Subscribe(_ => StartCoroutine(GameGo()));

    }
	
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
    
	// Update is called once per frame
	void Update () {
		
	}
}
