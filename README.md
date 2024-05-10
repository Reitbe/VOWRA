![배너.png](/ReadMeSource/banner.png)

- 태그 : 1인칭, VR, 시뮬레이션
- 사용 기술 : Oculus Integration SDK, Plastic SCM, Unity 3D
- 팀 구성 : 프로그래머 4명(3D 스케치 / 볼륨 렌더링 / 물체 상호작용 / 네트워크)
- 작업 기간 : 2022년 9월 3일 → 2022년 11월 21일
<br>

## ❗개요
- **VR을 활용한 3D 의료영상 시뮬레이터**
    
     CT, MRI와 같은 3D 의료영상을 2D 모니터가 아닌 3D VR 환경에서 관찰하고 상호작용하는 프로그램입니다. 
    
- 소프트웨어중심사업단 산학협력프로젝트
    
     의과대학 산학협력 프로젝트의 일환으로, 의료인을 위한 온라인 가상환경 회의 시스템의 기초 상호작용 레벨로써 제작되었습니다.
<br>

## 📜 주요 기능

### 다양한 입력 지원

- 핸드 트래킹 / 컨트롤러를 사용한 상호작용을 지원합니다.
- 개별 물체 생성 / 확대 및 축소 / 회전 / 복사 / 삭제 기능을 지원합니다.

### 인체 모델 뷰어

- https://github.com/mlavik1/UnityVolumeRendering 을 활용하여 여러 장의 사진으로 구성된 의료 영상을 단일 오브젝트로 제작합니다.
- 인체 모델의 단면을 볼 수 있는 뷰어를 지원합니다.
- 모델과 뷰어 모두 상호작용이 가능합니다.

### 3D 스케치

- 3D 공간에서의 스케치 및 스케치의 오브젝트화를 통한 상호작용이 가능합니다.
<br>


## 🖥️ 본인 작업 내용

### 기본 상호 작용 테스트

 Oculus Integration SDK를 사용하여 Unity와 Oculus Quest 2의 연동 테스트를 진행했습니다. 컨트롤러를 사용한 플레이어의 이동과 오브젝트 상호작용을 위한 근거리, 원거리 그랩 동작을 시험했습니다.

### 상호작용 오브젝트 개발
 인체 모델 오브젝트와 상호작용하기 위한 주사기 오브젝트를 개발하였습니다. 해당 오브젝트를 인체 모델에 삽입하였을 때, 그 각도와 깊이를 주사기 상단의 UI에 표시합니다.  

```cs
// 주사기 코드 중 일부(SyringeMeasurement.cs)
// 주사기 바늘의 양 끝에 위치한 포인트
public GameObject shortRayPoint;
public GameObject longRayPoint;
RaycastHit shortHit;
RaycastHit longHit;
float distOfRayPoints;
float maxDistance;
...

void Start()
{
    // 바늘의 길이 초기화
    distOfRayPoints = Vector3.Distance(shortRayPoint.transform.position, longRayPoint.transform.position);
    maxDistance = distOfRayPoints * 2;
	...
}

void Update()
{
    isShortRay = Physics.Raycast(shortRayPoint.transform.position, (-1) * shortRayPoint.transform.up, out shortHit, maxDistance);
    isLongRay = Physics.Raycast(longRayPoint.transform.position, (-1) * longRayPoint.transform.up, out longHit, maxDistance);
	
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
```
- 주사기 바늘의 양 끝에 위치를 지정하고, 해당 위치에서 Raycast를 진행합니다.
- 거리 측정은 바늘의 길이에서 인체 모델에 삽입하고 남은 길이를 뺀 값을 사용합니다.
- 각도 측정은 바늘이 인체 모델에 삽입된 포인트의 법선 벡터와 바늘 삽입 벡터 사이의 값을 사용합니다.
<br>


## 🕹️ 기능 이미지
- 절단면 뷰어
    
    ![절단면 뷰어.png](/ReadMeSource/viewer1.png)
    
- 절단면 CT/MRI 뷰어
    
    ![절단면 CTnMRI.png](/ReadMeSource/viewer2.png)
    
- 3D 스케치
    
    ![3D 스케치.png](/ReadMeSource/3Dsketch.png)
    
- 주사기 오브젝트
    
    ![주사기.png](/ReadMeSource/syringe.png)
<br>    


## 💡 성장 경험

### 협업은 솔직하게

팀의 목표를 달성하기 위해서는 팀원을 존중하고 솔직하게 소통하는 과정이 필요합니다. 저는 본 프로젝트를 진행하며 이를 크게 느꼈습니다.

 협업 경험이 부족한 상태에서 시작된 프로젝트에서는 많은 문제가 발생했습니다. 통일되지 않은 개발 환경, 플러그인 버전 차이 등의 문제가 있었지만, 개발 초기에 발생한 VR 환경에서의 테스트 문제가 가장 기억에 남습니다.

 Unity에서 작업한 내용을 VR 환경에서 테스트하고자 장비를 연결했을 때, 오로지 회색 공간만이 출력되는 문제입니다. 이후에 발견한 원인은 노트북에 장착된 2개의 그래픽카드가 자동으로 전환되는 기능 때문이었습니다. 내장 그래픽을 비활성화하여 문제를 해결하였습니다.

 하지만 그 과정에서 시간이 상당히 지연되었습니다. 문제를 공유하지 않고 홀로 해결하였기 때문입니다. 함께 작업하는 동료들에게 저의 부족함을 드러내는 것이 두려웠고, 이런 문제조차 혼자서 해결하지 않는다는 인상을 주고 싶지 않았습니다. 결과적으로 제가 담당한 부분의 개발이 미뤄지며 팀에게 악영향을 끼치고 말았습니다. 

 이 경험을 반성하며 저는 새로운 목표를 세웠습니다. 오로지 팀의 목표에 집중할 것. 스스로가 부족한 점을 인정하고 솔직하게 대화하며 문제 상황을 해결해 나가겠다고 결심했습니다. 또한 Unity 오픈소스 프로젝트의 참여 가이드라인을 참고하여 협업에 필요한 정보들을 조사하기도 했습니다.

 이러한 시도는 다음 프로젝트인 Escape Hospital에서 효과를 발휘하였습니다. 문제가 발생하더라도 부담 없는 의사소통과 작업 내용에 대한 지속적인 공유를 통해 추가적인 문제를 방지할 수 있었습니다.

 그렇기에 앞으로도 교훈을 잊지 않고 협업 과정에서 솔직하고 확실하게 의사소통해 나갈 것입니다.

### Unity 기초 사용 경험

 Unity의 기본 사용법을 학습하고 프로젝트에 적용하였습니다. UI, Rigidbody 등은 에디터에서 작업하였으며, 값의 제어는 코드로 처리했습니다.
