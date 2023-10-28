using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalyerMovement : MonoBehaviour
{
    public float Speed;
    float h;
    float v;
    bool isHorizonMove;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move value
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //Check Button Up & Down
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp = Input.GetButtonUp("Vertical");

        //Check Horizontal Move
        //stack을 구현해서 플레이어의 가장 최신 이동을 감지해 방향을 바꾸고
        //키를 떼면 눌려있는 키중에서 가장 최근에 들어온 키대로 움직이게 하자

        if (hDown || vUp)
            isHorizonMove = true;
        else if (hUp || vDown)
            isHorizonMove = false;
    }

    private void FixedUpdate()
    {
        //Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * Speed;
    }



}
