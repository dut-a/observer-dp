//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 

namespace PA {
    public class PlaySoundObserver : Observer {
        public override void Notify() {
            MailBox_Observer.Register(MailBox_Observer.Status.PLAY_SOUND_OBSERVER);
        }
    }
}

// --- End of File ---

