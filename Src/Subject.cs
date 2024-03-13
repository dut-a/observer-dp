//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 
using System.Diagnostics;

namespace PA {
    public class Subject {
        public void Notify() {
            Observer node = poHead;
            while (node != null) {
                node.Notify();
                node = (Observer)node.pNext; // Move to next observer
            }
        }

        public void Detach(Observer pObserver) {
            Debug.Assert(pObserver != null);
            Debug.Assert(poHead != null);

            // If the observer to detach is the head
            if (poHead == pObserver) {
                poHead = (Observer)poHead.pNext;
                pObserver.pNext = null;
            } else {
                Observer prevNode = poHead;
                Observer currentNode = (Observer)poHead.pNext;

                while (currentNode != null) {
                    if (currentNode == pObserver) {
                        prevNode.pNext = currentNode.pNext; // Detach
                        pObserver.pNext = null;
                        break;
                    }

                    prevNode = currentNode;
                    currentNode = (Observer)currentNode.pNext;
                }
            }
        }

        public void Attach(Observer pObserver) {
            Debug.Assert(pObserver != null);
            // Add new observer to the front of the list
            poHead.pNext = pObserver;
        }

        // Holds the observers with a Single Linked list
        // Add to the front of the list O(1)
        private Observer poHead;

    }
}

// --- End of File ---

