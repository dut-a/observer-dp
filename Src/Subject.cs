//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 
using System;
using System.Diagnostics;

namespace PA
{
    public class Subject
    {
        public void Notify()
        {
            // ------------------------------
            // Add CODE/REFACTOR here
            // ------------------------------
        }

        public void Detach(Observer pObserver)
        {
            Debug.Assert(pObserver != null);
            Debug.Assert(this.poHead != null);
            // ------------------------------
            // Add CODE/REFACTOR here
            // ------------------------------
        }

        public void Attach(Observer pObserver)
        {
            Debug.Assert(pObserver != null);
            // ------------------------------
            // Add CODE/REFACTOR here
            // ------------------------------
        }

        // Holds the observers with a Single Linked list
        // Add to the front of the list O(1)
        private Observer poHead;
        
    }
}

// --- End of File ---

