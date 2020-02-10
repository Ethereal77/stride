// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Microsoft.Owin;

using Owin;

using Xenko.Metrics.ServerApp;

[assembly: OwinStartup(typeof(Xenko.Metrics.ServerApp.Startup))]

namespace Xenko.Metrics.ServerApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseMetricServer();
        }
    }
}
