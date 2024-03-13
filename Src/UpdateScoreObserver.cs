//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 

namespace PA {
    public class UpdateScoreObserver : Observer {
        public override void Notify() {
            MailBox_Observer.Register(MailBox_Observer.Status.UPDATE_SCORE_OBSERVER);
        }
    }
}

// --- End of File ---

