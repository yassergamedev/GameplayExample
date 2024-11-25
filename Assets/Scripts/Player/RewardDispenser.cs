using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class  RewardDispenser : MonoBehaviour
{

    public string promptMessage;

    public void BaseOpen()
    {
        Open();
    }
    protected virtual void Open()
    {

    }


    public virtual string OnLook()
    {
        return promptMessage;   
    }
}
