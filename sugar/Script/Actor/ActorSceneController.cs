using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActorSceneController : MonoBehaviour
{            

    public GameObject     spriteObject;  // cameraTarget 역할도 동시에 수행됨

    public int textureWidth = 1080;
    public int textureHeight = 1920;

    public VideoPlayer[] videoPlayer_scene;
    public AudioClip    audioClip;

    VideoPlayer    _curVideoPlayer;

    SpriteRenderer _spriteRenderer;    

    List<VideoPlayer> videos = new List<VideoPlayer>();
    List<RenderTexture> textures = new List<RenderTexture>();

    GameObject      _parentMap;
    float           _rotationSpeed = 5.0f;
    float           _angle_default_y = 0.0f;
    float           _angle_range_left = 0.0f;
    float           _angle_range_right = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {   
        _spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();  

        foreach (VideoPlayer videoPlayer in videoPlayer_scene) {
            videos.Add(videoPlayer);
        }

        foreach (VideoPlayer videoPlayer in videos) {
            videoPlayer.loopPointReached += OnVideoEnd;

            if (videoPlayer != null) {
               PrepareNextClip(videoPlayer);                
               RenderTexture texture = new RenderTexture(textureWidth, textureHeight, 24);               
               texture.Create();
               textures.Add(texture);
               videoPlayer.targetTexture = texture;               
            } else {
               textures.Add(null);
            }
        }

        playAnimation();
    }

    // prefab 생성 후 argument 전달 용용
    public void SetInit(GameObject _a_parentMap,  float _a_rotate_y_angle_left , float _a_rotate_y_angle_right){
        _parentMap =  _a_parentMap;
        _angle_range_left = _a_rotate_y_angle_left;
        _angle_range_right = _a_rotate_y_angle_right;
    }

    // scene 의 경우 view 조정 필요
    public void SetView(float _a_rotate_y_angle) {
        if (_parentMap) {
            _parentMap.transform.Rotate(0f, _a_rotate_y_angle, 0f);               
        } 
    }

    public void SetDefaultView() {
        if (_parentMap) {
            _parentMap.transform.rotation = Quaternion.identity;
            // float _originParentMapY = _parentMap.transform.eulerAngles.y;
            // Debug.Log($"y {_originParentMapY}");

            // _parentMap.transform.Rotate(0f, -_originParentMapY, 0f);       
        } 
    }

    public void ChangeActorColor(Color _a_color) {
        _spriteRenderer.color = _a_color;
    }

    void FixedUpdate()
    {    
        if (Input.anyKey) {         
            if (Input.GetKey(KeyCode.E)) {
                if(_parentMap) {                              
                }
            }                

            float currentRotationY = _parentMap.transform.eulerAngles.y;       
            // rotatation y값 0 에서 오른쪽 이동 시 양수 증가, 왼쪽 이동 시 음수 증가
            if (_parentMap.transform.rotation.y < 0) { // # 음수인 경우 eulerAngles.y 가 0에서 360으로 전달된다는 의미
                currentRotationY = currentRotationY - 360;
            }

            if (Input.GetKey(KeyCode.LeftArrow) && currentRotationY  > _angle_range_left) {            
                if(_parentMap) {                    
                    _parentMap.transform.Rotate(0f, -_rotationSpeed * Time.deltaTime, 0f);   
                }
            }

            if (Input.GetKey(KeyCode.RightArrow) && currentRotationY < _angle_range_right) {
               if(_parentMap) {                   
                   _parentMap.transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f);   
                }              
            }          
        }
    }

    void playAnimation() {
        // virtual camemra 찾기
        GameObject _cameraObject = GameObject.FindWithTag("VirtualCamera");
        if (_cameraObject != null) {
            CinemachineCamera virtualCamera = _cameraObject.GetComponent<CinemachineCamera>();
            SetTrackingTarget(virtualCamera);
        } else {
            Debug.Log("there is no virtualCamera");
        }

        System.Random random = new System.Random();

        int randomValue = random.Next(0, videoPlayer_scene.Length); // 0 (포함) ~ 2 (제외) 사이의 정수           
        UpdateRenderTexture(videoPlayer_scene[randomValue]);
    }

    void UpdateRenderTexture(VideoPlayer curVideoPlayer)
    {
        if (curVideoPlayer != null) {
            curVideoPlayer.Play();
            curVideoPlayer.frame = 0;
            curVideoPlayer.targetMaterialRenderer = _spriteRenderer;   
            _curVideoPlayer =  curVideoPlayer;                            
        }
    }    

    void PrepareNextClip(VideoPlayer vp)
    {
        if (vp != null) {
            vp.Prepare();
            vp.Pause();
        }
    }
    
    void SetTrackingTarget(CinemachineCamera virtualCamera)
    {
        if (virtualCamera != null)
        {
            // Follow 및 LookAt 속성 설정
            virtualCamera.Follow = spriteObject.transform;
            virtualCamera.LookAt = spriteObject.transform;

            Debug.Log($"CinemachineCamera Follow/LookAt이 '{spriteObject.transform.name}'으로 설정되었습니다.");
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
    }

    void OnDestroy() {
        foreach (VideoPlayer videoPlayer in videos) {
            videoPlayer.loopPointReached -= OnVideoEnd;
            if (videoPlayer != null)
                Destroy(videoPlayer);
        }

        foreach (RenderTexture renderTexture in textures) {
            if (renderTexture != null) {
                renderTexture.Release();
                Destroy(renderTexture);
            }
        }
    }
}


