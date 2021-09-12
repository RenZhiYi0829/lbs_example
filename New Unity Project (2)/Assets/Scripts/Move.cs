using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class Move : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    private float moveSpeed = 5;

    [SerializeField]
    Animator anim;


    private void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        float H = ETCInput.GetAxis("Horizontal");
        float V = ETCInput.GetAxis("Vertical");
        if (H != 0 || V != 0)
        {
            //Player.transform.Translate(new Vector3(H, 0, V) * Time.deltaTime * moveSpeed, Space.World);


            transform.GetComponent<MapPosManager>().MoveMap(new Vector3(H, 0, V) * Time.deltaTime * moveSpeed);


            Vector3 targetDirection = new Vector3(H, 0f, V);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            Player.transform.GetChild(0).transform.rotation = targetRotation;

            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    public float GetHirizontal()
    {
        return ETCInput.GetAxis("Horizontal");
    }

    public float GetVertical()
    {
        return ETCInput.GetAxis("Vertical"); 
    }

}

