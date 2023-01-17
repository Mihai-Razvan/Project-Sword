using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraLook : MonoBehaviour
{
    PhotonView photonView;

    [SerializeField] Camera playerCamera;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    float rotationSpeed;
    float xRotation;

    void Start()
    {
        photonView = this.gameObject.transform.parent.gameObject.GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            Destroy(this.gameObject.GetComponent<AudioListener>());
            playerCamera.enabled = false;     //we don't destroy it because we might add spectate or smth like this in the future
        }

        xRotation = 0;
        rotationSpeed = 150;
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        rotateCamera();
    }

    public void rotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
