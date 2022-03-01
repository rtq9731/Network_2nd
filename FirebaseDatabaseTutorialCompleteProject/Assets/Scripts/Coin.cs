using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �߰��� ���� ��ũ��Ʈ
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
