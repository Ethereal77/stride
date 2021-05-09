// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Metrics.ServerApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventIdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetricEvents", "EventId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetricEvents", "EventId");
        }
    }
}
