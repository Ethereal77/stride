// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Metrics.ServerApp.Models;

namespace Stride.Metrics.ServerApp.Controllers
{
    internal static class MetricCache
    {
        private static readonly Dictionary<Guid, int> AppIds = new Dictionary<Guid, int>();
        private static readonly Dictionary<Guid, int> InstallIds = new Dictionary<Guid, int>();
        private static readonly Dictionary<Guid, int> MetricIds = new Dictionary<Guid, int>();

        public static int GetApplicationId(this MetricDbContext context, Guid appGuid)
        {
            int id;
            lock (AppIds)
            {
                if (!AppIds.TryGetValue(appGuid, out id))
                {
                    id = -1;
                    var app = context.Apps.FirstOrDefault(t => t.AppGuid == appGuid);
                    if (app != null)
                    {
                        id = app.AppId;
                        AppIds.Add(appGuid, id);
                    }
                }
            }
            return id;
        }

        public static int GetOrCreateInstallId(this MetricDbContext context, Guid installGuid)
        {
            int id;
            lock (InstallIds)
            {
                if (!InstallIds.TryGetValue(installGuid, out id))
                {
                    // Create automatically install id on first encounter
                    var install = context.Installs.FirstOrDefault(t => t.InstallGuid == installGuid);

                    if (install == null)
                    {
                        install = new MetricInstall(installGuid);
                        context.Installs.Add(install);
                        context.SaveChanges();
                    }

                    id = install.InstallId;
                    InstallIds.Add(installGuid, id);
                }
            }
            return id;
        }

        public static int GetMetricId(this MetricDbContext context, Guid metricGuid)
        {
            int id;
            lock (MetricIds)
            {
                if (!MetricIds.TryGetValue(metricGuid, out id))
                {
                    id = -1;
                    var metricDef = context.MetricDefinitions.FirstOrDefault(t => t.MetricGuid == metricGuid);
                    if (metricDef != null)
                    {
                        id = metricDef.MetricId;
                        MetricIds.Add(metricGuid, id);
                    }
                }
            }
            return id;
        }
    }
}