using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public enum SlingshotState
    {
        Idle,
        UserPulling,
        BirdFlying
    }

    public enum GameState
    {
        Menu,
        Start,
        BirdMovingToSlingshot,
        Playing,
        Won,
        Lost
    }

    public enum BirdState
    {
        BeforeThrown,
        Thrown
    }
    
}
