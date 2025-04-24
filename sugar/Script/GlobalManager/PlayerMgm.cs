using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class PlayerMgmtHandler : MonoBehaviour, INpcStatusInterface
{   
    
    // Singleton 인스턴스
    private static PlayerMgmtHandler instance;


    // Singleton 인스턴스에 접근할 수 있는 프로퍼티
    public static PlayerMgmtHandler Instance
    {
        get
        {
            if (instance == null)
            {
                // 게임 내에서 DataManager 오브젝트를 찾아서 가져오거나 생성
                instance = FindFirstObjectByType<PlayerMgmtHandler>();

                if (instance == null)
                {
                    // 게임 내에 DataManager 오브젝트가 없다면 새로 생성
                    GameObject obj = new GameObject("PlayerMgmtHandler");
                    instance = obj.AddComponent<PlayerMgmtHandler>();        
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    
    float health;
    float fatigue;
    float sexual;
    float beauty; 
    float money;
    
    string gender = "M";

    public Slider guage_health; 
    public Slider guage_fatigue;   
    
    public float getHealth() {
        return health;
    }

    public void setHealth(float _health){
        health = _health;
        if (guage_health != null)
        {
            guage_health.value = health;
        }
    }
    
    public float getFatigue() {
        return fatigue;
    }

    public void setFatigue(float _fatigue) {
        fatigue = _fatigue;
        if (guage_fatigue != null)
        {
            guage_fatigue.value = fatigue;
        }        
    }

    public float getSexual() {
        return sexual;
    }

    public void setSexual(float _sexual) {
        sexual = _sexual;
    }

    public float getBeauty() {
        return beauty;
    }

    public void setBeauty(float _beauty) {
        beauty = _beauty;
    }

    public float getMoney() {
        return  money;
    }

    public void setMoney(float _money) {
        money = _money;
    }

    public bool isFemale() {
        if (gender == "M") {
            return false;
        }

        return true;
    }
}
