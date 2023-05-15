using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;


// Eye-GazeHand with Quest Pro
public class GazeHand : MonoBehaviour
{
    public GameObject ovrCameraRig, centerEyeAnchor, leftHandAnchor, rightHandAnchor;           // ��ü, ��ü�� ��� ��
    public GameObject localCamera, localLeftHandAnchor, localRightHandAnchor, localPivot;       // ����, ���� ��
    public GameObject handGrabInteractorL, handGrabInteractorR;                                 // ��ü ��� ����
    HandGrabInteractable selectedObjectL, selectedObjectR;                                      // ���� ��ü

    public EyeTest eye;
    public Transform pointer;
    public float dist = 0.1f;

    float length;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        createPointer();
        setHand();

    }




    Collider lastCollider;

    void createPointer()
    {
        
        GazeRay ray = eye.centerEyeRay;

        RaycastHit[] raycastHits =  Physics.RaycastAll(ray.origin, ray.direction);
        System.Array.Sort(raycastHits, (a, b) => {      // �Ÿ� ������������ ����
            if (a.distance < b.distance) return 1;      // ��������: -1
            else if (a.distance > b.distance) return -1; // ��������: 1
            else return 0;
        });
        

        // ���� ��� �ִ� ��ü
        selectedObjectL = handGrabInteractorL.GetComponent<HandGrabInteractor>().SelectedInteractable;
        selectedObjectR = handGrabInteractorR.GetComponent<HandGrabInteractor>().SelectedInteractable;

        if(selectedObjectL != null || selectedObjectR != null)
            Debug.Log($"Selected: {(selectedObjectL != null ? selectedObjectL.gameObject.name : "None")}, " +
            $"{(selectedObjectR != null ? selectedObjectR.gameObject.name : "None")}");

        // �ϳ� �̻� �浹
        if (raycastHits.Length > 0)
        {
            int index = raycastHits.Length-1;       // ����! �������� ��!      ī�޶� -->  4   3   2   1   0

            string s = "";
            for(int i=0; i<raycastHits.Length; i++)
            {
                s += i + ": " + raycastHits[i].collider.gameObject.name + "\t";
            }
            Debug.Log(s);

            // ���� �ִ� ��� ������Ʈ �˻�
            do
            {
                if (raycastHits[index].collider.gameObject.CompareTag("User")                  // ����� ���� �ν� X

                    // ���� �ٷ� �ڷ� ������ ������ �Ʒ� 2�� �ּ� Ǯ��
                //|| (selectedObjectL != null && raycastHits[index].collider.gameObject == selectedObjectL.gameObject)        // �޼� ��ü
                //|| (selectedObjectR != null && raycastHits[index].collider.gameObject == selectedObjectR.gameObject)       // ������ ��ü
                )
                {
                    index--;
                    continue;
                }
                break;

            } while (index >= 0);

            if(index < 0)    // No raycasted except user && grabbed
            {
                pointer.position = ray.origin + ray.direction * length;
                //pointer.LookAt(ray.direction);
            }
            else                                // raycasted
            {
                if(raycastHits[index].collider != lastCollider)
                {
                    length = raycastHits[index].distance;
                    ray.length = length;
                    
                    /* - ray.direction * 0.05f*/
                    //pointer.LookAt(raycastHits[index].normal);
                    // Debug.Log(raycastHits[0].collider.gameObject.name);
                }
                //pointer.position = raycastHits[index].point;
                pointer.position = ray.origin + ray.direction * length;
                lastCollider = raycastHits[index].collider;
            }

           
        }
        else    // No raycasted
        {
            pointer.position = ray.origin + ray.direction * length;
            //pointer.LookAt(ray.direction);
        }

        pointer.LookAt(localPivot.transform);
    }

    void setHand()
    {   
        // Gaze pointer�� ��ü(=���� ��) �̵�
        ovrCameraRig.transform.position = pointer.transform.position - eye.centerEyeRay.direction * dist;

        // ī�޶��� ��ġ = ����ϴ� ������ + (���� �� ��ġ - ���� ī�޶� ��ġ)
        // �� ���� ����
        localCamera.transform.SetPositionAndRotation(
            centerEyeAnchor.transform.position - ovrCameraRig.transform.position + localPivot.transform.position,
            centerEyeAnchor.transform.rotation);   // ��ü�� ���� ȸ��

        // ���� �� ��ġ�� �̵�
        localLeftHandAnchor.transform.SetPositionAndRotation(
            leftHandAnchor.transform.position - ovrCameraRig.transform.position + localPivot.transform.position,
            leftHandAnchor.transform.rotation);
        localRightHandAnchor.transform.SetPositionAndRotation(
            rightHandAnchor.transform.position - ovrCameraRig.transform.position + localPivot.transform.position,
            rightHandAnchor.transform.rotation);

        //offset = gazePointer.transform.position - localCamera.transform.position;
        //Debug.Log(offset);
    }


}
