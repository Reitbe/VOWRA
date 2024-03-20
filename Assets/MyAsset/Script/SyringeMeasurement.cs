using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SyringeMeasurement : MonoBehaviour
{
    // 주사기 상단 UI에 들어가는 TextMeshPro 오브젝트
    public GameObject insertionDepth;
    public GameObject insertionAngle;

    // 주사기 pin에서 상단(pinsupport 부근)에 long, 하단에 short 배치
    public GameObject shortRayPoint;
    public GameObject longRayPoint;
    
    RaycastHit shortHit;
    RaycastHit longHit;

    float distOfRayPoints;
    float maxDistance;

    bool isShortRay = false;
    bool isLongRay = false;

    // Start is called before the first frame update
    void Start()
    {
        // Raycast가 적용되는 거리는 두 RayPoint 사이의 거리와 동일
        distOfRayPoints = Vector3.Distance(shortRayPoint.transform.position, longRayPoint.transform.position);
        maxDistance = distOfRayPoints * 2;
       
        Debug.Log("distOfRayPoints" + distOfRayPoints);
        Debug.Log("maxDistance" + maxDistance);
        Debug.Log("Get insertionDepth Object : " + insertionDepth.name);
        Debug.Log("Get insertionAngle Object : " + insertionAngle.name);
        Debug.Log("Get shortRayPoint Object : " + shortRayPoint.name);
        Debug.Log("Get longRayPoing Object : " + longRayPoint.name);
        
        // UI의 Depth, Angle 텍스트가 정상적으로 인식되는지 확인
        try
        {
            Debug.Log("InsertionDepth text : " + insertionDepth.GetComponent<TextMeshProUGUI>().text);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("in InsertionDepth, GetComponent<TextMeshProUGUI>() return NULL");
        }
        try
        {
            Debug.Log("InsertionAngle text : " + insertionAngle.GetComponent<TextMeshProUGUI>().text);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("in InsertionAngle, GetComponent<TextMeshProUGUI>() return NULL");
        }
    }


    // 보다 직관적인 각도 계산으로 변경 필요
    public static float GetAngle(Vector3 from, Vector3 to) 
    {
        Vector3 v = to - from;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * (-1) * maxDistance, Color.red, 1);

        isShortRay = Physics.Raycast(shortRayPoint.transform.position, (-1) * shortRayPoint.transform.up, out shortHit, maxDistance);
        isLongRay = Physics.Raycast(longRayPoint.transform.position, (-1) * longRayPoint.transform.up, out longHit, maxDistance);
        
        // 주사기가 삽입되었을 때
        if(!isShortRay && isLongRay)
        {
            try
            {
                insertionDepth.GetComponent<TextMeshProUGUI>().text = "Depth : " + (Mathf.Round(1000 * (distOfRayPoints - longHit.distance))).ToString() + "mm";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Can't change InsertionDepth Text");
            }
            try
            {
                insertionAngle.GetComponent<TextMeshProUGUI>().text = "Angle : " + Mathf.Round((GetAngle((-1) * transform.up, longHit.normal))).ToString() + "deg";
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Can't change IntertionAngle Text");
            }
        }
    }
}
