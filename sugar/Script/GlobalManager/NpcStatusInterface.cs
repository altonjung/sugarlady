using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface  INpcStatusInterface
{
    // Start is called before the first frame update

    public float getHealth();

    public void setHealth(float health);
    
    public float getFatigue();

    public void setFatigue(float health);

    public float getSexual();

    public void setSexual(float health);

    public float getBeauty();

    public void setBeauty(float health);

    public float getMoney();

    public void setMoney(float health);

    public bool isFemale();
}
