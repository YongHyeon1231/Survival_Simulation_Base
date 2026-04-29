Terrain Asset
- https://assetstore.unity.com/packages/p/idyllic-fantasy-nature-260042

Particle
- https://assetstore.unity.com/packages/p/cartoon-fx-remaster-free-109565

---

## 코드 스켈레톤 구조

### 폴더 구조

Assets/00_Scripts/
├── Main/
│   ├── Cam_Movement.cs          MonoBehaviour  싱글턴
│   ├── Character.cs             MonoBehaviour  ← 플레이어/워커 공통 베이스
│   ├── M_Object.cs              MonoBehaviour  ← 상호작용 오브젝트 베이스
│   ├── Interaction_Hit.cs       M_Object 상속
│   ├── Item.cs                  MonoBehaviour
│   ├── Monster.cs               MonoBehaviour
│   ├── P_Movement.cs            Character 상속  싱글턴
│   ├── P_Finder.cs              MonoBehaviour
│   └── Worker.cs                Character 상속
├── Manager/
│   ├── Base_Mng.cs              MonoBehaviour  싱글턴 (서브매니저 접근점)
│   ├── Game_Mng.cs              MonoBehaviour  (스태미나 관리)
│   ├── Building_Mng.cs          MonoBehaviour  (건물 배치)
│   ├── Object_Mng.cs            MonoBehaviour  (오브젝트 스폰/컬링)
│   ├── Delegate_Holder.cs       MonoBehaviour  이벤트 버스
│   ├── Asset_Mng.cs             순수 C# (정적 에셋 로더)
│   ├── ItemFlowController.cs    순수 C# (드롭 확률/인벤토리)
│   ├── Utils.cs                 MonoBehaviour  (정적 유틸)
│   └── Enum_Holder.cs           enum 정의만
├── Building/
│   ├── Building_OBJ.cs          MonoBehaviour
│   ├── BonFire.cs               M_Object 상속
│   └── Portal.cs                M_Object 상속
├── Scriptable/
│   ├── Scriptable_Base.cs       ScriptableObject  (Key, Icon 베이스)
│   ├── Object_Scriptable.cs     ScriptableObject
│   ├── Item_Scriptable.cs       ScriptableObject
│   ├── Building_Scriptable.cs   ScriptableObject
│   └── Unit_Scriptable.cs       ScriptableObject
├── UI/
│   ├── Canvas_Holder.cs         MonoBehaviour  싱글턴
│   ├── Directional_Monster_Slider.cs  MonoBehaviour
│   ├── CompassBar.cs            MonoBehaviour
│   ├── Navigation_Mng.cs        MonoBehaviour  싱글턴
│   ├── UI_Animation_Handler.cs  MonoBehaviour
│   ├── PopUP_Description.cs     MonoBehaviour
│   ├── Nav_Item.cs              MonoBehaviour
│   ├── Building_Panel.cs        MonoBehaviour
│   ├── Item_Panel.cs            MonoBehaviour
│   ├── Unit_Panel.cs            MonoBehaviour
│   └── PART/
│       ├── UIPART.cs            MonoBehaviour  (UI 패널 베이스)
│       ├── INVENTORY.cs         UIPART 상속
│       ├── BUILDING.cs          UIPART 상속
│       └── PORTAL.cs            UIPART 상속
└── Particle_Handler.cs          MonoBehaviour  싱글턴

---

### 클래스 계층

MonoBehaviour
├── Cam_Movement          카메라 Lerp 추적 + 카메라 쉐이크
├── Character             ★ 플레이어/워커 공통 베이스 (HP, 장비, 파티클, Hit/Attack 애니메이션)
│   ├── P_Movement        플레이어 이동/회전/데미지, 싱글턴
│   └── Worker            NavMesh 기반 자율 채집 유닛, 나침반 마커
├── M_Object              ★ 채집 오브젝트 베이스 (HP, 파괴, HP바 UI 연동)
│   ├── Interaction_Hit   타격 흔들림, 드롭 생성
│   ├── BonFire           상호작용 시 스태미나 회복
│   └── Portal            Worker 소환 UI 연동
├── Monster               NavMesh 추적 + 공격 + HP바
├── Item                  드롭 아이템 분산 → 흡착 → 자기파괴
├── P_Finder              범위 탐지, F키 상호작용/공격 발동
├── Delegate_Holder       OnInteraction / OnInteractionOut / OnStamina / OnHP static 이벤트
├── Base_Mng              싱글턴 - Build / Game / Object 서브매니저 접근점
├── Game_Mng              스태미나 관리
├── Building_Mng          건물 배치 (Raycast 이동, 회전, 확정)
├── Building_OBJ          건물 상태 관리 (투명→불투명, 건설 진행바, 완료 이펙트)
├── Object_Mng            오브젝트 스폰 + CullingGroup 가시성 관리
├── Canvas_Holder         HP바, Board HUD, 몬스터 슬라이더, UI 패널 토글, 데미지 텍스트
├── Directional_Monster_Slider  몬스터 HP바 개별 인스턴스 (WorldToScreenPoint 방식 - 개선 필요)
├── CompassBar            방위 나침반 + 건물/Worker 마커
├── Navigation_Mng        화면 하단 알림 (아이템 획득, 건설 알림)
├── UI_Animation_Handler  Animator Trigger 래퍼
├── UIPART                ★ UI 패널 베이스 (Open / Close / Toggle)
│   ├── INVENTORY         인벤토리 패널
│   ├── BUILDING          건설 패널
│   └── PORTAL            포탈(Worker 소환) 패널
└── Particle_Handler      오브젝트 파괴 파티클 재생

ScriptableObject
├── Scriptable_Base       공통 베이스 (Key, Icon)
│   ├── Building_Scriptable  건물 데이터 (prefab, Key, 건설 시간, 재료)
│   └── Unit_Scriptable      유닛 데이터
├── Object_Scriptable     채집 오브젝트 데이터 (HP, 드롭 테이블)
└── Item_Scriptable       아이템 데이터 (Key, 타입, 희귀도, Weight)

순수 C#
├── Asset_Mng             SpriteAtlas / Building ScriptableObject 정적 로더
├── ItemFlowController    확률 기반 드롭 + 인벤토리 Dictionary 관리
├── Utils                 Localization, 타이머 포맷, 레이어 변경 정적 유틸
└── ITEMLIST / ITEM       드롭 테이블 / 인벤토리 아이템 데이터 구조

---

### 핵심 이벤트 흐름

[F키 - 채집 오브젝트]
P_Finder → M_Object.Interaction(character)
  → character.m_Object 지정
  → Delegate_Holder.OnStartInteraction()
      ├─ P_Movement: 이동 잠금
      ├─ Canvas_Holder: HP바 HUD 표시
      └─ P_Finder: 아이콘 숨김

[애니메이션 이벤트 Hit()]
Character.Hit()
  → m_Object.HP -= 20
  → M_Object.OnHit(character)
      → [MainPlayer] Base_Mng.Game.SetStamina(-10) / CameraShake
      → HP_Init(character)
          → Canvas_Holder.BoardFill() (HP바 갱신)
          → [HP <= 0]
              → Particle_Handler: 파괴 파티클
              → Base_Mng.Object.RemoveObject()
              → Destroy(gameObject)
              → [MainPlayer] Delegate_Holder.OnOutInteraction()
              → [Worker] worker.StateChange(IDLE)
  → Interaction_Hit: ShakeTree / 드롭 생성

[F키 - 몬스터]
P_Finder → AttackMonster(colliders)
  → Character.AnimationChange("Attack")
  → [애니메이션 이벤트 Attack()] Character.Attack()
      → Monster.GetDamage(10)
          → Canvas_Holder: 데미지 텍스트 + HP바 갱신
          → [HP <= 0] Canvas_Holder.RemoveSlider() / DIE 애니메이션 / Destroy(1.5f)

[Monster 자율 추적]
Monster.FindPlayer() (1초마다) → target 지정
Monster.Update() → NavMesh 추격
  → [거리 < 2.0] AttackPlayer()
      → [애니메이션 이벤트 Attack()] Monster.Attack()
          → P_Movement.GetDamage(15)

---

### 버그

Object_Mng.cs — `Random.Range` 오프-바이-원으로 마지막 오브젝트 타입이 절대 생성되지 않음.

  [현재] m_Datas[Random.Range(0, m_Datas.Length - 1)]  ← 마지막 인덱스 제외
  [수정] m_Datas[Random.Range(0, m_Datas.Length)]

---

## 핵심 이벤트 흐름 상세설명

<details>
<summary>[F키] 채집 오브젝트 상호작용</summary>

```
P_Finder.Update()
  → Physics.OverlapSphere(position, 5.0f, interactableLayer)
  → 탐색된 Collider 중 activationDistance(3.0f) 이하 & 가장 가까운 것 → closetObject

  → ShowIcon(closetObject)
        → Instantiate(IconPrefab, uiCanvas.transform)  ← 아이콘 생성 (최초 1회)
        → UpdateIconPosition(targetTransform, iconInstance)
              → Camera.main.WorldToScreenPoint(target.position + (0, 1.5f, 0))
              → Icon.RectTransform.position = screenPosition  ← 매 프레임 갱신 (Canvas rebatch 발생)

  [F키 입력 감지]
  → subObject.Interaction(GetComponent<Character>())
        ↓ Interaction_Hit.Interaction() 오버라이드
        → character.AnimationChange(m_Data.m_Type)  ← 오브젝트 타입 애니메이션
        → character.EquipmentChange(m_Data.m_Type, true)
        → base.Interaction(character)  [M_Object.Interaction()]
              → character.m_Object = this
              → GetInteraction = true

  → Delegate_Holder.OnStartInteraction()
        ├─► P_Movement
        │     → animator.SetBool("NoneInteraction", true)
        │     → animator.SetFloat("a_Speed", 0.0f)
        │     → Update(): F 외 키 입력 → Delegate_Holder.OnOutInteraction() (강제 탈출)
        ├─► Canvas_Holder.GetBoard()
        │     → Board.SetActive(true)
        └─► P_Finder.OnInteractionVoid()
              → OnInteraction = true
              → transform.LookAt(closetObject)
              → IconInit() ← 아이콘 Out 애니메이션
```

</details>

<details>
<summary>[애니메이션 이벤트 Hit()] 타격 처리</summary>

```
[Animator가 Hit() 이벤트 호출]

Character.Hit()
  → if(m_Object == null) return
  → m_Object.HP -= 20
  → GetHitParticle()
        → GetParticleTransform 기준 랜덤 오프셋 위치에 HitParticle Instantiate

  → m_Object.OnHit(this)
        ↓ Interaction_Hit.OnHit() 오버라이드

        → base.OnHit(character)  [M_Object.OnHit()]  ← 먼저 호출 (버그: 개선 필요 항목 #8 참고)
              → [MainPlayer] Canvas_Holder.GetBoard()
              → [MainPlayer] Base_Mng.Game.SetStamina(-10)
              → [MainPlayer] Cam_Movement.CameraShake()
              → HP_Init(character)
                    → [HP <= 0]
                          → HP = 0
                          → Particle_Handler.OnParticle(자식[0].MeshRenderer)
                          → [MainPlayer]
                                → Canvas_Holder.AllStopCoroutine()
                                → Canvas_Holder.BoardHpWhiteFill.fillAmount = 1.0f
                                → Delegate_Holder.OnOutInteraction()
                                      ├─► P_Movement: NoneInteraction = false
                                      ├─► Canvas_Holder.BoardOut(): Out 애니메이션
                                      └─► P_Finder.OnInteractionOut(): 탐지 재개
                          → [Worker] worker.StateChange(State.IDLE)
                          → Base_Mng.Object.RemoveObject(gameObject)
                          → Destroy(gameObject)
                    → [HP > 0, MainPlayer]
                          → Canvas_Holder.BoardFill(HP, m_Data.HP)
                                → BoardHpFill.fillAmount = HP / maxHP
                                → FillCoroutine(): BoardHpWhiteFill → BoardHpFill Lerp (잔상)

        → if(gameObject.activeInHierarchy)
              → ShakeTree(transform.position - player.position)
                    → targetRotation = Quaternion.Euler(original + shakeAmount)
                    → StopAllCoroutines()
                    → ShakeAnimation(targetRotation)
                          → 0.25초: original → target Slerp
                          → 0.25초: target → original Slerp

        → [HP <= 0]
              → ItemFlowController.DROPITEMLIST(m_Data.Drop_Items)
                    → 각 항목마다 Random.Range(0, 100) vs item.value 비교
                    → 통과 항목 반환
              → 반환 리스트 수만큼 Instantiate(item_Prefab)
                    ↓ Item.Start()
                    → SpreadAndMoveToPlayer()
                          → Random.insideUnitSphere * spreadRadius 방향으로 0.3초 분산
                          → MoveToPlayer(): 매 프레임 플레이어 위치 추적 Lerp
                          → 거리 < 0.5f 도달
                                → Navigation_Mng.PanelGet_Item() ← 아이템 획득 알림
                                → ItemFlowController.GETITEM() ← 인벤토리 반영
                                → Destroy(item)
```

</details>

<details>
<summary>[F키] 몬스터 전투</summary>

```
P_Finder.Update()
  → monsterObjects = Physics.OverlapSphere(radius=5.0f, monsterLayer)
  → GetMonster = monsterObjects.Length > 0
  → [GetMonster] transform.LookAt(monsterObjects[0])

  [F키 입력 & !isAttack]
  → AttackMonster(monsterObjects)
        → P_Movement.AnimationChange("Attack") ← Attack 트리거
        → P_Movement.colliders = monsterObjects ← 공격 대상 저장
        → Invoke("ReturnAttack", attack_speed)  ← isAttack 해제 예약

[Animator가 Attack() 이벤트 호출]
Character.Attack()
  → GetHitParticle()
  → for each collider in colliders:
        Monster.GetDamage(10)
              → [isDead] return
              → [Range 체크 통과]
                    → Canvas_Holder.GetText() ← 데미지 텍스트 (World Space TMP)
                    → HP -= 10
                    → Canvas_Holder.AddSlider(this)
                          → [신규] Instantiate(monster_Slider) + monsterSliders 등록
                          → [기존] GetSliderCheck() → SliderCoroutine 갱신
                    → Character.GetHitParticle()
                    → GetHitCoroutine() ← Emission 발광 이펙트 (0.2초 on/off)
                    → [HP <= 0]
                          → isDead = true
                          → StopAllCoroutines()
                          → Canvas_Holder.RemoveSlider(this) ← Animator "Out" 트리거
                          → gameObject.layer = "Default"
                          → AnimationChange("DIE")
                          → Destroy(gameObject, 1.5f)
```

</details>

<details>
<summary>Monster 자율 추적 / 공격</summary>

```
[Monster.FindPlayer() - 1초마다 코루틴 (재귀 방식 - 개선 필요 항목 #3 참고)]
  → distance = Distance(self, player)
  → [target == null && distance <= 5.0f]
        → target = player
        → AnimationChange("WALK")

[Monster.Update() - 매 프레임]
  → [isDead] return
  → distance = Distance(target, self)
  → [2.0 < distance <= 10.0]
        → AnimationChange("WALK")
        → agent.SetDestination(target.position)
  → [distance < 2.0 && !isAttack]
        → AttackPlayer()
              → AnimationChange("ATTACK")
              → Invoke("AttackReturn", 1.0f)  ← 하드코딩 타이머 (개선 필요 항목 #2 참고)
  → [distance > 10.0] target = null ← 추격 포기

[Animator가 Attack() 이벤트 호출]
Monster.Attack()
  → P_Movement.GetDamage(15)
        → Canvas_Holder.GetText() ← 데미지 텍스트
        → HP -= 15
        → Delegate_Holder.OnHPChange(HP) ← HP UI 갱신
```

</details>

---

## Unity 지식 메모

<details>
<summary>자식 Collider의 충돌 이벤트를 부모 스크립트에서 받는 방법</summary>

`OnTriggerStay`, `OnCollisionEnter` 등의 충돌 이벤트는 **Collider가 있는 오브젝트** 또는 **Rigidbody가 있는 오브젝트**에 전달된다.

**문제 상황:**
- 스크립트는 부모에 있고, Collider는 자식에 있을 때
- Rigidbody가 없으면 이벤트가 자식에만 전달됨 → 부모 스크립트는 못 받음

**해결:** 부모에 Rigidbody를 추가하면 자식 Collider의 충돌 이벤트가 부모로 올라옴.

**Is Kinematic 활성화 이유:** Rigidbody 추가 시 중력/물리 연산이 켜져 오브젝트가 떨어지므로, 물리 이동이 필요 없는 오브젝트는 `Is Kinematic = true`로 설정.

```
이벤트 전달 기준:
- Collider만 있는 경우 → Collider가 있는 오브젝트에 전달
- 부모에 Rigidbody 있는 경우 → Rigidbody가 있는 부모에 전달 (자식 Collider 포함)
```

</details>

---

## 개선 필요 항목

<details>
<summary>1. Canvas_Holder.CheckSlider / P_Finder.UpdateIconPosition — Canvas 매 프레임 rebatch</summary>

**현재 방식:**
`Canvas_Holder.CheckSlider()`와 `P_Finder.UpdateIconPosition()`이 매 프레임 `WorldToScreenPoint`로 UI 요소의 `transform.position`을 변경.
Canvas 내부 요소가 움직이면 Canvas가 dirty 마킹되어 매 프레임 배치를 재계산함. 몬스터/오브젝트가 많아질수록 병목이 심해짐.

```csharp
// 현재 (문제) - 매 프레임 Canvas dirty → rebatch
private void CheckSlider()
{
    foreach(var slider in monsterSliders)
    {
        slider.Value.transform.position = Camera.main.WorldToScreenPoint(pos); // Canvas 내부 요소 이동
    }
}
```

**개선 방향:**
몬스터/오브젝트에 World Space Canvas를 자식으로 직접 부착. Canvas 자체의 transform이 따라가므로 Canvas 내부 UI 요소는 정적 배치 유지. `CheckSlider()`, `UpdateIconPosition()` 제거 가능.

```csharp
// 개선 - Instantiate 시 monster의 자식으로 부착
var go = Instantiate(monster_Slider, monster.transform);
go.transform.localPosition = new Vector3(0, 2.0f, 0); // 머리 위 고정
// CheckSlider() 전체 삭제
```

Canvas Render Mode를 `World Space`로 변경하고, 항상 카메라를 향하도록 Billboard 처리만 추가하면 됨.

</details>

<details>
<summary>2. Monster.cs — AttackReturn Invoke 하드코딩</summary>

`Invoke("AttackReturn", 1.0f)`로 공격 판정 종료 타이밍을 고정. 애니메이션 속도(Animator Speed)가 바뀌면 공격 애니메이션이 끝나도 `isAttack`이 여전히 true 상태거나, 반대로 판정이 너무 일찍 풀림.

```csharp
// 현재 (문제)
Invoke("AttackReturn", 1.0f); // 애니메이션 길이와 무관

// 개선 - 공격 애니메이션 마지막 프레임에 Animation Event로 AttackReturn() 호출
// Invoke 제거, Animation 창에서 이벤트 등록
```

</details>

<details>
<summary>3. Monster.cs — FindPlayer 재귀 코루틴</summary>

코루틴 끝에서 `StartCoroutine(FindPlayer())`를 호출하는 재귀 패턴. 매 1초마다 새 코루틴 객체를 생성하고 이전 코루틴은 종료됨. 실질적인 메모리 문제보다는 의도가 불명확하고 관리가 어려운 구조.

```csharp
// 현재 (문제)
IEnumerator FindPlayer()
{
    // ...
    yield return new WaitForSeconds(1.0f);
    StartCoroutine(FindPlayer()); // 재귀
}

// 개선
IEnumerator FindPlayer()
{
    while(true)
    {
        // ...
        yield return new WaitForSeconds(1.0f);
    }
}
```

</details>

<details>
<summary>4. Canvas_Holder.RemoveSlider — ContainsKey 없는 딕셔너리 접근</summary>

`monsterSliders[monster]` 직접 접근 시 키가 없으면 `KeyNotFoundException`. 이미 사망 처리된 몬스터에 중복 호출되거나 타이밍 이슈 발생 시 크래시.

```csharp
// 현재 (문제)
public void RemoveSlider(Monster monster)
{
    monsterSliders[monster].GetComponent<Animator>().SetTrigger("Out"); // 키 없으면 크래시
    monsterSliders.Remove(monster);
}

// 개선
public void RemoveSlider(Monster monster)
{
    if(!monsterSliders.ContainsKey(monster)) return;
    monsterSliders[monster].GetComponent<Animator>().SetTrigger("Out");
    monsterSliders.Remove(monster);
}
```

</details>

<details>
<summary>5. P_Movement.GetDamage — 사망 처리 없음</summary>

`HP -= dmg`만 있고 HP <= 0에 대한 처리가 없음. 플레이어가 죽어도 HP가 음수로 방치되고 아무 반응이 없음.

```csharp
// 현재 (문제)
public void GetDamage(int dmg)
{
    HP -= dmg;
    Delegate_Holder.OnHPChange(HP); // HP 음수여도 그냥 표시
}

// 개선
public void GetDamage(int dmg)
{
    HP = Mathf.Max(0, HP - dmg);
    Delegate_Holder.OnHPChange(HP);
    if(HP <= 0) { /* 사망 처리 */ }
}
```

</details>

<details>
<summary>6. Object_Mng.cs — Random.Range 오프-바이-원</summary>

`Random.Range(int, int)`는 상한값이 exclusive. `m_Datas.Length - 1`을 쓰면 마지막 오브젝트 타입은 절대 생성되지 않음.

```csharp
// 현재 (문제)
var GetObject = m_Datas[Random.Range(0, m_Datas.Length - 1)]; // 마지막 인덱스 미포함

// 개선
var GetObject = m_Datas[Random.Range(0, m_Datas.Length)];
```

</details>

<details>
<summary>7. Character.Attack — 루프 내 GetComponent 호출</summary>

공격 시 `colliders` 배열을 순회하며 매번 `GetComponent<Monster>()`를 호출. 몬스터가 아닌 콜라이더가 들어있으면 null 반환 후 NullReferenceException 크래시.

```csharp
// 현재 (문제)
for (int i = 0; i < colliders.Length; i++)
{
    colliders[i].GetComponent<Monster>().GetDamage(10); // null 가능성
}

// 개선
for (int i = 0; i < colliders.Length; i++)
{
    if(colliders[i].TryGetComponent(out Monster monster))
        monster.GetDamage(10);
}
```

</details>

<details>
<summary>8. Interaction_Hit.cs — base.OnHit() 호출 순서</summary>

`base.OnHit()` → `HP_Init()` → `Destroy(gameObject)` 예약이 먼저 실행된 뒤 `ShakeTree`가 호출됨.
`Destroy`는 프레임 말에 실행되므로 코루틴 자체는 시작되지만, 오브젝트 파괴 시점에 강제 중단됨.

```csharp
// 현재 (문제)
public override void OnHit(Character character)
{
    base.OnHit(character); // Destroy 예약
    if (gameObject.activeInHierarchy)
        ShakeTree(...);    // 코루틴 시작되나 도중에 끊김
    if(HP <= 0) { 드롭 처리 }
}

// 개선
public override void OnHit(Character character)
{
    if(HP > 0) ShakeTree(...);
    if(HP <= 0) { 드롭 처리 }
    base.OnHit(character); // 마지막에 Destroy 예약
}
```

</details>

<details>
<summary>9. Canvas_Holder.cs — FillCoroutine 역방향 조건 미처리</summary>

나무를 70% 깎다가 상호작용 해제 후 풀피 오브젝트를 새로 상호작용하면
`BoardHpFill`은 1.0으로 올라가지만 `BoardHpWhiteFill`은 0.3에 고정됨.
`while(white - fill > 0.001f)` 조건이 `-0.7 > 0.001f` → false라 코루틴이 즉시 탈출하고 잔상이 남음.

```csharp
// 개선
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
<summary>10. Delegate_Holder.cs — static 이벤트 구독 해제 없음</summary>

`P_Movement`, `Canvas_Holder`, `P_Finder`가 `Start()`에서 `+=` 구독하지만 `OnDestroy()`에서 `-=` 해제 없음.
씬 재로드 또는 오브젝트 재생성 시 중복 구독으로 같은 동작이 두 번씩 실행됨.
(`M_Object`는 `OnDestroy()`에서 해제하고 있어 올바른 예시.)

```csharp
// 개선 - P_Movement, Canvas_Holder, P_Finder 각각에 추가
private void OnDestroy()
{
    Delegate_Holder.OnInteraction -= (구독한 메서드);
    Delegate_Holder.OnInteractionOut -= (구독한 메서드);
}
```

</details>

---
