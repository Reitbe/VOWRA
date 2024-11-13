![배너.png](/ReadMeSource/banner.png)

- 태그 : 1인칭, VR, 시뮬레이션
- 사용 기술 : Oculus Integration SDK, Plastic SCM, Unity 3D
- 팀 구성 : 프로그래머 4명(3D 스케치 / 볼륨 렌더링 / 물체 상호작용 / 네트워크)
- 작업 기간 : 2022년 9월 3일 → 2022년 11월 21일
<br>

## ❗개요
- **VR을 활용한 3D 의료영상 시뮬레이터**
    
     CT, MRI와 같은 3D 의료영상을 2D 모니터가 아닌 3D VR 환경에서 관찰하고 상호작용하는 프로그램입니다. 소프트웨어중심사업단 산학협력프로젝트의 일환으로 제작하였습니다.
<br>


## 🖥️ 본인 작업 내용

### 기본 상호 작용 테스트

 Oculus Integration SDK를 사용하여 Unity와 Oculus Quest 2의 연동 테스트를 진행했습니다. 컨트롤러를 사용한 플레이어의 이동과 오브젝트 상호작용을 위한 근거리, 원거리 그랩 동작을 시험했습니다.

### 상호작용 오브젝트 개발
 인체 모델 오브젝트와 상호작용하기 위한 주사기 오브젝트를 개발하였습니다. 해당 오브젝트를 인체 모델에 삽입하였을 때, 그 각도와 깊이를 주사기 상단의 UI에 표시합니다.  

```cs
// 주사기 코드 중 일부(SyringeMeasurement.cs)
public GameObject shortRayPoint; // 주사기 바늘의 시작점
public GameObject longRayPoint;  // 주사기 바늘의 끝점
RaycastHit shortHit;
RaycastHit longHit;
float distOfRayPoints;
...

void Start()
{
    // 주사기 바늘의 길이 지정
    distOfRayPoints = Vector3.Distance(shortRayPoint.transform.position, longRayPoint.transform.position);
	...
}

void Update()
{
    isShortRay = Physics.Raycast(shortRayPoint.transform.position, (-1) * shortRayPoint.transform.up, out shortHit, maxDistance);
    isLongRay = Physics.Raycast(longRayPoint.transform.position, (-1) * longRayPoint.transform.up, out longHit, maxDistance);
    ...

    try
    {
        insertionDepth.GetComponent<TextMeshProUGUI>().text = "Depth : " + (Mathf.Round(1000 * (distOfRayPoints - longHit.distance))).ToString() + "mm";
    }
    catch (NullReferenceException ex)
    {
        Debug.Log("Can't change InsertionDepth Text");
    }
}
```
- 주사기 바늘의 시작점과 끝점에서 각각 Raycast를 수행해 대상과의 거리를 측정합니다.
- 바늘 길이에서 인체 모델에 삽입 후 남은 길이를 빼어 삽입 깊이를 계산합니다.
- 삽입 각도는 삽입 지점의 법선 벡터와 바늘의 방향 벡터 사이 각도를 이용해 측정합니다.
<br>

## 📜 주요 기능

### 다양한 입력 지원

- 핸드 트래킹 / 컨트롤러를 사용한 상호작용을 지원합니다.
- 개별 물체 생성 / 확대 및 축소 / 회전 / 복사 / 삭제 기능을 지원합니다.

### 인체 모델 뷰어

- 볼륨 렌더링 기술을 이용하여 여러 장의 사진으로 구성된 의료 영상을 단일 오브젝트로 제작합니다.
- 인체 모델의 단면을 볼 수 있는 뷰어를 지원합니다. 모델과 뷰어 모두 상호작용이 가능합니다.

### 3D 스케치

- 3D 공간에서의 스케치 및 스케치의 오브젝트화를 통한 상호작용이 가능합니다.
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

팀의 목표를 달성하기 위해서는 팀원을 존중하고 솔직하게 소통하는 과정이 필요하다는 것을 해당 프로젝트를 진행하며 배웠습니다. 

 이 프로젝트의 문제점은 팀원 모두의 협업 경험이 부족했다는 것이었습니다. 개발 초기 회의에서 몇 가지 논의가 생략되었고, 장비 환경과 플러그인 버전 등이 통일되지 않은 상태에서 작업이 진행되었습니다. 결국 문제가 발생했습니다. VR 환경에서 테스트를 진행하고자 장비를 노트북에 연결했을 때, 오로지 회색 공간만이 출력되었던 것입니다. 이후에 발견한 원인은 노트북에 장착된 2개의 그래픽카드가 자동으로 전환되는 기능 때문이었습니다.

 서로의 장비를 가지고 모여 테스트를 진행했다면 빠르게 해결할 수 있었던 문제였습니다. 하지만 저는 문제점을 공유하지 않은 채 홀로 문제를 해결하려 했고, 오히려 원인을 찾는 과정에서 시간이 지연되어 팀에 충분히 기여하지 못하는 결과를 초래했습니다. 

 이 경험을 계기로 몇 가지 목표를 세웠습니다. 오로지 팀의 목표에 집중할 것. 스스로의 부족한 점을 인정하고 솔직하게 대화하며 문제 상황을 해결해 나가겠다고 결심했습니다. 이를 위해 오픈소스 프로젝트의 참여 가이드라인을 참고하며 협업에 필요한 정보들을 조사했습니다.

 이러한 노력은 다음 프로젝트에서 큰 효과를 발휘하였습니다. 팀 내에 통일된 작업 환경과 원활한 의사소통 방식이 자리 잡았고, 주기적인 결과 공유를 통해  문제 발생을 방지할 수 있었습니다. 결과적으로 프로젝트는 성공적으로 마무리되었으며 성과 발표회에서 동상을 수상하는 기쁨을 누릴 수 있었습니다.


