# Unity 프로젝트 기획서 양식
Unity 프로젝트 기획서는 아래와 같은 양식으로
본인 레포에 README.md에 작성합니다.

## 게임 개요

- __게임 제목__ : FlakHero

- __게임 설명__  
   지상에서 대공포를 쏘아서 공중에 있는 전투기들을 격추시키는 게임이다.
   스테이지는 없고, 뱀파이어 서바이벌처럼 일정시간에 도달할때까지 전투기가 계속 생성되는 방식으로 진행한다.

 - __게임 방법__  
   ~~전투기의 속도와 향하는 위치를 생각해서 총을 발사해야하기 때문에, 요격하는것이 힘들수도 있다.
   이 게임의 특징이라면 전투기가 이동 할 위치로 총구를 겨냥 해 발사해야하기 때문에 일반 FPS와는 달리 예측샷을 요구한다. (게임구현에 있어서 기상상황과 공기저항등은 고려하지 않는다.)~~
   -> bullet 프리팹을 따로 만들지 않고, Raycast를 이용해 적기에 데미지를 주는 방식으로 변경.
   

 - __적기 공격 처리__    
   적기는 맵사이즈 기준 x,y축 100미터 이상 300미터 미만의 좌표에서 랜덤하게 일정한 간격으로 생성된다. 생성되는 즉시 플레이어를 향해간다.

   전투기1 (카미카제식 자폭공격) : 플레이어의 콜라이더에 닿으면 폭발 이펙트와 함께 데미지를 입힌다.
   
   전투기2 (미사일 폭격) : 플레이어를 향해가다가 플레이어와 가까워지면, 전투기 아래에 달려있는 스폰 포인트에서 미사일 프리팹을 생성해서 미사일이 플레이어를 향하도록 하고  
                          미사일은 카미카제와 비슷하게 플레이어의 콜라이더에 닿으면 폭발 이펙트와 함께 데미지를 입힌다.

   공수부대 수송선 : 공격없이 플레이어쪽으로 향하고 플레이어와 가까워지면 공수부대 요원을 3 ~ 5명 랜덤하게 생성한다. 
   
   공수부대 요원 : 낙하산을 펼치고 일정시간동안 떨어진다. 지상에 낙하한 순간 빠른속도로 플레이어쪽으로 총을쏘면서 다가온다. 
   
   
  - __게임 기획 의도__  
   처음에 콜오브 듀티 월드워2라는 게임을 접하고 대공포로 적 전투기를 격추시키는 미션을 플레이하면서 정말 재미있게 플레이했던 기억이 있는데 대공포만 따로 빼서, 다양한 아이템들을 넣어서 캐쥬얼식으로 게임을 제작하면 중독성 넘치게 제작할 수 있을것 같아서 기획하게 되었다. 
   
  - __게임 조작법__  
 
   <PC>  
      마우스 : 상, 하, 좌, 우 화면 움직임(기존 FPS게임과 동일)
      마우스 왼쪽 클릭 : 총알 발사
  
   <모바일>  
      조그 다이얼 : 상, 하, 좌, 우 화면 움직임
   


 - 그 외 비슷한 방식의 게임.  
   Ex) [Anti-AirCraft](https://play.google.com/store/apps/details?id=com.cirepa.AntiAirCraft2)
      
   Ex) [콜오브 듀티 월드워2](https://youtu.be/N_wM0Rm1eik?t=85)

## 개발 환경
프로젝트가 완료된 후 개발 환경을 작성합니다. 기간 / 인원 / 사용한 툴 / 외부 패키지, 라이브러리 등을 작성합니다.

## 링크
프로젝트가 완료된 후 `<a>` 태그의 `href` 속성에 관련된 링크를 작성합니다.

<a href="https://www.youtube.com"><img src="https://img.shields.io/badge/Youtube-FF0000?style=for-the-badge&logo=Youtube&logoColor=white"></a>
