using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangeTitle : MonoBehaviour
{
    // 中に記述された処理が一定間隔で繰り返し実行される
    void Update()
    {
        // もし入力したキーがSpaceキーならば、中の処理を実行する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // SampleSceneに切り替える
            SceneManager.LoadScene("GamePlay");
        }
    }
}
