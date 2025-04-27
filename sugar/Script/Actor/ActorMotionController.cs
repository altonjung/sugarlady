using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ActorMotionController : MonoBehaviour
{
    public VideoPlayer videoPlayerWalk_down;
    public VideoPlayer videoPlayerWalk_up;
    public VideoPlayer videoPlayerWalk_left;
    public VideoPlayer videoPlayerWalk_down_left;
    public VideoPlayer videoPlayerWalk_up_left;
    public VideoPlayer videoPlayerWalk_right;
    public VideoPlayer videoPlayerWalk_down_right;
    public VideoPlayer videoPlayerWalk_up_right;

    public VideoPlayer[] videoPlayerIdle_down;
    public VideoPlayer[] videoPlayerIdle_up;
    public VideoPlayer[] videoPlayerIdle_left;
    public VideoPlayer[] videoPlayerIdle_down_left;
    public VideoPlayer[] videoPlayerIdle_up_left;
    public VideoPlayer[] videoPlayerIdle_right;
    public VideoPlayer[] videoPlayerIdle_down_right;
    public VideoPlayer[] videoPlayerIdle_up_right;

    // 그림자 관리
    public GameObject shadowObject;

    public float speed = 2.0f;

    public int idleLoopStartIdx = 5; // 동영상내 idle 시작 index

    float _lastInputTime;

    bool _isShow;

    bool _idle;

    bool _isRestrictMove;

    bool[] _raystatusmap = new bool[2];  // 0: wall collision

    bool[] _keymap = new bool[4];

    VideoPlayer _curVideoPlayer;
    SpriteRenderer _spriteRenderer;

    List<VideoPlayer> _videos = new List<VideoPlayer>();
    List<GameObject> _prevRayCastObjs = new List<GameObject>();
    List<GameObject> _curRayCastObjs = new List<GameObject>();
  

  
    void init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _videos.Add(videoPlayerWalk_down);
        _videos.Add(videoPlayerWalk_up);
        _videos.Add(videoPlayerWalk_left);
        _videos.Add(videoPlayerWalk_down_left);
        _videos.Add(videoPlayerWalk_up_left);
        _videos.Add(videoPlayerWalk_right);
        _videos.Add(videoPlayerWalk_down_right);
        _videos.Add(videoPlayerWalk_up_right);

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_down)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_up)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_left)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_down_left)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_up_left)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_right)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_down_right)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videoPlayerIdle_up_right)
        {
            _videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in _videos)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            PrepareNextClip(videoPlayer);
        }
    }

    void OnEnable()
    {
        init();

        // virtual camemra 찾기
        GameObject _cameraObject = GameObject.FindWithTag("VirtualCamera");
        if (_cameraObject != null)
        {
            CinemachineCamera virtualCamera = _cameraObject.GetComponent<CinemachineCamera>();
            SetTrackingTarget(virtualCamera);
        }
        else
        {
            Debug.Log("there is no virtualCamera");
        }
        Array.Fill(_raystatusmap, false);
        Array.Fill(_keymap, false);
        _idle = true;
        _keymap[3] = true;
        playMoveAnimation();
    }

    void OnDisable()
    {
        _videos.Clear();
    }

    void Start()
    {
        init();

        _isShow = true;
        _idle = true;
        _isRestrictMove = false;
        _lastInputTime = Time.time;

        _keymap[3] = true;
        playMoveAnimation();
    }

    public void SetActorPosition(Vector3 _a_position)
    {
        _a_position.y = -0.0f;
        this.transform.position = _a_position; 

        Debug.Log($" 수정된 sprite transform {transform.position}");
    }

    public string GetActorDirection()
    {
        if (_keymap[0])
            return "left";
        else if (_keymap[1])
            return "right";
        else if (_keymap[2])
            return "up";
        else
            return "down";
    }

    public void SetActorDirection(string _a_direction)
    {
        if (_a_direction == "left")
            _keymap[0] = true;
        else if (_a_direction == "right")
            _keymap[1] = true;
        else if (_a_direction == "up")
            _keymap[2] = true;
        else
            _keymap[3] = true;
    }

    public void RestrictAllMove()
    {
        _isRestrictMove = true;
    }

    public void ReleaseAllMove()
    {
        _isRestrictMove = false;
    }

    public void changeActorColor(Color _a_color)
    {
        _spriteRenderer.color = _a_color;
    }

    void FixedUpdate()
    {
        if (!_isShow)
        {
            return;
        }

        if (_isRestrictMove)
        {
            _idle = true;
            playMoveAnimation();
            return;
        }

        _lastInputTime = Time.time;

        if (Input.anyKey)
        {
            _idle = false;
            Array.Fill(_keymap, false);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _keymap[0] = true;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                _keymap[1] = true;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _keymap[2] = true;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                _keymap[3] = true;
            }

            if (_keymap[0] || _keymap[1] || _keymap[2] || _keymap[3])
            {
                RayCast();

                if (_raystatusmap[0]) // wall collision is true
                {
                    _idle = true;
                }
                else
                {
                    ChangePosition(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                }

                playMoveAnimation();
            }
        }
        else
        {
            if (!_idle)
            {
                _idle = true;
                playMoveAnimation();
            }
        }
    }

    // rigidBody 처리 이후, transform 내 y값 고정하여, player 출력 위치 조정
    void LateUpdate()
    {
        float _anchorY = 0.4f; // 해당값을 통해 player y 값 위치 조정
        Vector3 currentPosition = transform.position;
        currentPosition.y = _anchorY;
        transform.position = currentPosition;
    }

    void playMoveAnimation()
    {
        VideoPlayer _targetVideo;
        bool isLeft = _keymap[0]; // left
        bool isRight = _keymap[1]; // right
        bool isUp = _keymap[2]; // up
        bool isDown = _keymap[3]; // down

        if (!_idle)
        {
            if (isDown)
            {
                if (isLeft)
                {
                    _targetVideo = videoPlayerWalk_down_left;
                }
                else if (isRight)
                {
                    _targetVideo = videoPlayerWalk_down_right;
                }
                else
                {
                    _targetVideo = videoPlayerWalk_down;
                }
            }
            else if (isUp)
            {
                if (isLeft)
                {
                    _targetVideo = videoPlayerWalk_up_left;
                }
                else if (isRight)
                {
                    _targetVideo = videoPlayerWalk_up_right;
                }
                else
                {
                    _targetVideo = videoPlayerWalk_up;
                }
            }
            else if (isLeft)
            {
                _targetVideo = videoPlayerWalk_left;
            }
            else if (isRight)
            {
                _targetVideo = videoPlayerWalk_right;
            }
            else
            {
                return;
            }
        }
        else
        {
            int _idx = 0;
            if (isDown)
            {
                if (isLeft)
                {
                    _targetVideo = videoPlayerIdle_down_left[_idx];
                }
                else if (isRight)
                {
                    _targetVideo = videoPlayerIdle_down_right[_idx];
                }
                else
                {
                    _targetVideo = videoPlayerIdle_down[_idx];
                }
            }
            else if (isUp)
            {
                if (isLeft)
                {
                    _targetVideo = videoPlayerIdle_up_left[_idx];
                }
                else if (isRight)
                {
                    _targetVideo = videoPlayerIdle_up_right[_idx];
                }
                else
                {
                    _targetVideo = videoPlayerIdle_up[_idx];
                }
            }
            else if (isLeft)
            {
                _targetVideo = videoPlayerIdle_left[_idx];
            }
            else if (isRight)
            {
                _targetVideo = videoPlayerIdle_right[_idx];
            }
            else
            {
                return;
            }
        }

        if (_curVideoPlayer == _targetVideo)
            return;

        if (_curVideoPlayer == null)
        {
            UpdateRenderTexture(null, _targetVideo);
        }
        else
        {
            UpdateRenderTexture(_curVideoPlayer, _targetVideo);
        }
    }

    void UpdateRenderTexture(VideoPlayer _a_prevVideoPlayer, VideoPlayer _a_curVideoPlayer)
    {
        if (_a_prevVideoPlayer != null)
        {
            _a_prevVideoPlayer.Pause();
            _a_prevVideoPlayer.frame = 0;
            _a_prevVideoPlayer.targetMaterialRenderer = null;
        }

        if (_a_curVideoPlayer != null)
        {
            _a_curVideoPlayer.Play();
            _a_curVideoPlayer.frame = 0;
            _a_curVideoPlayer.targetMaterialRenderer = _spriteRenderer;
            _curVideoPlayer = _a_curVideoPlayer;        
        }
    }

    // raycast 충돌 처리
    void RayCast()
    {   
        Array.Fill(_raystatusmap, false);
        Vector3 cast = new Vector3(0.0f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cast = new Vector3(-1.0f, 0.1f, 0.0f); // left (X축)
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            cast = new Vector3(1.0f, 0.1f, 0.0f); // right (X축)
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            cast = new Vector3(0.0f, 0.1f, 1.0f); // forward (z축)
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            cast = new Vector3(0.0f, 0.1f, -1.0f); //  forward   (z축)
        }

        Ray ray = new Ray(transform.position, cast); // 앞쪽으로 레이 쏘기

        Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.red);

        int _layerMask = LayerMask.GetMask("Object");

        RaycastHit[] hits = Physics.RaycastAll(ray, 10.0f, _layerMask);

        _curRayCastObjs.Clear();

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits) {
                if (hit.collider.gameObject.tag == "Item" || hit.collider.gameObject.tag == "Door")
                {
                    _curRayCastObjs.Add(hit.collider.gameObject);
                }
            }
        }
        
        List<GameObject> _active_list =  _curRayCastObjs.Intersect(_prevRayCastObjs).ToList();
        foreach (GameObject _object in _active_list) {
            if (_object != null) {
                _ObjectController _script = _object.GetComponent<_ObjectController>();
                _script.ShowItemMark();
            }
        }

        List<GameObject> _inactive_list = _prevRayCastObjs.Except(_curRayCastObjs).ToList();
        foreach (GameObject _object in _inactive_list) {
            if (_object != null) {
                _ObjectController _script = _object.GetComponent<_ObjectController>();
                _script.HideItemMark();
            }
        }

        _prevRayCastObjs.Clear();
        _prevRayCastObjs.AddRange(_curRayCastObjs);

        RaycastHit _hit;
        _layerMask = LayerMask.GetMask("Wall");
        if (Physics.Raycast(ray, out _hit, 10.0f, _layerMask))
        {
            // Debug.Log($"Collider: {hit.collider.gameObject.name}, {hit.distance}");
            if (_hit.collider.gameObject.tag == "Wall" && _hit.distance < 0.93)
            {
                _raystatusmap[0] = true;
            }
        }
    }


    void ChangePosition(float _a_horizontal, float _a_vertical)
    {
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        Vector3 originPosition = rigidBody.transform.position;
        Vector3 targetPosition = new Vector3(
            originPosition.x + (speed / 1.5f) * _a_horizontal,
            0.0f,
            originPosition.z + (speed / 1.5f) * _a_vertical
        );
        rigidBody.MovePosition(targetPosition);
    }

    // 가상 카메라 자동 조정
    void SetTrackingTarget(CinemachineCamera _a_virtualCamera)
    {
        if (_a_virtualCamera != null)
        {
            // Follow 및 LookAt 속성 설정
            _a_virtualCamera.Follow = transform;
            _a_virtualCamera.LookAt = transform;

            Debug.Log($"CinemachineCamera Follow/LookAt이 '{transform.name}'으로 설정되었습니다.");

            // Debug.Log($" 수정된 sprite transform {transform.position}");
        }
    }

    // 동영상 미리 로딩
    void PrepareNextClip(VideoPlayer _a_vp)
    {
        if (_a_vp != null)
        {
            _a_vp.Prepare();
            _a_vp.Pause();
        }
    }

    // Idle 모션 처리
    void OnVideoEnd(VideoPlayer _a_vp)
    {
        if (_idle)
        {
            StartCoroutine(PlayIdleFrames(_a_vp));
        }
    }

    IEnumerator PlayIdleFrames(VideoPlayer _a_vp)
    {
        _a_vp.Pause();
        _a_vp.frame = idleLoopStartIdx;

        // 프레임 이동이 완료될 때까지 대기
        while (_a_vp.frame != idleLoopStartIdx)
        {
            yield return null;
        }

        _a_vp.Play();
    }

    void OnDestroy()
    {
        foreach (VideoPlayer __videoPlayer in _videos)
        {
            __videoPlayer.loopPointReached -= OnVideoEnd;
            if (__videoPlayer != null)
                Destroy(__videoPlayer);
        }

        _videos.Clear();
    }
}
