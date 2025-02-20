/****************************************************
    File：LoopDragonAni.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 22:17:13
	Description ：飞龙循环动画
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour 
{
    private Animation ani;

    private void Awake()
    {
        ani = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        InvokeRepeating("PlayDragonAni", 0, 20);
    }

    private void PlayDragonAni()
    {
        if (ani != null)
        {
            ani.Play();
        }
    }
}