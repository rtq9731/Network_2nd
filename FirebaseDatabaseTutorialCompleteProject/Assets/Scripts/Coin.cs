using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코인 점수 추가를 위한 스크립트
public class Coin : MonoBehaviour
{
    [SerializeField] int score = 5;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            FindObjectOfType<PlayStageUIManager>().RefreshTextScore(score);
            gameObject.SetActive(false);
        }
    }
}
