// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading;

using Mono.Options;

using ServiceWire.NamedPipes;

namespace Stride.VisualStudio.Commands
{
    class Program
    {
        public static void Main(string[] args)
        {
            string pipeAddress = string.Empty;

            var p = new OptionSet
            {
                { "pipe=", "Pipe for communication", v => pipeAddress = v },
            };

            p.Parse(args);

            var host = new NpHost(pipeAddress + "/IStrideCommands");
            host.AddService<IStrideCommands>(new StrideCommands());
            host.Open();

            // Forbid process to terminate (unless ctrl+c)
            while (true)
            {
                Console.Read();
                Thread.Sleep(100);
            }
        }
    }
}
