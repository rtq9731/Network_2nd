using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// UI 스코어 표시를 위한 스크립트
public class PlayStageUIManager : MonoBehaviour
{
    [SerializeField] Text textScore = null;

    public int score = 0;
    float clearTimer = 0f;

    void Start()
    {
        textScore.text = score.ToString();
        clearTimer = 0f;
    }

    private void Update()
    {
        clearTimer += Time.deltaTime;
    }

    public void RefreshTextScore(int gain)
    {
        score += gain;
        textScore.text = score.ToString();
    }

    public void GoStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
        FindObjectOfType<FirebaseManager>().SaveStageClearData(clearTimer);
    }
}
