// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Diagnostics;
using Xenko.Core.Diagnostics;
using Xenko.Core.Serialization.Contents;

namespace Xenko.Core.Assets
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
