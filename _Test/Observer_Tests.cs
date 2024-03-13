//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 

using System;
using System.Diagnostics;
using UnitTest;

// ----------------------------------
// ---     DO NOT MODIFY FILE     ---
// ----------------------------------

namespace PA
{
    public class Flyweight_Tests : UnitTestBase
    {
        public void Observer_Shakeout()
        {
            if (Tests_Flags.Shakeout_Enable)
            {

                Subject pSubject = new Subject();
                CHECK(pSubject != null);

                Observer pA = new UpdateScoreObserver();
                CHECK(pA != null);

                Observer pB = new RemoveAlienObserver();
                CHECK(pB != null);

                Observer pC = new RemoveMissileObserver();
                CHECK(pC != null);

                Observer pD = new PlaySoundObserver();
                CHECK(pD != null);

                // Attach by pushing to front
                pSubject.Attach(pA);
                CHECK(pA.GetSubject() == pSubject);

                pSubject.Attach(pB);
                CHECK(pB.GetSubject() == pSubject);

                pSubject.Attach(pC);
                CHECK(pC.GetSubject() == pSubject);

                pSubject.Attach(pD);
                CHECK(pD.GetSubject() == pSubject);

                pSubject.Notify();

                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.PLAY_SOUND_OBSERVER);
                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.REMOVE_MISSILE_OBSERVER);
                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.REMOVE_ALIEN_OBSERVER);
                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.UPDATE_SCORE_OBSERVER);

                pSubject.Detach(pA);
                pSubject.Detach(pD);

                pSubject.Notify();

                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.REMOVE_MISSILE_OBSERVER);
                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.REMOVE_ALIEN_OBSERVER);

                pSubject.Detach(pB);

                pSubject.Notify();

                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.REMOVE_MISSILE_OBSERVER);

                pSubject.Detach(pC);

                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.EMPTY);

                pSubject.Notify();

                CHECK(MailBox_Observer.TestValue() == MailBox_Observer.Status.EMPTY);

            }
            else
            {
                IGNORE();
            }
        }

    }
}

// --- End of File ---

