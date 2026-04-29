## 에셋

| 종류 | 링크 |
|------|------|
| Terrain | https://assetstore.unity.com/packages/p/idyllic-fantasy-nature-260042 |
| Particle | https://assetstore.unity.com/packages/p/cartoon-fx-remaster-free-109565 |

---

## 코드 구조

| 파일 | 타입 | 역할 |
|------|------|------|
| **Main/** | | |
| `Character.cs` | MonoBehaviour ★ | 플레이어/워커 공통 베이스 — HP, 장비, 파티클, Hit/Attack |
| `P_Movement.cs` | Character 상속 · 싱글턴 | 플레이어 이동/회전/데미지 |
| `Worker.cs` | Character 상속 | NavMesh 기반 자율 채집 유닛, 나침반 마커 |
| `M_Object.cs` | MonoBehaviour ★ | 채집 오브젝트 베이스 — HP, 파괴, HP바 연동 |
| `Interaction_Hit.cs` | M_Object 상속 | 타격 흔들림, 드롭 생성 |
| `BonFire.cs` | M_Object 상속 | 상호작용 시 스태미나 회복 |
| `Portal.cs` | M_Object 상속 | Worker 소환 UI 연동 |
| `Monster.cs` | MonoBehaviour | NavMesh 추적 + 공격 + HP바 |
| `Item.cs` | MonoBehaviour | 드롭 아이템 분산 → 흡착 → 자기파괴 |
| `P_Finder.cs` | MonoBehaviour | 범위 탐지, F키 상호작용/공격 발동 |
| `Cam_Movement.cs` | MonoBehaviour · 싱글턴 | 카메라 Lerp 추적 + 쉐이크 |
| **Manager/** | | |
| `Base_Mng.cs` | MonoBehaviour · 싱글턴 | Build / Game / Object 서브매니저 접근점 |
| `Game_Mng.cs` | MonoBehaviour | 스태미나 관리 |
| `Building_Mng.cs` | MonoBehaviour | 건물 배치 (Raycast 이동, 회전, 확정) |
| `Object_Mng.cs` | MonoBehaviour | 오브젝트 스폰 + CullingGroup 가시성 관리 |
| `Delegate_Holder.cs` | MonoBehaviour | static 이벤트 버스 |
| `Asset_Mng.cs` | 순수 C# | SpriteAtlas / Building ScriptableObject 정적 로더 |
| `ItemFlowController.cs` | 순수 C# | 확률 기반 드롭 + 인벤토리 Dictionary 관리 |
| `Utils.cs` | static 클래스 | Localization, 타이머, 레이어 변경 유틸 |
| `Enum_Holder.cs` | — | enum 정의 |
| **Building/** | | |
| `Building_OBJ.cs` | MonoBehaviour | 건물 상태 관리 (투명→불투명, 진행바, 완료 이펙트) |
| **Scriptable/** | | |
| `Scriptable_Base.cs` | ScriptableObject | Key, Icon 공통 베이스 |
| `Object_Scriptable.cs` | ScriptableObject | 채집 오브젝트 데이터 (HP, 드롭 테이블) |
| `Item_Scriptable.cs` | ScriptableObject | 아이템 데이터 (Key, 타입, 희귀도, Weight) |
| `Building_Scriptable.cs` | ScriptableObject | 건물 데이터 (prefab, Key, 건설 시간, 재료) |
| `Unit_Scriptable.cs` | ScriptableObject | 유닛 데이터 |
| **UI/** | | |
| `Canvas_Holder.cs` | MonoBehaviour · 싱글턴 | HP바, 몬스터 슬라이더, UI 패널 토글, 데미지 텍스트 |
| `Directional_Monster_Slider.cs` | MonoBehaviour | 몬스터 HP바 개별 인스턴스 |
| `CompassBar.cs` | MonoBehaviour | 방위 나침반 + 건물/Worker 마커 |
| `Navigation_Mng.cs` | MonoBehaviour · 싱글턴 | 화면 하단 알림 (아이템 획득, 건설 알림) |
| `UIPART.cs` | MonoBehaviour ★ | UI 패널 베이스 — Open / Close / Toggle |
| `INVENTORY.cs` `BUILDING.cs` `PORTAL.cs` | UIPART 상속 | 인벤토리 / 건설 / 포탈 패널 |
| `Particle_Handler.cs` | MonoBehaviour · 싱글턴 | 오브젝트 파괴 파티클 재생 |

<details>
<summary>클래스 스켈레톤 구조</summary>

```
MonoBehaviour
│
├── [Main]
│   ├── Character ★
│   │   ├── P_Movement          싱글턴 · 플레이어 이동/데미지
│   │   └── Worker              NavMesh 채집 유닛
│   ├── M_Object ★
│   │   ├── Interaction_Hit     채집 오브젝트 (흔들림/드롭)
│   │   ├── BonFire             모닥불 (스태미나 회복)
│   │   └── Portal              Worker 소환 포탈
│   ├── Monster                 NavMesh 추적/전투
│   ├── Monster_Spawner         몬스터 스폰
│   ├── Item                    드롭 아이템 분산→흡착
│   ├── P_Finder                범위 탐지 / F키 상호작용
│   └── Cam_Movement            싱글턴 · Lerp 추적 + 쉐이크
│
├── [Manager]
│   ├── Base_Mng                싱글턴
│   │   ├── (자식) Game_Mng     스태미나
│   │   ├── (자식) Building_Mng 건물 배치
│   │   └── (자식) Object_Mng   스폰 + CullingGroup
│   └── Delegate_Holder         static 이벤트 버스
│
├── [Building]
│   └── Building_OBJ            건물 상태 (투명→불투명, 진행바)
│
└── [UI]
    ├── Canvas_Holder           싱글턴 · HP바/슬라이더/패널/데미지텍스트
    ├── Directional_Monster_Slider  몬스터 HP바 개별 인스턴스
    ├── UIPART ★
    │   ├── INVENTORY
    │   ├── BUILDING
    │   └── PORTAL
    ├── CompassBar              방위 나침반 + 마커
    ├── Navigation_Mng          화면 하단 알림
    ├── UI_Animation_Handler    애니메이션 상태 전환
    ├── Particle_Handler        싱글턴 · 파괴 파티클
    ├── PopUP_Description       오브젝트 설명 팝업
    ├── Building_Panel          건설 패널 아이템
    ├── Unit_Panel              유닛 패널 아이템
    ├── Item_Panel              인벤토리 아이템
    └── Nav_Item                알림 아이템

ScriptableObject
├── Scriptable_Base ★
│   ├── Item_Scriptable
│   ├── Building_Scriptable
│   └── Unit_Scriptable
└── Object_Scriptable           (Scriptable_Base 미상속)

static / 순수 C#
├── Asset_Mng                   SpriteAtlas / Building 정적 로더
├── ItemFlowController          확률 드롭 + 인벤토리 Dictionary
├── Utils                       유틸 (Localization, 타이머, 레이어)
└── Enum_Holder                 enum 정의

★ = 베이스 클래스
```

</details>

---

## 핵심 이벤트 흐름

```
[F키 — 채집 오브젝트]
P_Finder ──► M_Object.Interaction(character)
              └─ character.m_Object = this
              Delegate_Holder.OnStartInteraction()
              ├── P_Movement    : 이동 잠금
              ├── Canvas_Holder : HP바 HUD 표시
              └── P_Finder      : 아이콘 숨김

[애니메이션 이벤트 Hit()]
Character.Hit()
  ├─ m_Object.HP -= 20
  └─ M_Object.OnHit(character)
       ├─ [MainPlayer] SetStamina(-10), CameraShake
       └─ HP_Init(character)
            ├─ [HP > 0]  Canvas_Holder.BoardFill()
            └─ [HP <= 0] Particle_Handler → Destroy → OnOutInteraction
                          ├── P_Movement    : 이동 잠금 해제
                          ├── Canvas_Holder : HUD Out 애니메이션
                          └── P_Finder      : 탐지 재개
  └─ Interaction_Hit: ShakeTree / 드롭 생성

[F키 — 몬스터]
P_Finder ──► AttackMonster(colliders)
              └─ AnimationChange("Attack")
              [애니메이션 이벤트 Attack()]
              Character.Attack()
              └─ Monster.GetDamage(10)
                   ├─ Canvas_Holder.GetText()  데미지 텍스트
                   ├─ Canvas_Holder.AddSlider() HP바 갱신
                   └─ [HP <= 0] RemoveSlider → DIE → Destroy(1.5f)

[Monster 자율 추적]
FindPlayer() 1초마다 ──► target 지정
Update() 매 프레임
  ├─ [2.0 < 거리 ≤ 10.0] NavMesh 추격
  ├─ [거리 < 2.0]        AttackPlayer()
  │                        [애니메이션 이벤트 Attack()]
  │                        Monster.Attack() → P_Movement.GetDamage(15)
  └─ [거리 > 10.0]       target = null
```

---

## 버그

```
Object_Mng.cs — Random.Range 오프-바이-원 (마지막 오브젝트 타입 미생성)

  현재: m_Datas[Random.Range(0, m_Datas.Length - 1)]
  수정: m_Datas[Random.Range(0, m_Datas.Length)]
```

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
[Monster.FindPlayer() - 1초마다 코루틴]
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
              → Invoke("AttackReturn", 1.0f)
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
- Collider만 있는 경우    → Collider가 있는 오브젝트에 전달
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
        slider.Value.transform.position = Camera.main.WorldToScreenPoint(pos);
    }
}
```

**개선 방향:**
몬스터/오브젝트에 World Space Canvas를 자식으로 직접 부착. Canvas 자체의 transform이 따라가므로 Canvas 내부 UI 요소는 정적 배치 유지. `CheckSlider()`, `UpdateIconPosition()` 제거 가능.

```csharp
// 개선 - Instantiate 시 monster의 자식으로 부착
var go = Instantiate(monster_Slider, monster.transform);
go.transform.localPosition = new Vector3(0, 2.0f, 0);
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
```

</details>

<details>
<summary>3. Monster.cs — FindPlayer 재귀 코루틴</summary>

코루틴 끝에서 `StartCoroutine(FindPlayer())`를 호출하는 재귀 패턴. 매 1초마다 새 코루틴 객체를 생성함.

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

`monsterSliders[monster]` 직접 접근 시 키가 없으면 `KeyNotFoundException`.

```csharp
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

`HP -= dmg`만 있고 HP <= 0에 대한 처리가 없음.

```csharp
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
var GetObject = m_Datas[Random.Range(0, m_Datas.Length - 1)];

// 개선
var GetObject = m_Datas[Random.Range(0, m_Datas.Length)];
```

</details>

<details>
<summary>7. Character.Attack — 루프 내 GetComponent + null 체크 없음</summary>

몬스터가 아닌 콜라이더가 들어있으면 null 반환 후 NullReferenceException 크래시.

```csharp
// 현재 (문제)
colliders[i].GetComponent<Monster>().GetDamage(10);

// 개선
if(colliders[i].TryGetComponent(out Monster monster))
    monster.GetDamage(10);
```

</details>

<details>
<summary>8. Interaction_Hit.cs — base.OnHit() 호출 순서</summary>

`base.OnHit()` → `Destroy(gameObject)` 예약 후 `ShakeTree` 코루틴이 시작되지만 오브젝트 파괴 시점에 강제 중단됨.

```csharp
// 현재 (문제)
public override void OnHit(Character character)
{
    base.OnHit(character); // Destroy 예약
    if (gameObject.activeInHierarchy)
        ShakeTree(...);    // 도중에 끊김
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

나무를 70% 깎다가 상호작용 해제 후 풀피 오브젝트를 새로 상호작용하면 `BoardHpWhiteFill`이 0.3에 고정되어 잔상이 남음.

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

`P_Movement`, `P_Finder`가 `Start()`에서 `+=` 구독하지만 `OnDestroy()`에서 `-=` 해제 없음. 씬 재로드 시 중복 구독 발생.

```csharp
// 개선
private void OnDestroy()
{
    Delegate_Holder.OnInteraction -= (구독한 메서드);
    Delegate_Holder.OnInteractionOut -= (구독한 메서드);
}
```

</details>

---
