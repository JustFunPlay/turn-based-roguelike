using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{

    public AnimationClip clip1;
    public AnimationClip clip2;
    public AnimationClip clip3;
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayAnim()
    {
        anim.Play();
    }

    public void Right()
    {
        anim.clip= clip2;
    }

    public void Left() { 
        anim.clip= clip1;
    }
}
