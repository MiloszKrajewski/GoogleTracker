namespace GoogleTracker
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ResultsStorage : DbContext
    {
        // Your context has been configured to use a 'ResultsStorage' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GoogleTracker.ResultsStorage' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ResultsStorage' 
        // connection string in the application configuration file.
        public ResultsStorage()
            : base("name=ResultsStorage")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<SearchInstance> SearchInstances { get; set; }
        public virtual DbSet<Result> Results { get; set; }
    }
}