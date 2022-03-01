using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 결과 데이터 저장을 위해 수정
public class PlayerMove : MonoBehaviour
{
    [SerializeField] float playerSpeed = 0f;
    [SerializeField] float jumpPower = 0f;
    bool canJump = false;

    Rigidbody2D rigid = null;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            rigid.velocity = new Vector2(playerSpeed, rigid.velocity.y);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity = new Vector2(-playerSpeed, rigid.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent.name == "Platforms")
            canJump = true;
        else if (collision.gameObject.name.Contains("Finish"))
            FindObjectOfType<PlayStageUIManager>().GoStageSelect();
    }
}
