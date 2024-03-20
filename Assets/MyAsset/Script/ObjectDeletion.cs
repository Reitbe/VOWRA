using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class ObjectDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    bool isPressed = false;
    GameObject delBt, copyBt;

    // Update is called once per frame
    void Update()
    {
        // interactor ���� �ľ�
        ICollection<RayInteractor> list = gameObject.GetComponent<RayInteractable>().Interactors;
        if (list.Count > 0)
        {
            foreach (RayInteractor interactor in list)
            {
                Debug.Log(interactor.name+": "+interactor.State+" on "+ interactor.Interactable.name);
                if (interactor.Interactable.GetComponent<PointableCanvas>() != null)
                {
                
                    Debug.Log("On UI");
                }
            }
        }




        // ��ü ����
        if (!isPressed && (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three)))
        { // AŰ�� XŰ
            isPressed = true;

            //ICollection<RayInteractor> list = gameObject.GetComponent<RayInteractable>().Interactors;
            if (list.Count == 1)
            {
                foreach (RayInteractor interactor in list)
                {
                    Debug.Log(interactor.End); // �������� ��ġ ã��

                    showDelBt(interactor.End);

                }
            }

        }

        if(!OVRInput.Get(OVRInput.Button.One) && !OVRInput.Get(OVRInput.Button.Three))
        {
            isPressed=false;
        }
    }


    public GameObject delBtPrefab;
    public GameObject copyBtPrefab;


    // PointableCanvasModule ���� ���� �߻���
    // ������ Object �ı��� �Ϸ� ������ ���� ���ߴ� ����� setActive�� false
    public void showDelBt(Vector3 position)
    {

        // ���� ��ư
        if (delBt != null) Destroy(delBt);
        delBt = Instantiate(delBtPrefab);

        // Vector3 bound = GetComponent<Collider>().bounds.extents;
        // Debug.Log(bound);
        delBt.transform.position = position + new Vector3(0, 0, -0.02f);
        Debug.Log(delBt.transform.position);

        //delBt.transform.SetParent(gameObject.transform);
      
        GameObject panel = delBt.transform.GetChild(0).GetChild(0).gameObject;
        panel.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => removeObj());



        // ���� ��ư
        if (copyBt != null) Destroy(copyBt);
        copyBt = Instantiate(copyBtPrefab);

        copyBt.transform.position = position + new Vector3(0, 0.16f, -0.02f);
        Debug.Log(copyBt.transform.position);

        //delBt.transform.SetParent(gameObject.transform);

        GameObject panel2 = copyBt.transform.GetChild(0).GetChild(0).gameObject;
        panel2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => copyObj());

        StartCoroutine(wait(3));
        
    }

    IEnumerator wait(float f)
    {
        yield return new WaitForSeconds(f);
        delBt.SetActive(false); // 3�� �ڿ� �����
        copyBt.SetActive(false); // 3�� �ڿ� �����
    }

    public void removeObj()
    {
        copyBt.SetActive(false);
        delBt.SetActive(false);
        //Destroy(copyBt, 0.1f);
        //Destroy(delBt, 0.1f); // ���� ����
        //gameObject.SetActive(false);
        Destroy(gameObject); //Object�� �������̹Ƿ� ��� ���µ�
    }

    public void copyObj()
    {
        delBt.SetActive(false);
        copyBt.SetActive(false);
        //Destroy(copyBt, 0.1f);
        //Destroy(delBt, 0.1f); // ���� ����

        // ũ�� ���� ������ ����, �ӽ÷� �и���Ű��
        Transform cparent = transform.parent;
        transform.SetParent(null);

        GameObject newObj = Instantiate(gameObject,
            gameObject.transform.position + new Vector3(0.4f, 0, 0), gameObject.transform.rotation);
        ObjectPlacement.objects.Add(newObj);
        newObj.transform.SetParent(ObjectPlacement.CTObj_s.transform);  // �𵨰� �Բ� �����̱�


        transform.SetParent(cparent);
    }


}
