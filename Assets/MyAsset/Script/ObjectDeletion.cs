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
        // interactor 상태 파악
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




        // 물체 삭제
        if (!isPressed && (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three)))
        { // A키나 X키
            isPressed = true;

            //ICollection<RayInteractor> list = gameObject.GetComponent<RayInteractable>().Interactors;
            if (list.Count == 1)
            {
                foreach (RayInteractor interactor in list)
                {
                    Debug.Log(interactor.End); // 포인터의 위치 찾기

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


    // PointableCanvasModule 관련 오류 발생중
    // 원래는 Object 파괴를 하려 했으나 앱이 멈추는 관계로 setActive만 false
    public void showDelBt(Vector3 position)
    {

        // 삭제 버튼
        if (delBt != null) Destroy(delBt);
        delBt = Instantiate(delBtPrefab);

        // Vector3 bound = GetComponent<Collider>().bounds.extents;
        // Debug.Log(bound);
        delBt.transform.position = position + new Vector3(0, 0, -0.02f);
        Debug.Log(delBt.transform.position);

        //delBt.transform.SetParent(gameObject.transform);
      
        GameObject panel = delBt.transform.GetChild(0).GetChild(0).gameObject;
        panel.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => removeObj());



        // 복사 버튼
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
        delBt.SetActive(false); // 3초 뒤에 사라짐
        copyBt.SetActive(false); // 3초 뒤에 사라짐
    }

    public void removeObj()
    {
        copyBt.SetActive(false);
        delBt.SetActive(false);
        //Destroy(copyBt, 0.1f);
        //Destroy(delBt, 0.1f); // 오류 방지
        //gameObject.SetActive(false);
        Destroy(gameObject); //Object랑 독립적이므로 상관 없는듯
    }

    public void copyObj()
    {
        delBt.SetActive(false);
        copyBt.SetActive(false);
        //Destroy(copyBt, 0.1f);
        //Destroy(delBt, 0.1f); // 오류 방지

        // 크기 변동 방지를 위해, 임시로 분리시키기
        Transform cparent = transform.parent;
        transform.SetParent(null);

        GameObject newObj = Instantiate(gameObject,
            gameObject.transform.position + new Vector3(0.4f, 0, 0), gameObject.transform.rotation);
        ObjectPlacement.objects.Add(newObj);
        newObj.transform.SetParent(ObjectPlacement.CTObj_s.transform);  // 모델과 함께 움직이기


        transform.SetParent(cparent);
    }


}
