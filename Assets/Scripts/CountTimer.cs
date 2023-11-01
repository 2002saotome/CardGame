using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountTimer : MonoBehaviour
{
    //�J�E���g�_�E��
    private float CountDown = 10.0f;

    //���Ԃ�\������Text�^�̕ϐ�
    public Text timeText;

    //�N���b�N���ꂽ���ǂ���
    private bool isClick = false;

    // Update is called once per frame
    void Update()
    {
        //�N���b�N���ꂽ���ǂ���
        if (isClick)
        {
            CountDown -= Time.deltaTime;
        }

        //�N���b�N�����u��
        if (Input.GetMouseButton(0))
        {
            //���Ԃ��J�E���g����
            CountDown -= Time.deltaTime;
            //���Ԃ�\������
            timeText.text = CountDown.ToString("f1") + "�b";
            isClick = true;
        }

        if (CountDown < 0)
        {
            timeText.text="���Ԃ��o���܂���";
        }
    }
}