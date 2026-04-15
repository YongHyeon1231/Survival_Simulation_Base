Terrain Asset
- https://assetstore.unity.com/packages/p/idyllic-fantasy-nature-260042

Particle
- https://assetstore.unity.com/packages/p/cartoon-fx-remaster-free-109565

---

## 코드 스켈레톤 구조

### 폴더 구조

```
Assets/00_Scripts/
├── Main/
│   ├── Cam_Movement.cs          MonoBehaviour
│   ├── M_Object.cs              MonoBehaviour  ← 상호작용 오브젝트 베이스
│   ├── Interaction_Hit.cs       M_Object 상속
│   ├── Item.cs                  MonoBehaviour
│   ├── P_Movement.cs            MonoBehaviour  싱글턴
│   ├── P_Finder.cs              MonoBehaviour
│   └── P_Handler.cs             MonoBehaviour
├── Manager/
│   ├── Game_Mng.cs              MonoBehaviour  싱글턴 (뼈대만)
│   ├── Delegate_Holder.cs       MonoBehaviour  이벤트 버스
│   ├── Asset_Mng.cs             순수 C# (정적 유틸)
│   ├── ItemFlowController.cs    순수 C# (드롭 확률 계산)
│   └── Enum_Holder.cs           enum 정의만
├── Scriptable/
│   ├── Object_Scriptable.cs     ScriptableObject  (+ITEMLIST 내부 클래스)
│   └── Item_Scriptable.cs       ScriptableObject
├── UI/
│   ├── Canvas_Holder.cs         MonoBehaviour  싱글턴
│   └── UI_Animation_Handler.cs  MonoBehaviour
└── Particle_Handler.cs          MonoBehaviour  싱글턴
```

---

### 클래스 계층

```
MonoBehaviour
├── Cam_Movement          카메라 Lerp 추적
├── M_Object              ★ 채집 오브젝트 베이스 (HP, 파괴, UI 연동)
│   └── Interaction_Hit   타격 흔들림, 드롭 생성
├── Item                  드롭 아이템 분산 → 흡착 → 자기파괴
├── P_Movement            플레이어 이동/회전/애니메이션, 상호작용 잠금
├── P_Finder              범위 탐지, F키 상호작용 발동
├── P_Handler             Hit() 애니메이션 이벤트 → M_Object HP 감소
├── Delegate_Holder       OnInteraction / OnInteractionOut static 이벤트
├── Game_Mng              싱글턴 뼈대 (로직 없음)
├── Canvas_Holder         HP바 UI, Board HUD 관리
├── UI_Animation_Handler  Animator Trigger 래퍼
└── Particle_Handler      오브젝트 파괴 파티클 재생

ScriptableObject
├── Object_Scriptable     오브젝트 데이터 (HP, 드롭 테이블)
└── Item_Scriptable       아이템 데이터 (ID, 이름, 타입, 희귀도)

순수 C#
├── Asset_Mng             SpriteAtlas 정적 로더
├── ItemFlowController    확률 기반 드롭 아이템 결정
└── ITEMLIST              드롭 항목 데이터 (Item_Scriptable + 확률)
```

---

### 핵심 이벤트 흐름

```
[F키]
P_Finder → M_Object.Interaction()
  → P_Handler.m_Object 지정
  → Delegate_Holder.OnStartInteraction()
      ├─ P_Movement: 이동 잠금
      ├─ Canvas_Holder: HP바 HUD 표시
      └─ P_Finder: 아이콘 숨김

[애니메이션 이벤트 Hit()]
P_Handler.Hit()
  → m_Object.HP -= 20
  → M_Object.HP_Init()
      → Canvas_Holder.BoardFill() (HP바 갱신)
      → [HP <= 0]
          → Particle_Handler: 파괴 파티클
          → Destroy(gameObject)
          → ItemFlowController: 확률 드롭 판정
          → Instantiate(Item) x N개: 분산 → 플레이어 흡착
          → Delegate_Holder.OnOutInteraction()
              ├─ P_Movement: 이동 잠금 해제
              ├─ Canvas_Holder: HUD 숨김
              └─ P_Finder: 탐지 재개
```

---

## 핵심 이벤트 흐름 상세설명

<details>
<summary>[F키] 상호작용 시작</summary>

```
P_Finder.Update()
  → Physics.OverlapSphere(position, 5.0f, interactableLayer)
        ← 반경 5.0f 내 interactableLayer 오브젝트 전부 탐색
  → 탐색된 Collider 중 activationDistance(3.0f) 이하 & 가장 가까운 것 → closetObject

  → ShowIcon(closetObject)
        → Instantiate(IconPrefab, uiCanvas.transform)  ← 아이콘 생성 (최초 1회)
        → UpdateIconPosition(targetTransform, iconInstance)
              → Camera.main.WorldToScreenPoint(target.position + (0, 1.5f, 0))
                    ← 월드 좌표 → 스크린 좌표 변환
              → Icon.RectTransform.position = screenPosition  ← 매 프레임 아이콘 위치 갱신

  [F키 입력 감지]
  → closetObject.GetComponent<M_Object>().Interaction()
        ↓ Interaction_Hit.Interaction() 오버라이드 실행
        → P_Movement.instance.AnimationChange(m_Data.m_Type.ToString())
              → animator.SetTrigger(m_Data.m_Type)  ← 오브젝트 타입에 맞는 애니메이션 전환
        → base.Interaction()  [M_Object.Interaction()]
              → P_Handler.m_Object = this  ← Hit() 타격 대상 지정
              → GetInteraction = true
              → HP_Init()
                    → [HP > 0] Canvas_Holder.instance.BoardFill(HP, m_Data.HP)
                          → BoardHpFill.fillAmount = HP / maxHP  ← HP바 즉시 갱신
                          → FillCoroutine() 시작
                                ← 매 프레임 BoardHpWhiteFill을 BoardHpFill 쪽으로 Lerp (잔상 효과)

  → Delegate_Holder.OnStartInteraction()  ← OnInteraction static 이벤트 발동 → 구독자 일괄 호출
        ├─► P_Movement (구독자)
        │     → animator.SetBool("NoneInteraction", true)  ← 이동 애니메이션 잠금
        │     → animator.SetFloat("a_Speed", 0.0f)         ← 속도 0으로 고정
        │     → Update()에서 Finder.OnInteraction == true → Move(), RotateTowardsMouse() 건너뜀
        │     → 상호작용 중 F 외 아무 키 입력 → Delegate_Holder.OnOutInteraction() 호출 (강제 탈출)
        │
        ├─► Canvas_Holder.GetBoard() (구독자)
        │     → Board.SetActive(true)  ← HP바 HUD 활성화
        │
        └─► P_Finder.OnInteractionVoid() (구독자)
              → OnInteraction = true       ← Update() 탐색 루프 차단
              → transform.LookAt(closetObject.position)  ← 플레이어가 오브젝트 방향으로 즉시 회전
              → closetObject = null
              → IconInit()                ← 아이콘 Out 애니메이션 후 activeIcons에서 제거
```

</details>

<details>
<summary>[애니메이션 이벤트 Hit()] 타격 처리</summary>

```
[Animator가 Hit() 이벤트 호출]

P_Handler.Hit()
  → m_Object.HP -= 20

  → pos 계산 (히트 파티클 생성 위치)
        x = m_Object.x + Random.Range(-0.5f, 0.5f)
        y = m_Object.y + 1.5f
        z = m_Object.z + Random.Range(-0.5f, 0.5f)
  → Instantiate(HitParticle, pos, Quaternion.identity)  ← 오브젝트 위쪽 주변 랜덤 위치에 파티클 생성

  → m_Object.OnHit()
        ↓ Interaction_Hit.OnHit() 오버라이드 실행

        ├─ [HP > 0]
        │     → ShakeTree(오브젝트 위치 - 플레이어 위치)
        │           → oppositeDirection = -attackDirection.normalized  ← 공격 반대 방향
        │           → targetRotation = Quaternion.Euler(
        │                   x: -oppositeDirection.z * 5.0f,
        │                   y: 0,
        │                   z:  oppositeDirection.x * 5.0f)
        │           → StopAllCoroutines()  ← 이전 흔들림 중단
        │           → ShakeAnimation(targetRotation) 코루틴 시작
        │                 → 0.25초: originalRotation → targetRotation  Slerp
        │                 → 0.25초: targetRotation → originalRotation  Slerp
        │                 → transform.rotation = originalRotation  ← 원위치
        │
        └─ [HP <= 0]
              → ItemFlowController.DROPITEMLIST(m_Data.Drop_Items)
                    → Drop_Items 리스트 순회
                    → 각 항목마다 Random.Range(0.0f, 100.0f) vs item.value 비교
                    → 통과한 항목만 Get_Item_List에 추가 후 반환
              → 반환 리스트 수만큼 루프
                    → Instantiate(item_Prefab, 오브젝트 위치, Quaternion.identity)
                          ↓ Item.Start()
                          → SpreadAndMoveToPlayer() 코루틴
                                → spreadDirection = Random.insideUnitSphere * 10.0f
                                → spreadPosition.y = Max(y, 5.0f)  ← 지면 관통 방지
                                → 0.3초: 생성 위치 → spreadPosition  Lerp (분산)
                                → MoveToPlayer(spreadPosition) 코루틴
                                      → 매 루프 endPosition = player.position + (0, 1, 0)
                                      → journeyTime = 거리 / 8.0f  ← 속도 기반 시간
                                      → 매 프레임 플레이어 위치 갱신하며 Lerp 추적
                                      → 거리 < 0.5f 도달 시 루프 탈출
                                      → Instantiate(GetParticle, 현재 위치, Quaternion.identity)
                                      → Destroy(this.gameObject)

        → base.OnHit()  [M_Object.OnHit() → HP_Init()]  ← 마지막 호출
              → [HP <= 0]
                    → HP = 0  ← 언더플로우 방지
                    → Particle_Handler.instance.OnParticle(자식[0].MeshRenderer)
                          → transform.position = meshRenderer 위치로 이동
                          → shape.meshRenderer = meshRenderer  ← 파티클 쉐이프를 오브젝트 메시로 교체
                          → particleSystem.Play()
                    → Canvas_Holder.instance.AllStopCoroutine()  ← 잔상 코루틴 강제 중지
                    → Canvas_Holder.instance.BoardHpWhiteFill.fillAmount = 1.0f
                    → Destroy(gameObject)  ← 오브젝트 파괴 예약 (프레임 말에 실행)
                    → Delegate_Holder.OnOutInteraction()  ← OnInteractionOut 이벤트 발동
                          ├─► P_Movement (구독자)
                          │     → animator.SetBool("NoneInteraction", false)  ← 이동 잠금 해제
                          ├─► Canvas_Holder.BoardOut() (구독자)
                          │     → UI_Animation_Handler.AnimationChange("Out")  ← HUD Out 애니메이션
                          └─► P_Finder.OnInteractionOut() (구독자)
                                → OnInteraction = false  ← 탐색 재개
                                → activeIcons.Clear()
```

</details>

---

## 개선 필요 항목

<details>
<summary>1. Interaction_Hit.cs — base.OnHit() 호출 순서</summary>

현재 코드에서 `base.OnHit()`을 먼저 호출하면 `HP_Init()` → `Destroy(gameObject)` 예약이 실행된 뒤,
파괴 예약된 오브젝트에서 `ShakeTree`가 `StartCoroutine`을 시도함. Unity는 이를 조용히 무시해서 흔들림 애니메이션이 재생되지 않음.

```csharp
// 현재 (문제)
public override void OnHit()
{
    base.OnHit();       // Destroy 예약 발생
    ShakeTree(...);     // 파괴 예약 오브젝트에서 코루틴 시작 → 무시됨
    if(HP <= 0) { 드롭 처리 }
}

// 수정
public override void OnHit()
{
    if(HP > 0) ShakeTree(...);      // HP > 0일 때만 흔들림
    if(HP <= 0) { 드롭 처리 }
    base.OnHit();                    // 마지막에 Destroy 예약
}
```

</details>

<details>
<summary>2. Canvas_Holder.cs — FillCoroutine 역방향 조건 미처리</summary>

나무를 70% 깎다가 상호작용 해제 후, 다른 풀피 오브젝트를 상호작용하면
`BoardHpFill`은 1.0으로 올라가지만 `BoardHpWhiteFill`은 0.3에 고정됨.
`while(white - fill > 0.001f)` 조건이 `-0.7 > 0.001f` → false라 코루틴이 즉시 탈출.

```csharp
// 현재 (문제)
IEnumerator FillCoroutine()
{
    while(BoardHpWhiteFill.fillAmount - BoardHpFill.fillAmount > 0.001f)
    { ... }
}

// 수정 - white가 fill보다 낮을 때도 즉시 맞춰줌
IEnumerator FillCoroutine()
{
    if(BoardHpWhiteFill.fillAmount < BoardHpFill.fillAmount)
    {
        BoardHpWhiteFill.fillAmount = BoardHpFill.fillAmount;
        yield break;
    }
    while(BoardHpWhiteFill.fillAmount - BoardHpFill.fillAmount > 0.001f)
    { ... }
}
```

</details>

<details>
<summary>3. Delegate_Holder.cs — static 이벤트 구독 해제 없음</summary>

`P_Movement`, `Canvas_Holder`, `P_Finder` 세 클래스 모두 `Start()`에서 `+=` 구독하지만
`OnDestroy()`에서 `-=` 해제가 없음. 씬 재로드나 오브젝트 재생성 시 이벤트가 중복 구독되어
같은 동작이 두 번씩 실행됨.

```csharp
// 수정 - 각 클래스에 OnDestroy 추가
private void OnDestroy()
{
    Delegate_Holder.OnInteraction -= (구독한 메서드);
    Delegate_Holder.OnInteractionOut -= (구독한 메서드);
}
```

</details>

<details>
<summary>4. P_Handler.cs — m_Object null 체크 없음</summary>

`Hit()`은 애니메이션 이벤트로 호출됨. 상호작용 도중 오브젝트가 파괴되거나
`OnOutInteraction`이 먼저 발동되는 타이밍에 `m_Object`가 null이 될 경우 크래시.

```csharp
// 현재 (문제)
public void Hit()
{
    m_Object.HP -= 20;  // m_Object null이면 NullReferenceException
}

// 수정
public void Hit()
{
    if(m_Object == null) return;
    m_Object.HP -= 20;
}
```

</details>

<details>
<summary>5. Asset_Mng.cs — Resources.Load 결과 null 체크 없음</summary>

`Resources/Atlas` 에셋이 없거나 경로가 바뀌면 `atlas`가 null이고,
`Get_Atlas()` 호출 시 즉시 NullReferenceException. 현재 `Get_Atlas()`가 미사용 상태라
당장 터지지 않지만 추후 사용 시 문제 발생.

```csharp
// 수정
public static Sprite Get_Atlas(string temp)
{
    if(atlas == null)
    {
        Debug.LogError("Atlas 에셋을 찾을 수 없습니다. Resources/Atlas 경로를 확인하세요.");
        return null;
    }
    return atlas.GetSprite(temp);
}
```

</details>
