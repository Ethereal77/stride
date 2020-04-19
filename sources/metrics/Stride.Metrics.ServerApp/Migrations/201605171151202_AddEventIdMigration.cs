// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Metrics.ServerApp.Migrations
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
