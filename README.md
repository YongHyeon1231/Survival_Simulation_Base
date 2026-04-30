# Survival Simulation

## 👋 프로젝트 소개

- **게임명:** Survival Simulation
- **개발 기간:** 2026.04 ~
- **게임 장르:** 서바이벌 / 시뮬레이션
- **프로젝트 소개:** 자원을 채집하고 건물을 건설하며 생존하는 Unity 기반 싱글플레이 서바이벌 시뮬레이션입니다. NavMesh 기반 AI 유닛, 확률 드롭 인벤토리, 실시간 날씨/주야 시스템을 포함합니다.
- **프로젝트 목표:** 서바이벌 장르에 RTS 요소가 결합된 사례가 부족하다는 점에 착안하여, 자원 수급과 생존 중심 구조에 기지 건설 및 유닛 생산 시스템을 결합한 게임을 기획·개발하였습니다. 특히 스타크래프트의 프로토스 생산 방식을 참고해 게이트 기반 유닛 소환 구조를 구현했으며, 핵심 게임 루프 중심의 프로토타입을 제작했습니다.

---

## 🛠️ 기술 스택

<b>Language</b><br>
<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#"/>
<br>
<b>Engine</b><br>
<img src="https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white" alt="Unity"/>
<br>
<b>AI / Navigation</b><br>
<img src="https://img.shields.io/badge/NavMesh-FF6C00?style=for-the-badge&logo=unity&logoColor=white" alt="NavMesh"/>
<br>
<b>VCS</b><br>
<img src="https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white" alt="GitHub"/>

---

## 📌 주요 기능

### 플레이어 / 전투

- 이동, 회전, 상호작용 입력 처리 (`P_Movement`, `P_Finder`)
- 애니메이션 이벤트 기반 타격 판정 (`Character.Hit`, `Character.Attack`)
- 카메라 Lerp 추적 + 피격 셰이크 (`Cam_Movement`)

### 자원 채집 시스템

- 오브젝트 HP·드롭 테이블을 ScriptableObject로 데이터화 (`Object_Scriptable`)
- 타격 시 나무 흔들림 연출 + 확률 드롭 (`Interaction_Hit`, `ItemFlowController`)
- 드롭 아이템 분산→플레이어 흡착→자동 파괴 (`Item`)

### Worker AI 유닛

- NavMesh 기반 자율 채집 유닛 (`Worker`)
- 나침반 마커 연동으로 위치 시각화 (`CompassBar`)

### 건물 건설 시스템

- Raycast 기반 배치 이동, 회전, 확정 (`Building_Mng`)
- 건설 중 투명→불투명 전환, 진행바, 완료 이펙트 (`Building_OBJ`)
- 재료 소모 및 건물 데이터 ScriptableObject화 (`Building_Scriptable`)

### 날씨 / 주야 시스템

- 태양 회전·색상 변화 기반 24시간 주야 사이클 (`Wheather_Mng`)
- 비(Rain ParticleSystem) 강도 및 바람(Shader Wind) 세기 실시간 제어
- Delegate 이벤트 버스로 날씨 상태 전파 (`Delegate_Holder`)

### 몬스터 AI

- 1초 주기 플레이어 탐지 → NavMesh 추격 → 근접 공격 (`Monster`)
- 피격 Emission 이펙트, HP바 자동 생성/제거 (`Canvas_Holder`)

### UI / HUD

- HP바, 몬스터 슬라이더, 데미지 텍스트, 나침반 마커 (`Canvas_Holder`, `CompassBar`)
- 인벤토리, 건설, 포탈 패널 (UIPART 상속 구조)
- 화면 하단 아이템 획득·건설 완료 알림 (`Navigation_Mng`)

### 성능 최적화

- CullingGroup 기반 오브젝트 가시성 관리 (`Object_Mng`)
- static 이벤트 버스로 컴포넌트 간 의존성 분리 (`Delegate_Holder`)

---

## ⚙️ 아키텍처

```
[ScriptableObject Layer]
  Item_Scriptable / Building_Scriptable / Object_Scriptable / Unit_Scriptable

[Manager Layer]
  Base_Mng (싱글턴 진입점)
  ├── Game_Mng        — 스태미나
  ├── Building_Mng    — 건물 배치
  └── Object_Mng      — 스폰 + CullingGroup

  Asset_Mng           — SpriteAtlas / Building 정적 로더
  ItemFlowController  — 확률 드롭 + 인벤토리 Dictionary
  Delegate_Holder     — static 이벤트 버스
  Wheather_Mng        — 날씨 / 주야 시스템

[Gameplay Layer]
  Character (베이스)
  ├── P_Movement      — 플레이어
  └── Worker          — NavMesh 채집 유닛

  M_Object (베이스)
  ├── Interaction_Hit — 채집 오브젝트
  ├── BonFire         — 모닥불
  └── Portal          — Worker 소환

  Monster / Monster_Spawner
  Item / P_Finder / Cam_Movement

[UI Layer]
  Canvas_Holder (싱글턴)
  UIPART (베이스) → INVENTORY / BUILDING / PORTAL
  CompassBar / Navigation_Mng / Particle_Handler
```

---

## 📁 디렉토리 구조

<details>
<summary>📂 Assets/00_Scripts</summary>

```
00_Scripts/
├── Main/
│   ├── Character.cs            — 플레이어/워커 공통 베이스
│   ├── P_Movement.cs           — 플레이어 이동/데미지
│   ├── P_Finder.cs             — 범위 탐지, 상호작용/공격
│   ├── Worker.cs               — NavMesh 자율 채집 유닛
│   ├── M_Object.cs             — 채집 오브젝트 베이스
│   ├── Interaction_Hit.cs      — 타격 흔들림, 드롭 생성
│   ├── Monster.cs              — NavMesh 추적 + 공격
│   ├── Monster_Spawner.cs
│   ├── Item.cs                 — 드롭 아이템 분산→흡착
│   └── Cam_Movement.cs         — Lerp 추적 + 카메라 셰이크
│
├── Building/
│   ├── Building_OBJ.cs         — 건물 상태 관리
│   ├── BonFire.cs
│   └── Portal.cs
│
├── Manager/
│   ├── Base_Mng.cs
│   ├── Game_Mng.cs
│   ├── Building_Mng.cs
│   ├── Object_Mng.cs
│   ├── Wheather_Mng.cs         — 날씨/주야 시스템
│   ├── Delegate_Holder.cs      — static 이벤트 버스
│   ├── Asset_Mng.cs
│   ├── ItemFlowController.cs
│   ├── Utils.cs
│   └── Enum_Holder.cs
│
├── Scriptable/
│   ├── Scriptable_Base.cs
│   ├── Item_Scriptable.cs
│   ├── Building_Scriptable.cs
│   ├── Object_Scriptable.cs
│   └── Unit_Scriptable.cs
│
└── UI/
    ├── Canvas_Holder.cs
    ├── Directional_Monster_Slider.cs
    ├── CompassBar.cs
    ├── Navigation_Mng.cs
    ├── Particle_Handler.cs
    ├── UI_Animation_Handler.cs
    ├── PopUP_Description.cs
    └── PART/
        ├── UIPART.cs           — UI 패널 베이스
        ├── INVENTORY.cs
        ├── BUILDING.cs
        └── PORTAL.cs
```

</details>

---

## 🔗 핵심 이벤트 흐름

<details>
<summary>채집 오브젝트 상호작용 (F키)</summary>

```
P_Finder → M_Object.Interaction()
  └─ Delegate_Holder.OnStartInteraction()
       ├── P_Movement    : 이동 잠금
       ├── Canvas_Holder : HP바 HUD 표시
       └── P_Finder      : 아이콘 숨김

[애니메이션 이벤트 Hit()]
Character.Hit() → M_Object.HP -= 20 → M_Object.OnHit()
  ├── [HP > 0]  Canvas_Holder.BoardFill()
  └── [HP ≤ 0]  Particle_Handler → Destroy
                  └─ Delegate_Holder.OnOutInteraction()
                       ├── P_Movement    : 이동 잠금 해제
                       ├── Canvas_Holder : HUD Out 애니메이션
                       └── P_Finder      : 탐지 재개
```

</details>

<details>
<summary>몬스터 전투 (F키)</summary>

```
P_Finder → AttackMonster()
  └─ AnimationChange("Attack")

[애니메이션 이벤트 Attack()]
Character.Attack() → Monster.GetDamage(10)
  ├── Canvas_Holder.GetText()   — 데미지 텍스트
  ├── Canvas_Holder.AddSlider() — HP바 갱신
  └── [HP ≤ 0]
        → Canvas_Holder.RemoveSlider()
        → AnimationChange("DIE") → Destroy(1.5f)
```

</details>

<details>
<summary>날씨 이벤트 흐름</summary>

```
Wheather_Mng
  ├── UpdateTime()     — currentTime += deltaTime * TimeSpeed
  ├── RotateSun()      — DirectionalLight 회전 (0~360도)
  └── UpdateSunColor() — Gradient 기반 색상 보간

  Delegate_Holder.OnRainIntensityChange(intensity)
    └── Wheather_Mng.UpdateRainEmission()
          → emissionModule.rateOverTime = Lerp(min, max, intensity)

  Delegate_Holder.OnWindStrengthChange(strength)
    └── Wheather_Mng.UpdateWindStrength()
          → windMaterial.SetFloat("_Wind_Strength", Lerp(min, max, strength))
```

</details>

---

## 👩‍💻 개발자

| 이름   | 블로그 | GitHub |
|--------|--------|--------|
| 박용현 | [dydgustmdfl1231 Blog](https://dydgustmdfl1231.tistory.com/) | [YongHyeon1231](https://github.com/YongHyeon1231/) |

---

## 📦 사용 에셋

| 종류 | 링크 |
|------|------|
| Terrain | [Idyllic Fantasy Nature](https://assetstore.unity.com/packages/p/idyllic-fantasy-nature-260042) |
| Particle | [Cartoon FX Remaster Free](https://assetstore.unity.com/packages/p/cartoon-fx-remaster-free-109565) |

---

## 🔧 아쉬운 점 및 개선 방향 - 수정 예정

<details>
<summary>1. 중앙 관리 시스템을 초반부터 설계하지 못한 점</summary>

**현재 문제**
`Base_Mng`, `Canvas_Holder`, `Delegate_Holder` 등 매니저 구조를 개발 중후반에 도입하면서 리팩토링이 미완성된 상태로 남았다. 일부 컴포넌트는 여전히 `FindObjectOfType` 또는 직접 참조로 다른 시스템에 접근하고 있어 의존성이 혼재한다.

**개선 방안**
- 초기 설계 단계에서 GameManager → SubManager 계층 구조와 Delegate 이벤트 버스를 확정하고 시작
- 모든 컴포넌트 간 통신은 `Delegate_Holder`의 이벤트를 통해서만 진행하도록 규칙화
- `FindObjectOfType` 사용을 전면 금지하고 싱글턴 또는 DI(의존성 주입)로 대체

</details>

<details>
<summary>2. 맵 경계 방벽 미설치로 인한 플레이 불편</summary>

**현재 문제**
맵 끝에 물리적 장벽이 없어 플레이어와 Worker, 몬스터가 맵 밖으로 이탈할 수 있다. NavMesh 범위 밖에서 에이전트가 멈추거나 예외 동작을 일으키는 원인이 되기도 한다.

**개선 방안**
- 맵 외곽에 Invisible Wall(투명 Collider) 배치 또는 Terrain 높이 기반 경계 설정
- NavMesh 베이크 범위를 맵 내부로 한정해 에이전트의 이탈 자체를 원천 차단
- 카메라에도 경계 클램프를 적용해 시야가 맵 밖으로 나가지 않도록 처리

</details>

<details>
<summary>3. Occlusion Culling으로 인한 오브젝트 비활성화 버그</summary>

**현재 문제**
`Object_Mng`의 CullingGroup이 플레이어 기준으로 가시성을 판단하기 때문에, AI(Worker, Monster)가 채집 오브젝트를 공격하는 도중 플레이어가 멀어지면 해당 오브젝트가 비활성화(`SetActive(false)`)되어 AI가 공격 중인 대상이 사라지는 버그가 발생한다.

**개선 방안**
- CullingGroup 비활성화 기준에 "현재 AI가 상호작용 중인 오브젝트" 예외 처리 추가
- `M_Object`에 `isOccupied` 플래그를 두고, 활성 상태인 오브젝트는 컬링 대상에서 제외
- 또는 오브젝트를 완전히 파괴하는 대신 렌더러만 끄고 Collider는 유지하는 방식으로 전환

</details>

<details>
<summary>4. 인벤토리 아이템 재배치 불가</summary>

**현재 문제**
인벤토리는 획득 순서대로 고정 슬롯에 표시되며, 아이템 간 위치 교체(드래그 앤 드롭)나 슬롯 선택 기능이 없다. 실질적인 인벤토리 조작이 불가능해 플레이어 경험이 크게 떨어진다.

**개선 방안**
- `IBeginDragHandler`, `IDragHandler`, `IEndDropHandler` 인터페이스를 활용한 Unity UI 드래그 앤 드롭 구현
- 슬롯 인덱스 기반으로 아이템 Dictionary를 재정렬하는 `SwapItem(int from, int to)` 메서드 추가
- 장기적으로 핫바(단축 슬롯) 분리 및 아이템 사용/버리기 기능으로 확장

</details>

<details>
<summary>5. SOLID 원칙 미준수로 인한 스크립트 복잡도 문제</summary>

**현재 문제**
- **SRP 위반:** `Canvas_Holder`가 HP바, 몬스터 슬라이더, 데미지 텍스트, 패널 토글을 모두 담당. 하나의 클래스가 너무 많은 역할을 맡아 수정 시 영향 범위가 넓다.
- **OCP 위반:** 새 건물/오브젝트 타입 추가 시 `switch-case` 또는 `if-else` 분기를 직접 수정해야 하는 구조.
- **DIP 위반:** 일부 컴포넌트가 구체 클래스(`P_Movement`, `Canvas_Holder`)를 직접 참조해 테스트 및 교체가 어렵다.

**개선 방안**
- `Canvas_Holder`를 `HpBarController`, `DamageTextController`, `PanelController` 등으로 역할별 분리
- 오브젝트/건물 동작은 `ScriptableObject` + 전략 패턴(Strategy Pattern)으로 확장 가능하게 설계
- 핵심 시스템은 인터페이스(`IDamageable`, `IInteractable`)를 정의하고 구체 클래스 대신 인터페이스에 의존

</details>

<details>
<summary>6. 오브젝트 풀링 미적용 (잦은 Instantiate/Destroy)</summary>

**현재 문제**
드롭 아이템, 파티클, 몬스터 HP 슬라이더, 알림 패널 등이 매번 `Instantiate`/`Destroy`로 처리된다. 오브젝트가 많아질수록 GC 호출이 빈번해져 프레임 드롭의 원인이 된다.

**개선 방안**
- Unity `ObjectPool<T>` (2021 LTS 이상) 또는 커스텀 오브젝트 풀 구현
- `Item`, `Nav_Item`, `Directional_Monster_Slider` 처럼 반복 생성/소멸되는 오브젝트를 우선 대상으로 풀링 적용

</details>

<details>
<summary>7. 플레이어 사망 처리 미구현</summary>

**현재 문제**
`P_Movement.GetDamage()`는 HP를 차감만 하고 HP ≤ 0에 대한 처리가 없다. 몬스터에게 계속 맞아도 게임이 끝나지 않으며, 게임 오버/리스폰 로직 자체가 부재하다.

**개선 방안**
- `GetDamage()`에 `HP <= 0` 분기 추가 후 `Delegate_Holder`를 통해 GameOver 이벤트 발행
- `Game_Mng`에서 구독하여 씬 재로드 또는 리스폰 처리
- UI에 게임 오버 패널 연동

</details>

<details>
<summary>8. AI 상태 관리 비구조화 (FSM 미도입)</summary>

**현재 문제**
`Monster`와 `Worker`의 상태 전환(IDLE → WALK → ATTACK 등)이 `Update()` 내 `if-else` 분기로 처리된다. 상태가 추가될수록 분기가 중첩되어 가독성과 유지보수성이 급격히 낮아진다.

**개선 방안**
- State 패턴 기반 FSM(Finite State Machine) 도입: `IState` 인터페이스 + 상태별 클래스 분리
- Unity `Animator`의 StateMachineBehaviour를 활용해 애니메이션 전환과 게임 로직 상태를 일치시키는 방법도 고려

</details>

<details>
<summary>9. 하드코딩된 매직 넘버 다수</summary>

**현재 문제**
`Monster`의 탐지 거리(`5.0f`), 공격 범위(`2.0f`), 추격 포기 거리(`10.0f`), 공격 쿨타임(`1.0f`), 아이템 흡착 거리(`0.5f`) 등이 코드 내에 리터럴로 산재해 있다. 밸런스 조정 시 소스 코드를 직접 수정해야 한다.

**개선 방안**
- 수치 데이터를 `ScriptableObject`로 분리하거나 `SerializeField`로 Inspector 노출
- 최소한 클래스 상단에 `const`/`readonly` 상수로 명명하여 의미를 명확히 하고 한 곳에서 관리

</details>

<details>
<summary>10. 저장/불러오기 시스템 부재</summary>

**현재 문제**
게임을 종료하면 인벤토리, 건물 배치, 플레이어 HP 등 모든 진행 상황이 초기화된다. 서바이벌 장르의 핵심 플레이 루프인 "진행 축적"이 동작하지 않는다.

**개선 방안**
- `PlayerPrefs`(간단) 또는 JSON 직렬화 + `Application.persistentDataPath` 저장(확장성)
- 저장 대상: 인벤토리 Dictionary, 건물 배치 리스트, 플레이어 스탯
- `SaveManager` 단일 클래스에서 저장/불러오기를 중앙 관리

</details>
