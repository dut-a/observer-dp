//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 

namespace PA {
    public class RemoveMissileObserver : Observer {
        public override void Notify() {
            MailBox_Observer.Register(MailBox_Observer.Status.REMOVE_MISSILE_OBSERVER);
        }
    }
}

// --- End of File ---

