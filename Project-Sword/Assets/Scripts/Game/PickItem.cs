using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickItem : MonoBehaviour
{
    [SerializeField] PhotonView view;

    [SerializeField] LayerMask itemMask;
    [SerializeField] Camera playerCamera;
    [SerializeField] float detectionCapsuleLength;
    [SerializeField] float detectionCapsuleRadius;
    [SerializeField] Transform handTransform;
    [SerializeField] GameObject stonePrefab;

    void Start()
    {

    }

    void Update()
    {
        if (!view.IsMine)
            return;

        checkPickableItem();
    }

    void checkPickableItem()
    {
        Collider[] colliders = Physics.OverlapCapsule(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * detectionCapsuleLength, detectionCapsuleRadius, itemMask);

        if (Input.GetKeyDown(KeyCode.E) && colliders.Length != 0)
        {
            view.RPC("DestroyCollectedItemRPC", RpcTarget.MasterClient, colliders[0].gameObject.GetComponent<PhotonView>().ViewID);
            GameObject spawnedObject = PhotonNetwork.Instantiate(stonePrefab.name, handTransform.position, Quaternion.identity);
            spawnedObject.transform.SetParent(handTransform);
        }
    }

    [PunRPC]
    void DestroyCollectedItemRPC(int id)
    {
        if(PhotonView.Find(id) != null)       //we need to do this check because the object might get destroyed during propagation
            PhotonNetwork.Destroy(PhotonView.Find(id));
    }
}
