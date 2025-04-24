using UnityEngine;
using UnityEngine.Video;


// video frame 내 특정 frame 에서 event 처리 (walk 소리라던지..)
public class ActorTriggerVideoFrameEventListener : MonoBehaviour
{     
    public int[] triggerFrames; // 이벤트를 발생시킬 프레임 번호

    double _lastFrame = -1; // 마지막 확인된 프레임
    VideoPlayer         _videoPlayer; // VideoPlayer 컴포넌트        
    ActorActionListener _actionScript;

    void Start(){
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.prepareCompleted += OnVideoPrepared;
        _videoPlayer.Prepare();

        _actionScript = transform.parent.gameObject.GetComponent<ActorActionListener>();
    }

    void OnVideoPrepared(VideoPlayer _a_vp)
    {
    }    

    void FixedUpdate()
    {
        // 비디오가 재생 중인지 확인
        if (_videoPlayer.isPlaying)
        {
            // 현재 재생 중인 프레임
            long _currentFrame = _videoPlayer.frame;

            // 중복 실행 방지
            if (_currentFrame != _lastFrame)
            {
                _lastFrame = _currentFrame;

                // 이벤트 프레임 확인
                foreach (int __frame in triggerFrames)
                {
                    if (_currentFrame == __frame)
                    {
                        TriggerEvent(__frame);
                    }
                }
            }
        }
    }

    void TriggerEvent(int _a_frame)
    {
        // 특정 프레임에 실행할 이벤트를 구현
        _actionScript.OnSound("floor", "walk");
        // 여기에 원하는 이벤트 코드를 작성
    }
}

