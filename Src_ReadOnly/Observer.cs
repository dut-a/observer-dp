//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 

using System.Diagnostics;

// ----------------------------------
// ---     DO NOT MODIFY FILE     ---
// ----------------------------------

namespace PA {
    abstract public class Observer : SLink {
        public virtual void Notify() {
            // NORMALLY this is an abstract method...
            //    Its virtual and implemented for unit testing purposes
            //    Treat this method "as if" it is abstract...so override in derived
            Debug.Assert(false);
        }

        public Observer() {
            this.pSubject = null;
        }
        public Subject GetSubject() {
            return this.pSubject;
        }
        public void SetSubject(Subject _pSubject) {
            Debug.Assert(_pSubject != null);
            this.pSubject = _pSubject;
        }

        private Subject pSubject;
    }
}

// --- End of File ---

