// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Diagnostics;
using Stride.Core.Diagnostics;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.Assets
{
    class AssetLogger : Logger
    {
        private readonly Package package;
        private readonly IReference assetReference;
        private readonly string assetFullPath;
        private readonly ILogger loggerToForward;

        public AssetLogger(Package package, IReference assetReference, string assetFullPath, ILogger loggerToForward)
        {
            this.package = package;
            this.assetReference = assetReference;
            this.assetFullPath = assetFullPath;
            this.loggerToForward = loggerToForward;
            ActivateLog(LogMessageType.Debug);
        }

        protected override void LogRaw(ILogMessage logMessage)
        {
            loggerToForward?.Log(AssetLogMessage.From(package, assetReference, logMessage, assetFullPath));
        }
    }
}
