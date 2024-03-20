using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum OurObject { cube, sphere, syringe };


// ��ü�� ũ��/ȸ��/��ġ����(Transform)
[System.Serializable]
public class ObjInfo
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
}


public class ObjectPlacement : MonoBehaviour
{

    public OurObject currentPrefab;
    public GameObject cubePrefab, spherePrefab, syringePrefab;

    // ��ġ
    public GameObject rightHandAnchor, leftHandAnchor, CTObj;
    public GameObject lThumb, rThumb;

    public static GameObject CTObj_s;

    public GameObject currentObj;
    public static List<GameObject> objects = new List<GameObject>();
    //public static List<ObjInfo> objInfo = new List<ObjInfo>();

    bool isPlaced = false;
    int hand = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentPrefab = OurObject.cube;
        CTObj_s = CTObj;
    }


    // Hand Tracking ����
    public void LHandThumb()
    {
        Debug.Log("LT Detected");
        hand = Sketch.LHand;
        pos = lThumb.transform.position;
        if(isTrue()) Spawn();
    }
    public void RHandThumb()
    {
        Debug.Log("RT Detected");
        hand = Sketch.RHand;
        pos = rThumb.transform.position;
        if (isTrue()) Spawn();
    }




    // Update is called once per frame
    void Update()
    {

        // ��ü �߰� (��Ʈ�ѷ�)
        // Ʈ���� �ֱ�
        if (isTrue() &&
            (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9 || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9))
        {
            Place();
        }

        // Ʈ���� ����
        if ((hand == Sketch.RHand && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.9)
                || (hand == Sketch.LHand && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.9))
        {
            isPlaced = false;
        }


    }

    public bool isTrue()
    {
        return ManipulationMenu.isObMenu && !isPlaced;
    }

    Vector3 pos;

    public void Place()
    {

        //ICollection<RayInteractor> list = gameObject.GetComponent<RayInteractable>().Interactors;
        //if (list.Count > 0) return;


        isPlaced = true;

        // Controller ����
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)) // right
        {
            pos = rightHandAnchor.transform.position;
            hand = Sketch.RHand;
        }
        else
        {
            pos = leftHandAnchor.transform.position;
            hand = Sketch.LHand;
        }


        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9
            && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9) {}



        Spawn();

    }

    public void Spawn()
    {
        switch (currentPrefab)
        {
            case OurObject.cube:
                currentObj = Instantiate(cubePrefab, pos, Quaternion.identity);
                break;
            case OurObject.sphere:
                currentObj = Instantiate(spherePrefab, pos, Quaternion.identity);
                break;
            case OurObject.syringe:
                currentObj = Instantiate(syringePrefab, pos, Quaternion.identity);
                break;
        }
        objects.Add(currentObj);
        currentObj.transform.SetParent(CTObj.transform); // �𵨰� �Բ� �����̱�
    }



    public void selection(int index)
    {
        switch (index)
        {
            case 0:
                currentPrefab = OurObject.cube;
                break;
            case 1:
                currentPrefab = OurObject.sphere;
                break;
            case 2:
                currentPrefab = OurObject.syringe;
                break;
        }
        Debug.Log(currentPrefab + " clicked");
    }


}
