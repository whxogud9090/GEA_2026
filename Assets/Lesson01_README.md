# 게임엔진응용 1강 샘플

Unity 6.3 LTS의 Universal 3D 프로젝트에서 사용하는 1강 실습 샘플입니다.

## 자동 생성되는 내용

- `Lesson01/Scenes/Lesson01Sample.unity`: 1강 완성 예제 씬
- `Lesson01/Prefabs/Player.prefab`: 부모-자식 구조의 큐브 캐릭터
- `Lesson01/Materials`: 피부, 옷, 청바지, 바닥, 금속, 발광 재질
- `Lesson01/Textures/T_Denim_Indigo.png`: 반복 가능한 짙은 인디고 데님 텍스처
- Ground: Position `(0, 0, 0)`, Scale `(3, 1, 3)`
- Main Camera: Position `(0, 6, -8)`, Rotation `(30, 0, 0)`
- Material Samples: Matte, Metallic, Emission 비교용 큐브
- Practice Steps: 장면 꾸미기용 계단 예제

## 사용 방법

Unity가 스크립트를 컴파일하면 샘플 씬이 한 번 자동 생성됩니다. 생성되지 않았거나 다시 만들고 싶다면 상단 메뉴에서 다음을 실행하세요.

`Game Engine Class > Lesson 01 > Create Sample Scene`

Project 창에서 `Assets/Lesson01/Scenes/Lesson01Sample.unity`를 더블 클릭하고 Play 버튼을 누르면 됩니다.

## 실습 포인트

1. Hierarchy에서 Player를 펼쳐 Body, Head, Arm, Leg가 자식인지 확인합니다.
2. Player의 Position을 바꾸어 모든 자식이 함께 이동하는지 확인합니다.
3. Materials의 색, Metallic, Smoothness, Emission 값을 바꾸어 차이를 관찰합니다.
4. Player에 모자, 꼬리, 무기 큐브를 추가해 자신만의 캐릭터로 확장합니다.

기존 `Assets/Scenes/SampleScene.unity`는 수정하지 않습니다.
