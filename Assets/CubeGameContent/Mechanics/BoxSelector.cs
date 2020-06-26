using Assets.VR_Controller_scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Valve.VR;


public class BoxSelector : MonoBehaviour
{
   
    public SteamVR_Action_Boolean LeftScroll = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("leftScroll");
    public SteamVR_Action_Boolean RightScroll = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("leftScroll");
    public SteamVR_Input_Sources LeftHand;
    public int ScrollDelayMilliseconds;
    public GameObject player;
    private Boolean scrollReady = true;
    private Boolean isScrollingLeft = false;
    private Boolean isScrollingRight = false;
    public BoxInventory inventory;
    private enum Direction
    {
        LEFT, RIGHT
    }

    void Start()
    {
        LeftScroll.AddOnStateDownListener(ScrollLeft, LeftHand);
        LeftScroll.AddOnStateUpListener(ScrollLeft, LeftHand);
        RightScroll.AddOnStateDownListener(ScrollRight, LeftHand);
        RightScroll.AddOnStateUpListener(ScrollRight, LeftHand);

    }

    private void ScrollRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Scrolling right");
        
        isScrollingRight = !isScrollingRight;
        Scroll(Direction.RIGHT);
    }

    private void ScrollLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Scrolling left");
        isScrollingLeft = !isScrollingLeft;
        Scroll(Direction.LEFT);
    }

    private void Scroll(Direction direction)
    {
       if(direction == Direction.LEFT && scrollReady)
        {
            inventory.ScrollLeft();
            StartScrollDelay();
        }else if(direction == Direction.RIGHT && scrollReady)
        {
            inventory.ScrollRight();
            StartScrollDelay();
        }
    }

    private void StartScrollDelay()
    {
        scrollReady = false;
        Timer timer = new Timer(ScrollDelayMilliseconds);
        timer.Elapsed += EndTimer;
        timer.Start();
    }

    private void EndTimer(object sender, ElapsedEventArgs e)
    {
        scrollReady = true;
        Timer timer = (Timer)sender;
        timer.Stop();
        timer.Dispose();
    }
}