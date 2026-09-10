# GEA_2026

게임엔진응용 2강 과제 — 점프맵

## 실행

1. Unity Hub에서 이 저장소 폴더를 추가합니다.
2. Unity **6000.3.14f1**으로 엽니다.
3. `Assets/Lesson02/Scenes/Lesson02JumpMap.unity`를 열고 Play를 누릅니다.
4. Game 화면을 클릭하고 **WASD / 방향키**로 이동, **Space**로 점프합니다.

초록색 발판이 시작 지점, 노란색 발판이 도착 지점입니다. 앞으로 이어진 12개 틈을 점프로 건너 도착합니다. 떨어지면 Play를 종료한 후 다시 실행합니다.

## 강의 적용

- PPT 35쪽 전체 코드와 동일한 `Assets/Scripts/PlayerController.cs`를 사용합니다.
- 이동 속도 5, 점프 힘 5, 중력 -20을 유지합니다.
- Player 부모에 CharacterController, Player Input, PlayerController를 붙였습니다.
- CharacterController: Center (0, 0.95, 0), Radius 0.35, Height 1.9, Skin Width 0.08, Step Offset 0.3, Slope Limit 45.
- 자식 캐릭터의 Collider를 제거했습니다.
- 기본 `InputSystem_Actions`를 수정 없이 사용하며 Player Input은 Send Messages, 기본 액션 맵은 Player입니다.
- PPT 36쪽처럼 Main Camera를 Player의 자식으로 두고 상대 Position (0, 4, -6), Rotation (30, 0, 0)을 적용했습니다.

## 마지막 과제

- 시작·도착 포함 13개 발판, 12개 점프 구간을 배치했습니다.
- 발판 사이 간격은 1.3이며 연속 발판을 한 번에 두 개 건너뛸 수 없도록 구성했습니다.
- 계단, 벽, 경사를 섞었으며 모든 지형에 Material과 Collider가 있습니다.
- 이전 강의의 큐브 캐릭터와 재질을 이어서 사용했습니다. 마지막 과제 슬라이드에는 MagicaVoxel 교체가 필수 항목으로 기재되어 있지 않습니다.
- 이전 강의 씬은 `Assets/Lesson01/Scenes/Lesson01Sample.unity`에 보관했습니다.

## 제출

저장소: https://github.com/whxogud9090/GEA_2026

구글 클래스룸에는 저장소 링크를 제출하고, 비공개 댓글에 제출할 커밋의 7자리 해시와 메시지를 기재합니다. 이 저장소 업로드와 별개로 클래스룸 제출은 직접 진행합니다.
