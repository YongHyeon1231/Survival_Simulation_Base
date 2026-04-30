# Survival Simulation

## 👋 프로젝트 소개

- **게임명:** Survival Simulation
- **개발 기간:** 2026.04 ~
- **게임 장르:** 서바이벌 / 시뮬레이션
- **프로젝트 소개:** 자원을 채집하고 건물을 건설하며 생존하는 Unity 기반 싱글플레이 서바이벌 시뮬레이션입니다. NavMesh 기반 AI 유닛, 확률 드롭 인벤토리, 실시간 날씨/주야 시스템을 포함합니다.

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
