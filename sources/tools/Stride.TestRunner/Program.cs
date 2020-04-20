// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using Mono.Options;

using Stride.ConnectionRouter;
using Stride.Engine.Network;
using Stride.Graphics.Regression;

using static System.String;

namespace Stride.TestRunner
{
    class TestServerHost : RouterServiceServer
    {
        /// <summary>
        /// The name of the branch the test is done on;
        /// </summary>
        private readonly string branchName;

        /// <summary>
        /// The current buildNumber.
        /// </summary>
        private readonly int buildNumber;

        private string resultFile;

        private bool testFailed = true;
        private bool testFinished = false;

        private readonly AutoResetEvent clientResultsEvent = new AutoResetEvent(false);

        public TestServerHost(int bn, string branch) : base("/task/Stride.TestRunner.exe")
        {
            buildNumber = bn;
            branchName = branch;
        }

        /// <summary>
        /// A structure to store information about the connected test devices.
        /// </summary>
        public struct ConnectedDevice
        {
            public string Serial;
            public string Name;
            public TestPlatform Platform;

            public override string ToString()
            {
                return Name + " " + Serial + " " + PlatformPermutator.GetPlatformName(Platform);
            }
        }

        protected override async void HandleClient(SimpleSocket clientSocket, string url)
        {
            clientResultsEvent.Set();

            await AcceptConnection(clientSocket);

            try
            {
                var binaryReader = new BinaryReader(clientSocket.ReadStream);

                //Read events
                TestRunnerMessageType messageType;
                do
                {
                    messageType = (TestRunnerMessageType)binaryReader.ReadInt32();
                    switch (messageType)
                    {
                        case TestRunnerMessageType.TestStarted:
                        {
                            var testFullName = binaryReader.ReadString();
                            Console.WriteLine($"Test Started: {testFullName}");
                            clientResultsEvent.Set();
                            break;
                        }
                        case TestRunnerMessageType.TestFinished:
                        {
                            var testFullName = binaryReader.ReadString();
                            var status = binaryReader.ReadString();
                            Console.WriteLine($"Test {status}: {testFullName}");
                            clientResultsEvent.Set();
                            break;
                        }
                        case TestRunnerMessageType.TestOutput:
                        {
                            var outputType = binaryReader.ReadString();
                            var outputText = binaryReader.ReadString();
                            Console.WriteLine($"  {outputType}: {outputText}");
                            clientResultsEvent.Set();
                            break;
                        }
                        case TestRunnerMessageType.SessionSuccess:
                            testFailed = false;
                            break;
                        case TestRunnerMessageType.SessionFailure:
                            testFailed = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                } while (messageType != TestRunnerMessageType.SessionFailure && messageType != TestRunnerMessageType.SessionSuccess);

                // Mark test session as finished
                testFinished = true;

                //Read output
                var output = binaryReader.ReadString();
                Console.WriteLine(output);

                // Read XML result
                var result = binaryReader.ReadString();
                Console.WriteLine(result);

                // Write XML result to disk
                File.WriteAllText(resultFile, result);

                clientResultsEvent.Set();
            }
            catch (Exception)
            {
                clientResultsEvent.Set();
                Console.WriteLine(@"Client disconnected before sending results, a fatal crash might have occurred.");
            }
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var showHelp = false;
            int exitCode;
            string resultPath = "TestResults";
            bool reinstall = true;

            var p = new OptionSet
            {
                "Copyright (c) Stride contributors (https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp) All Rights Reserved", "Stride Test Suite Tool - Version: " + Format("{0}.{1}.{2}", typeof(Program).Assembly.GetName().Version.Major, typeof(Program).Assembly.GetName().Version.Minor, typeof(Program).Assembly.GetName().Version.Build) + Empty, Format("Usage: {0} [assemblies|apk] -option1 -option2:a", exeName), Empty, "=== Options ===", Empty, { "h|help", "Show this message and exit", v => showHelp = v != null }, { "result-path:", "Result .XML output path", v => resultPath = v }, { "no-reinstall-apk", "Do not reinstall APK", v => reinstall = false },
            };

            try
            {
                var commandArgs = p.Parse(args);
                if (showHelp)
                {
                    p.WriteOptionDescriptions(Console.Out);
                    return 0;
                }

                // Make sure path exists
                Directory.CreateDirectory(resultPath);
                exitCode = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"{0}: {1}", exeName, e);
                if (e is OptionException)
                    p.WriteOptionDescriptions(Console.Out);
                exitCode = 1;
            }

            return exitCode;
        }
    }
}
