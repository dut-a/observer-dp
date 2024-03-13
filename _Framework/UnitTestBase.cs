//-----------------------------------------------------------------------------
// Copyright 2023, Ed Keenan, all rights reserved.
//----------------------------------------------------------------------------- 
//
// Simple C# Unit Test Framework
// Copyright 2014-2023 Ed Keenan All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of Ed Keenan nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// Author/Architect: Ed Keenan - ekeenan2@cdm.depaul.edu
//----------------------------------------------------------------------------- 

// ----------------------------------
// ---     DO NOT MODIFY FILE     ---
// ----------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace UnitTest
{
    class DummyException : Exception
    {
        public DummyException()
        {
            // dummy exception to force the next call up the stack to deal with problem
            // great example on how to exploit exceptions for evil
            return;
        }
    }

    public class Framework
    {
        public struct UnitStats
        {
            public int testCount;
            public int testPass;
            public int testFail;
            public int testDisabled;
            public int indvAsserts;
        }

        public struct UnitData
        {
            public bool result;
            public bool ignore;
            public string memberName;
            public string sourceFilePath;
            public int sourceLineNumber;
        }

        public Framework()
        {
            this.data.result = true;
            this.data.memberName = "";
            this.data.sourceFilePath = "";
            this.data.sourceLineNumber = 0;

            this.stats.testCount = 0;
            this.stats.testFail = 0;
            this.stats.testPass = 0;
            this.stats.testDisabled = 0;
            this.stats.indvAsserts = 0;
        }

        public static void Dump(string value)
        {
            Debug.Write(value);
            Console.Write(value);
        }

        public static void runMe()
        {

            string FRAMEWORK_VER = "1.00";

            Dump("\n");
            Dump("***********************************\n");
            Dump("**      Framework: " + FRAMEWORK_VER + "          **\n");
            Dump("**   C++ Compiler: " + typeof(string).Assembly.ImageRuntimeVersion + "    **\n");
            Dump("**           Mode: Debug         **\n");
            Dump("***********************************\n");
            Dump("\n");


            var baseType = typeof(UnitTestBase);
            var assembly = typeof(UnitTestBase).Assembly;
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

            foreach (var subType in types)
            {
                string ns = subType.FullName;
                object obj = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(ns);

                foreach (MethodInfo item in subType.GetRuntimeMethods())
                {
                    string methodName = new string(item.Name.ToCharArray());
                    MethodInfo method = subType.GetMethod(methodName);

                    // Stupid filter to deal with default methods
                    if (methodName != "Equals" &&
                        methodName != "ToString" &&
                        methodName != "GetHashCode" &&
                        methodName != "GetType" &&
                        methodName != "Finalize" &&
                        methodName != "MemberwiseClone")
                    {

                        // OK, here's where its fucked up....
                        // Once in the CustomUnitTests methods fail, abort - process no more code.
                        // To do this, we throw an exception in CHECK(), it get passed up the call stack to here
                        // here we do nothing, but it handles the exception, since there is a catch
                        // Viola - we exited the unit test on the first fail
                        try
                        {
                            method.Invoke(obj, null);
                        }
                        catch
                        {
                        }

                        Framework testResults = Framework.getResults();

                        testResults.stats.testCount++;
                        if (testResults.data.ignore == true)
                        {
                            testResults.stats.testDisabled++;
                            Dump("-IGNORE: " + testResults.data.memberName + "()\n");

                        }
                        else
                        {
                            if (testResults.data.result == false)
                            {
                                testResults.stats.testFail++;
                                Dump("-FAILED: " + testResults.data.memberName + "()\n");
                                Dump("    CHECK failure (" + testResults.data.result + ")\n");
                                // write to output window, in a way that is clickable
                                // format needs to be filename(lineNumber): to work



                                Dump("    " + testResults.data.sourceFilePath);
                                Dump("(" + testResults.data.sourceLineNumber + "): < double - click >\n");

                                // write to output window, in a way that is clickable
                                // format needs to be filename(lineNumber): to work
                                //Trace.Write(testResults.data.sourceFilePath);
                                //Trace.Write("(" + testResults.data.sourceLineNumber + "): ");
                                //Trace.Write(testResults.data.memberName + "() - ");
                                //Trace.WriteLine("CHECK failure (" + testResults.data.result + ")");
                            }
                            else
                            {
                                testResults.stats.testPass++;
                                Dump(" PASSED: " + testResults.data.memberName + "()\n");
                            }
                        }
                        // prepare the singleton for the next pass
                        Framework.ready();

                    } // filter
                } // foreach methods
            } // foreach types

            // print results:
            Framework testResults2 = Framework.getResults();

            string mode = "x86 Debug";
            Dump("\n");
            Dump("  --- Tests Results ---    \n");
            Dump("\n");
            Dump("[" + mode + "] Ignored: " + testResults2.stats.testDisabled + "\n");
            Dump("[" + mode + "]  Passed: " + testResults2.stats.testPass + "\n");
            Dump("[" + mode + "]  Failed: " + testResults2.stats.testFail + "\n");
            Dump("\n");
            Dump("   Test Count: " + testResults2.stats.testCount + "\n");
            Dump(" Indiv Checks: " + testResults2.stats.indvAsserts + "\n");
            Dump("         Mode: " + mode + "\n");
            Dump("\n");
            Dump("-----------------\n");

        } // runME


        public static Framework getResults()
        {
            return Framework.privInstance();
        }

        public static void ready()
        {
            // Partial reset between tests
            Framework t = Framework.privInstance();

            t.data.ignore = true;
            t.data.result = true;
            t.data.memberName = "";
            t.data.sourceFilePath = "";
            t.data.sourceLineNumber = 0;
        }

        // ---- Private: -----------------------------------------

        private static Framework privInstance()
        {
            // Singleton - baby!
            if (instance == null)
            {
                instance = new Framework();
            }

            return instance;
        }
        private static Framework instance;

        // ---------------------------------------------
        public UnitData data;
        public UnitStats stats;

    } // class Framework

    public class UnitTestBase
    {
        public static void IGNORE(
                                [CallerMemberName] string inMemberName = "",
                                [CallerFilePath] string inSourceFilePath = "",
                                [CallerLineNumber] int inSourceLineNumber = 0)
        {
            Framework t = Framework.getResults();

            t.data.ignore = true;
            t.data.result = false;
            t.data.memberName = inMemberName;
            t.data.sourceFilePath = inSourceFilePath;
            t.data.sourceLineNumber = inSourceLineNumber;

            t.stats.indvAsserts++;

            // OK here's the situation, if we have failed a CHECK,
            // exit out of this testMethod() goto next testMethod()
            // since C# doesn't have macros, I managed to get it to work with exceptions.

            // force an dummy exception, do nothing
            // this kicks the can up the stack (GREAT! that's what we want)
            // at the next higher level handle the imaginary dummy exception (do nothing)
            // viola - we exited out of testMethod() from an outside function
            // bartender - get me a drink!
            //if (inResult == false)
            //{
            //    // dummy exception to force us to walk up the stack.
            //    throw new DummyException();
            //}
        }

        public static void CHECK(bool inResult,
                                [CallerMemberName] string inMemberName = "",
                                [CallerFilePath] string inSourceFilePath = "",
                                [CallerLineNumber] int inSourceLineNumber = 0)
        {
            Framework t = Framework.getResults();

            t.data.ignore = false;
            t.data.result = inResult;
            t.data.memberName = inMemberName;
            t.data.sourceFilePath = inSourceFilePath;
            t.data.sourceLineNumber = inSourceLineNumber;

            t.stats.indvAsserts++;

            // OK here's the situation, if we have failed a CHECK,
            // exit out of this testMethod() goto next testMethod()
            // since C# doesn't have macros, I managed to get it to work with exceptions.

            // force an dummy exception, do nothing
            // this kicks the can up the stack (GREAT! that's what we want)
            // at the next higher level handle the imaginary dummy exception (do nothing)
            // viola - we exited out of testMethod() from an outside function
            // bartender - get me a drink!
            if (inResult == false)
            {
                // dummy exception to force us to walk up the stack.
                throw new DummyException();
            }
        }

    } // class UnitTestBase

    public class Utility
    {
        public static bool AreEqual(float a, float b, float epsilon = 0.001f)
        {
            return (Math.Abs(a - b) < epsilon);
        }

        public static bool AreEqual(double a, double b, double epsilon = 0.001f)
        {
            return (Math.Abs(a - b) < epsilon);
        }


    }

} // namespace UnitTest

// --- End of File ---
