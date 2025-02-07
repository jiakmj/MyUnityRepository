# 2025-02-07 필기

</hr>목차

+ 오디오 소스
 + 컴포넌트의 프로퍼티
 + 3D Sound Settings
+ 오디오 믹서
 + 믹서 만드는 방법
+ 유니티 레코더
----------------------
#### 오디오 소스(Audio Source)
오디오 소스는 Scene에서 오디오 클립(Audio Clip)을 재생하는 도구입니다.
재생을 하기 위해서는 오디오 리스너(Audio Listener)나 오디오 믹서(Audio Mixer)를 통해서 재생이 가능합니다.
믹서의 경우는 따로 만들어야 하며, 오디오 리스너의 경우에는 메인 카메라에 붙어있습니다.

![오디오 소스](https://github.com/user-attachments/assets/6d0a6c13-60d6-40e7-b0ee-b3f276bd6905)

```cs
//0) 인스펙터에서 직접 연결하는 경우
public AudioSource audioSourceBGM;    

//1) AudioSourceSample을 객체가 자체적으로 오디오 소스를 가지고 있는 경우
//private AudioSource own_audioSource;

//3) 씬에서 찾아서 연결하는 경우
//4) Resources.Load() 기능을 이용해 리소스 폴더에 있는 오디오 소스의 클립을 받아오겠습니다.
//public AudioSource audioSourceSFX;

    public AudioClip bgm; //오디오 클립에 대한 연결

void Start()
{
    //1)의 경우 GetComponent<T>를 통해 해당 객체가 가지고 있는 오디오 소스 연결 가능
    //own_audioSource = GetComponent<AudioSource>(); //지금은 BGM으로 따로 빼놓아서 X

    //3)의 경우 GameObject.Find().GetComponent<T> 활용
    //GameObject.Find()는 씬에서 찾은 gameObject를 return하는 기능을 가지고 있음. 즉 이 값은 gameObject임
    //GameObject이기 때문에 GetComponent<T>를 이어 작성함으로써 오브젝트가 가진 컴포넌트의 값을 return 합니다.
    //따라서 저 결과물은 AudioSource가 됩니다.
    //audioSourceSFX = GameObject.Find("SFX").GetComponent<AudioSource>();

...

//오디오 소스 스크립트 기능
//audioSourceBGM.Play(); //클립을 실행하는 도구
//audioSourceBGM.Pause(); //일시 정지 기능
//audioSourceSFX.PlayOneShot(bgm); //클립 하나를 한순간 플레이를 진행합니다.
//audioSourceBGM.Stop(); //오디오 클립 재생 중지
//audioSourceBGM.UnPause(); //일시 정지 해제
//audioSourceBGM.PlayDelayed(1.0f); //1초 뒤에 재생

}
```


#### 컴포넌트의 프로퍼티

+ Audio Resource: 재생을 진행할 사운드 클립에 대한 등록
 
+ Output: 기본적으로는 오디오 리스너에 직접 출력됩니다. (None)
  + 만든 오디오 믹서가 있다면 그 믹서를 통해 출력

+ Mute: 체크 시 음소거 처리
+ Bypass Effects: 오디오 소스에 적용되어 있는 필터 효과를 분리
+ Bypass Listener Effect: 리스너 효과를 키거나 끄는 기능
+ Bypass Reverb Zones: 리버브 존을 키거나 끄는 효과
+ 리버브 존: 오디오 리스너의 위치에 따라 잔향 효과를 설정하는 도구
+ Play on Awake: 해당 옵션을 체크했을 경우 씬이 실행되는 시점에 사운드 재생이 처리 됩니다.
  + 해당 기능 비활성화 시 스크립트를 통해 Play() 명령을 진행해 사운드를 재생합니다.
+ Loop: 옵션 활성화 시 재생이 끝날 때 오디오 클립은 루프합니다.
+ Priority: 오디오 소스의 우선 순위
  + 0 = 최우선
  + 128 = 기본
  + 256 = 최하위
+ Volume: 리스너 기준으로 거리 기준 소리에 대한 수치 컴퓨터 내의 소리를 재생하는 여러 가지 요소 중 하나를 제어
+ Pitch: 재생 속도가 빨라지거나 느려질 때의 피치 변화량
  + 1 일반 속도
  + 3 최대 수치 (3배속)
+ Strero Pan: 소리 재생 시 좌우 스피커 간의 소리 분포를 조절 가능
  + -1 왼쪽 스피커
  + 0 양쪽 균등
  + 1 오른쪽 스피커
+ Spatial Blend
  + 0 사운드가 거리와 상관없이 일정하게 들어갑니다. (2D 사운드라고 불림)
  + 1 사운드가 사운드 트는 도구의 거리에 따라 변화
+ Reverb Zone mix: 리버브 존에 대한 출력 신호 양을 조절합니다.
  + 0 영향을 받지 않겠습니다. 
  + 1 오디오 소스와 리버브 존 사이의 신호를 최대치
  + 1.1 10db 증폭
+ 리버브존을 따로 설계해야 하는 상황?
  + 동굴에서 소리가 울리는 효과 연출
  + 건물 등에서 다른 부분을 반사해서 울리는 소리에 대한 설정

#### 3D Sound Settings

+ Doppler Level: 거리에 따른 사운드 높낮이 표현
  + 0 효과 없음
+ Spread: 사운드가 퍼지는 각도 (0 ~ 360)
  + 0 한 점에서 사운드가 나오는 방식
  + 360 모든 방향으로 균일하게 퍼지는 방식

+ Rolloff Mode: 그래프 설정
  + 1. 로그 그래프(Logarithmic Rolloff): 가까우면 사운드가 크고, 멀수록 빠르게 사운드가 작아짐
  + 2. 선형 그래프(Linear Rolloff): 거리에 따라 일정하게 사운드가 변화하는 구조
  + 3. 커스텀 그래프(Custom Rolloff): 직접 조절하는 영역

![오디오소스3d](https://github.com/user-attachments/assets/1a45cef1-295c-4038-bb67-b500500d0b26)

-----------------------------

#### 오디오 믹서(Audio Mixer)
오디오 소스에 대한 제어, 균형, 조정을 제공하는 도구입니다.

+ 믹서 만드는 방법
 + Create -> Audio -> AudioMixer를 통해 Audio Group을 생성합니다.
 + 최초 생성시 Master 그룹이 존재합니다.

![오디오믹서](https://github.com/user-attachments/assets/298c5cdc-2207-4369-a57e-cfba43647894)

--------------------------------

#### 유니티 레코더
+ Package Manager -> Unity Registry -> Recorder
+ window -> General -> Recorder -> Recorder Window
+ Exit Play Mode: 체크되어 있으면 녹화 끝나면 플레이도 끝
+ Recording Mode: Manual(사용자 직접 녹화 설정 종료 가능)
+ Playback: 녹화 중 일정 프레임 속도 유지
+ Target FPS: 녹화 FPS 지정
+ Cap: 설정한 FPS를 넘지 않도록 제한
+ movie 설정

![유니티레코더](https://github.com/user-attachments/assets/41838b7e-b64f-487e-add7-a3e4b0570c69)


