using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView pv;
    private CharacterController myCC;
    public float movementSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine == true)
        {
            BasicMovement();
            BasicRotation();
        }
    }

    private void BasicMovement()
    {
        if (Input.GetKey(KeyCode.W) == true) {
            myCC.Move(transform.forward * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.A) == true)
        {
            myCC.Move(-transform.right * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            myCC.Move(-transform.forward * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            myCC.Move(transform.right * Time.deltaTime * movementSpeed);
        }
    }

    private void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, mouseX, 0));
    }
}
