using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SyringeMeasurement : MonoBehaviour
{
    // �ֻ�� ��� UI�� ���� TextMeshPro ������Ʈ
    public GameObject insertionDepth;
    public GameObject insertionAngle;

    // �ֻ�� pin���� ���(pinsupport �α�)�� long, �ϴܿ� short ��ġ
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
        // Raycast�� ����Ǵ� �Ÿ��� �� RayPoint ������ �Ÿ��� ����
        distOfRayPoints = Vector3.Distance(shortRayPoint.transform.position, longRayPoint.transform.position);
        maxDistance = distOfRayPoints * 2;
       
        Debug.Log("distOfRayPoints" + distOfRayPoints);
        Debug.Log("maxDistance" + maxDistance);
        Debug.Log("Get insertionDepth Object : " + insertionDepth.name);
        Debug.Log("Get insertionAngle Object : " + insertionAngle.name);
        Debug.Log("Get shortRayPoint Object : " + shortRayPoint.name);
        Debug.Log("Get longRayPoing Object : " + longRayPoint.name);
        
        // UI�� Depth, Angle �ؽ�Ʈ�� ���������� �νĵǴ��� Ȯ��
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


    // ���� �������� ���� ������� ���� �ʿ�
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
        
        // �ֻ�Ⱑ ���ԵǾ��� ��
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
