//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 

// ----------------------------------
// ---     DO NOT MODIFY FILE     ---
// ----------------------------------

namespace PA {
    public class MailBox_Observer {

        public enum Status {
            EMPTY,
            PLAY_SOUND_OBSERVER,
            REMOVE_MISSILE_OBSERVER,
            REMOVE_ALIEN_OBSERVER,
            UPDATE_SCORE_OBSERVER
        }
        public static void Register(Status s) {
            MailBox_Observer.status[WriteIndex++] = s;
        }

        // ---------------------------
        // Used for testing
        // ---------------------------
        public static Status TestValue() {
            Status s = MailBox_Observer.status[ReadIndex++];
            return s;
        }

        private static Status[] status = new Status[40];
        private static int WriteIndex = 0;
        private static int ReadIndex = 0;
    }
}

// --- End of File ---

