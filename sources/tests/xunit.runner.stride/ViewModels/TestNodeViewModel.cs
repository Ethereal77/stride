// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using ReactiveUI;

using Xunit.Abstractions;

namespace xunit.runner.stride.ViewModels
{
    public abstract class TestNodeViewModel : ViewModelBase
    {
        public abstract IEnumerable<TestCaseViewModel> EnumerateTestCases();

        public abstract TestCaseViewModel LocateTestCase(ITestCase testCase);

        bool running;
        public bool Running
        {
            get => running;
            set => this.RaiseAndSetIfChanged(ref running, value);
        }

        bool failed;
        public bool Failed
        {
            get => failed;
            set => this.RaiseAndSetIfChanged(ref failed, value);
        }

        bool succeeded;
        public bool Succeeded
        {
            get => succeeded;
            set => this.RaiseAndSetIfChanged(ref succeeded, value);
        }

        public abstract string DisplayName { get; }
    }
}
