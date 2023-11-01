using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountTimer : MonoBehaviour
{
    //カウントダウン
    private float CountDown = 10.0f;

    //時間を表示するText型の変数
    public Text timeText;

    //クリックされたかどうか
    private bool isClick = false;

    // Update is called once per frame
    void Update()
    {
        //クリックされたかどうか
        if (isClick)
        {
            CountDown -= Time.deltaTime;
        }

        //クリックした瞬間
        if (Input.GetMouseButton(0))
        {
            //時間をカウントする
            CountDown -= Time.deltaTime;
            //時間を表示する
            timeText.text = CountDown.ToString("f1") + "秒";
            isClick = true;
        }

        if (CountDown < 0)
        {
            timeText.text="時間が経ちました";
        }
    }
}