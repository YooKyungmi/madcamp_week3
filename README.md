# MSG : Madcamp Survival Game
## A. 개발 팀원
유경미, 권혁원

## B. 사용 언어
Unity
C#
Asset : https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068

## C. 사용 목적
뱀파이어 서바이벌 라이크, 이른바 '뱀서라이크'류 게임을 통해서 무한히 성장하는 재미와 적들을 쓸어버리는 재미를 느낀다.

## D. Application 소개
### Start
> 캐릭터 선택

플레이어 케릭터를 선택한다.
![image](https://github.com/YooKyungmi/madcamp_week3/assets/52480724/d1d46592-3377-4f76-b92e-b778fb437c67)
### 전투
자동으로 조준 및 사출되는 총과 주변을 도는 무기인 삽으로 몰려오는 적을 공격한다.

### 레벨 업
몰려오는 적을 죽이면 보석이 생성된다.
보석은 플레이어에게 인력이 작용한다.
보석이 플레이어에게 닿으면 경험치가 올라가며, 경험치를 최대로 획득하면 레벨이 오른다.
레벨이 오르면 무기를 선택하여 강화할 수 있다.

### 웨이브
2분을 주기로 진행된다.
30초마다 생성되는 적의 종류가 늘어난다.
1분마다 수많은 적이 생성되어 몰려온다.
2분마다 생성되는 적의 종류가 초기화된다.

### 드랍
맵을 이동하다 보면 상자가 생성된다.
![image](https://github.com/YooKyungmi/madcamp_week3/assets/52480724/1d538e3d-6650-4827-93f8-9d48786d9cee)

상자를 깨면 아이템이 드랍된다.
- 폭탄 : 맵의 모든 적을 죽인다.
- 자석 : 맵의 모든 아이템에 인력을 설정한다.
- 포션 : 체력을 회복한다.

### 업적
게임 진행 상황에 따라 새로운 캐릭터가 해금된다.
![image](https://github.com/YooKyungmi/madcamp_week3/assets/52480724/7b88cecd-4d6e-490e-8683-3807f72e7d4d)
