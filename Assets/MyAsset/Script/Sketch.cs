using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class Line
{
    public List<Vector3> position = new List<Vector3>();
    public Color color = Color.red;
    public float width = 4; // �����δ� 0.02 (1~10 == 0.005~0.05)
}


[System.Serializable]
public enum MyColor { red, orange, yellow, green, blue, black }

public class Sketch : MonoBehaviour
{
    public Material[] MatColors;

    public GameObject linePrefab;
    LineRenderer lineRenderer;
    bool isDrawing = false;

    // Tube Renderer by good man
    TubeRenderer tube;

    // ��ġ �ľ�
    public GameObject rightHandAnchor, leftHandAnchor;
    public GameObject CTObj;
    public GameObject lThumb, rThumb;

    // ���� ��
    public GameObject currentLine;
    public Line currentLineData;

    // ��ü ����
    public List<GameObject> lines = new List<GameObject>();
    public List<Line> linesData = new List<Line>();

    // �ɼ�
    public Color currentColor;
    public MyColor currentMeshColor;
    public Slider width;
    public TMP_Text widthValue;

    // Start is called before the first frame update
    void Start()
    {
        currentColor = Color.red;
        currentMeshColor = MyColor.red;
       
    }

    float timer = 0; // ����ġ�� ª�� Ŭ���� ����ġ�� �������� �ʵ��� �ϱ� ���� �ð� ����

    int hand = 0; // 1: Left, 2: Right
    public const int No = 0, LHand = 1, RHand = 2, Both = 3;


    bool isHandSketch = false;

    public void LHandThumb()
    {
        Debug.Log("LT Detected");
        hand = LHand;
        if (isTrue()) startDrawing();
        isHandSketch = true;
    }
    public void RHandThumb()
    {
        Debug.Log("RT Detected");
        hand = RHand;
        if (isTrue()) startDrawing();
        isHandSketch = true;
    }

    public void LHandThumbEnd()
    {
        if(ManipulationMenu.sketchMode && hand == LHand)
        {
            finishDrawing();
            isHandSketch = false;
        }
    }
    public void RHandThumbEnd()
    {
        if (ManipulationMenu.sketchMode && hand == RHand)
        {
            finishDrawing();
            isHandSketch = false;
        }
    }

    public bool isTrue()
    {
        return ManipulationMenu.sketchMode && !isDrawing;
    }

    public void startDrawing()
    {
        isDrawing = true;
        currentLineData = new Line();
        currentLineData.color = currentColor;
        currentLine = CreateLine();
    }

    public void finishDrawing()
    {
        // �׸��� ����ÿ� �߰�
        linesData.Add(currentLineData);
        isDrawing = false;

        // Tube
        tube = currentLine.GetComponent<TubeRenderer>();
        tube.SetRadius(width.value / 200);
        tube.SetPositions(currentLineData.position.ToArray());

        currentLine.GetComponent<MeshRenderer>().material = MatColors[getMeshColorIndex(currentMeshColor)];

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);

        //currentLine.GetComponent<MeshCollider>().sharedMesh = mesh; // �� ����� collider
        //currentLine.GetComponent<MeshFilter>().mesh = mesh; // ���� �� ������ �ٲٰų� Ư��ȿ���� �ַ��� ��� ����

        // �޽� ����
        currentLine.GetComponent<MeshCollider>().sharedMesh = tube.GetMesh();
        currentLine.GetComponent<MeshFilter>().sharedMesh = tube.GetMesh();


        // ����ġ�� ª�� Ŭ���� ����ġ�� �������� �ʵ��� ��(����)
        if (timer < 0.3) Destroy(currentLine);
        // ����ġ ������ Ÿ�̸� �ʱ�ȭ
        timer = 0;
        hand = No;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));

        // ����ġ�� ª�� Ŭ���� ����ġ�� �������� �ʵ��� �ϱ� ���� �ð� ����
        if(isDrawing) timer+=Time.deltaTime;

        widthValue.text = width.value.ToString("F2");


        // ��Ʈ�ѷ� ����ġ
        // �׸��� ����
        if( isTrue() &&
            (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5 || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5))
        {
            Debug.Log("aaa");

            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger)) // Right
                hand = RHand;
            else hand = LHand;

            startDrawing();
        }

        // �׸��� ��
        if (isDrawing)
        {
            AddPoint();
            Debug.Log("Drawing");

            // �׸��� ����
            if ( !isHandSketch &&
                ((hand == RHand && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.5)
                || (hand == LHand && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.5)))
            {
                finishDrawing();
            }
        }

        if (!ManipulationMenu.sketchMode && isDrawing)
        {
            finishDrawing();
            isHandSketch = false;
        }



    }

    public GameObject CreateLine()
    {
        GameObject line = Instantiate(linePrefab);
        line.transform.SetParent(CTObj.transform);


        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;
        lineRenderer.startWidth = width.value / 100;
        lineRenderer.endWidth = width.value / 100;




        lines.Add(line);
        return line;
    }


    // Sketch

    //Debug.Log("RTouch");

    // ����ġ world space�� CT ������Ʈ�� local space�� ��ȯ
    // CT Object�� Center Eye Anchor �� �� ��ü position�� 0,0,0, ȸ���� 0,0,0�̹Ƿ� ����

    //Debug.Log("---------------------");
    //Debug.Log(rightHandAnchor.transform.position);
    //Debug.Log(transform.InverseTransformPoint(rightHandAnchor.transform.position));
    //Debug.Log(CTObj.transform.InverseTransformPoint(rightHandAnchor.transform.position));
    //Debug.Log(rightHandAnchor.transform.InverseTransformPoint(rightHandAnchor.transform.position));

    // rightHandAnchor.transform.position: World
    // OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch): Local
    // transform.InverseTransformPoint(rightHandAnchor.transform.position): World to Local
    public void AddPoint()
    {
        if(hand == RHand &&
            (isHandSketch || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5))
        {
            if (isHandSketch)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount++, rThumb.transform.position);
                currentLineData.position.Add(rThumb.transform.position);
            }
            else
            {
                lineRenderer.SetPosition(lineRenderer.positionCount++, rightHandAnchor.transform.position);
                currentLineData.position.Add(rightHandAnchor.transform.position);
            }


        }
        if (hand == LHand &&
            (isHandSketch || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.5))
        {

            if (isHandSketch)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount++, lThumb.transform.position);
                currentLineData.position.Add(lThumb.transform.position);
            }
            else
            {
                lineRenderer.SetPosition(lineRenderer.positionCount++, leftHandAnchor.transform.position);
                currentLineData.position.Add(leftHandAnchor.transform.position);
            }


        }

    }


    public int getMeshColorIndex(MyColor color)
    {
        switch (currentMeshColor)
        {
            case MyColor.red: return 0;
            case MyColor.orange: return 1;
            case MyColor.yellow: return 2;
            case MyColor.green: return 3;
            case MyColor.blue: return 4;
            default: return 5;
        }


    }

    public void PickColor(int color)
    {

        switch (color)
        {
            case 0:
                currentColor = new Color(1, 0, 0); // NOT 255
                currentMeshColor = MyColor.red;
                break;
            case 1:
                currentColor = new Color(1, 0.5f, 0);
                currentMeshColor= MyColor.orange;
                break;
            case 2:
                currentColor = new Color(1, 1, 0);
                currentMeshColor = MyColor.yellow;
                break;
            case 3:
                currentColor = new Color(0, 1, 0);
                currentMeshColor = MyColor.green;
                break;
            case 4:
                currentColor = new Color(0, 0, 1);
                currentMeshColor = MyColor.blue;
                break;
            case 5:
            default:
                currentColor = new Color(0, 0, 0);
                currentMeshColor = MyColor.black;
                break;

        }

    }


    public void asdf(MyColor color)
    {

    }
}
