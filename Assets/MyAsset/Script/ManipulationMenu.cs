using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Oculus.Interaction;

public class ManipulationMenu : MonoBehaviour
{

    public GameObject crossSectionPlane;
    public GameObject slicingPlane;
    //public TMP_Text currentMode;

    public GameObject leftHandAnchor, rightHandAnchor; // 컨트롤러 부착시 위치할 곳
    public GameObject CTObj; // 고정시 위치할 곳

    public GameObject CSMenu, SCMenu, RoMenu, SkMenu, ObMenu; //Mode
    public GameObject mainCanvas;

    public Slider scale, rotation;
    float scalef = 1, rotationf = 0;
    public TMP_Text scaleValue, rotationValue;




    // Start is called before the first frame update
    void Start()
    {
        
    }


    //bool isSCMenu = false;
    bool isROMenu = false;
    public static bool sketchMode = false;
    public static bool isObMenu = false;

    bool isSCFixed = false;
    bool isCSFixed = false;


    // hand gesture based

    bool ishandRotating = false, ishandSizing = false;
    int hand = Sketch.No;
    Vector3 startPos, startPos2;
    float rotDelta;
    public float rotSpeed = 10;

    public void handRotator(int h)
    {
        if (h == 0)
        {
            Debug.Log("LS Detected");
            hand = Sketch.LHand;
            startPos = leftHandAnchor.transform.position;
        }
            
        else if (h == 1)
        {
            Debug.Log("RS Detected");
            hand = Sketch.RHand;
            startPos = rightHandAnchor.transform.position;
        }

        ishandRotating = true;
    }

    public void handRotating()
    {
        
        if (hand == Sketch.LHand)
        {
            rotDelta = leftHandAnchor.transform.position.x - startPos.x;
            rotationf -= rotDelta*rotSpeed;
            //CTObj.transform.rotation = Quaternion.Euler(0, rotationf + rotDelta * 100, 0);
        }
        else if (hand == Sketch.RHand)
        {
            rotDelta = rightHandAnchor.transform.position.x - startPos.x;
            rotationf -= rotDelta * rotSpeed;
            //CTObj.transform.rotation = Quaternion.Euler(0, rotationf + rotDelta * 100, 0);
        }

        Debug.Log(rotDelta);
    }

    public void finishHandRotator()
    {
        Debug.Log("S Finished");
        ishandRotating = false;
        hand = 0;
    }

    //////////


    public bool onehand = false;

    public void handSizer(int h)
    {
        
        if (h == 0)
        {
            Debug.Log("LP Detected");
            hand = Sketch.LHand;
            startPos = leftHandAnchor.transform.position;

            if (onehand && hand != Sketch.Both) {
                hand = Sketch.Both;
                ishandSizing = true;
            }
            else
            {
                onehand = true;
            }
        }

        else if (h == 1)
        {
            Debug.Log("RP Detected");
            hand = Sketch.RHand;
            startPos2 = rightHandAnchor.transform.position;

            if (onehand && hand != Sketch.Both)
            {
                hand = Sketch.Both;
                ishandSizing = true;
            }
            else
            {
                onehand = true;
            }
        }
    }

    float lastScale;

    public void handSizing()
    {
        lastScale = startPos2.x - startPos.x;
        float curScale = rightHandAnchor.transform.position.x - leftHandAnchor.transform.position.x;
        scalef = Mathf.Max(Mathf.Min(curScale / lastScale, 3), 0.5f);
    }

    public void finishHandSizing()
    {
        Debug.Log("P Finished");
        ishandSizing = false;
        onehand = false;
        hand = 0;

    }



    //////////


    // Update is called once per frame
    void Update()
    {

        scalef = scale.value;
        //rotationf = rotation.value;

        if (ishandRotating)
        {
            handRotating();
            rotation.value = rotationf; // 슬라이더 사용 불가
        }
        else
        {
            rotationf = rotation.value; // 슬라이더 사용 가능
        }

        rotationf = rotationf % 360;
        while (rotationf < 0) rotationf += 360;

        //--------------------


        if (ishandSizing)
        {
            handSizing();
            scale.value = scalef;
        }
        else
        {
            scalef = scale.value;
        }



        changeSizeAndRotation();
    }

    void changeSizeAndRotation()
    {
        slicingPlane.transform.localScale = 0.1f * scalef * Vector3.one;

        // crossSectionPlane은 크기가 커지거나 회전되지 않는 게 좋음
        // 임시로 가장 밖으로 빼냄
        Transform cparent = crossSectionPlane.transform.parent;
        crossSectionPlane.transform.SetParent(null);
        Transform sparent = slicingPlane.transform.parent;
        slicingPlane.transform.SetParent(null);

        CTObj.transform.rotation = Quaternion.Euler(0, rotationf, 0); // 기본값 0, 0, 0
        CTObj.transform.localScale = Vector3.one * scalef;

        // 크기 조정 이후에 Parent 적용
        crossSectionPlane.transform.SetParent(cparent);
        slicingPlane.transform.SetParent(sparent);


        scaleValue.text = scalef.ToString("F2") + "x";
        rotationValue.text = rotationf.ToString("F1") + "˚";
        

        //if (isSCMenu)
        //{
        //    //crossSectionPlane.transform.SetPositionAndRotation(slicingPlane.transform.position + new Vector3(0, 0, 0.01f), slicingPlane.transform.rotation);
        //}
    }

    public void ShowHide()
    {
        mainCanvas.SetActive(!mainCanvas.activeSelf);
    }



    // Main Menu
    public void CSMode()
    {
        //currentMode.text = "Cross Section Mode";

        if (!isCSFixed) crossSectionPlane.SetActive(!CSMenu.activeSelf); // 고정 상태가 아닐 때만 숨기기

        CSMenu.SetActive(!CSMenu.activeSelf);
        //isSCMenu = CSMenu.activeSelf;
        //AttachCS();

        isObMenu = false;
        ObMenu.SetActive(false);

    }

    public void SCMode()
    {
        //currentMode.text = "CT Slicing Mode";

        if (!isSCFixed) slicingPlane.SetActive(!SCMenu.activeSelf); // 고정 상태가 아닐 때만 숨기기

        SCMenu.SetActive(!SCMenu.activeSelf);
        //isSCMenu = SCMenu.activeSelf;
        //AttachSC();

        isObMenu = false;
        ObMenu.SetActive(false);
    }

    public void RoMode()
    {
        // 판은 고정되지 않은 경우에만 숨기기
        if (!isSCFixed)
        {
            slicingPlane.SetActive(false);
        }
        if (!isCSFixed)
        {
            crossSectionPlane.SetActive(false);
        }

        isROMenu = !isROMenu;
        RoMenu.SetActive(isROMenu);

        //crossSectionPlane.gameObject.SetActive(false);
        //slicingPlane.gameObject.SetActive(false);
        //currentMode.text = "Rotation Mode";
        CSMenu.SetActive(false);

        //isSCMenu = false;
        SCMenu.SetActive(false);

        sketchMode = false;
        SkMenu.SetActive(false);
    }

    public void SkMode()
    {
        //currentMode.text = "Sketch Mode";

        // 판은 고정되지 않은 경우에만 숨기기
        if (!isSCFixed)
        {
            slicingPlane.SetActive(false);
        }
        if (!isCSFixed)
        {
            crossSectionPlane.SetActive(false);
        }
        sketchMode = !sketchMode;
        SkMenu.SetActive(sketchMode);

        isROMenu = false;
        RoMenu.SetActive(false);

        isObMenu = false;
        ObMenu.SetActive(false);

    }

    public void ObMode()
    {
        //currentMode.text = "Object Mode";

        // 판은 고정되지 않은 경우에만 숨기기
        if (!isSCFixed)
        {
            slicingPlane.SetActive(false);
        }
        if (!isCSFixed)
        {
            crossSectionPlane.SetActive(false);
        }


        SCMenu.SetActive(false);
        CSMenu.SetActive(false);
        //isSCMenu=false;

        SkMenu.SetActive(false); // 중복 입력 방지
        sketchMode = false;

        isObMenu = !isObMenu;
        ObMenu.SetActive(isObMenu);

    }

    public void ExitScene()
    {
        SceneManager.LoadScene("Mainmenu");
        //JsonUtility
        //PlayerPrefs
    }


    //Cross Section Mode
    public void FixCS() // 판 고정 및 Grab 허용
    {
        crossSectionPlane.GetComponent<GrabInteractable>().enabled = true;
        crossSectionPlane.transform.SetParent(CTObj.transform);
        isCSFixed = true;
    }

    public void AttachCS()
    {
        crossSectionPlane.GetComponent<GrabInteractable>().enabled = false;
        crossSectionPlane.transform.SetParent(rightHandAnchor.transform);
        crossSectionPlane.transform.position = rightHandAnchor.transform.position + new Vector3(0, 0, 0.1f);
        isCSFixed = false;
    }


    //CT Slicing Mode
    public void FixSC() // 판 고정 및 Grab 허용
    {

        slicingPlane.GetComponent<GrabInteractable>().enabled = true;
        slicingPlane.transform.SetParent(CTObj.transform);
        isSCFixed = true;
    }

    public void AttachSC()
    {
        slicingPlane.GetComponent<GrabInteractable>().enabled = false;
        slicingPlane.transform.SetParent(rightHandAnchor.transform);
        slicingPlane.transform.position = rightHandAnchor.transform.position + new Vector3(0, 0, 0.1f);
        isSCFixed = false;
    }





}
