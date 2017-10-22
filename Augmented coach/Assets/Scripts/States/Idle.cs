﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State {
    
    public Idle()
    {
        id = StateID.IdleID;
    }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {

    }

    public override StateID Reason()
    {
        return base.Reason();
    }

    public override void Exit()
    {
        
    }

}
