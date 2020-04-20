// Copyright (c) Stride contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more in

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
