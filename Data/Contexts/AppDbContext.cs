using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contexts
{
    public class AppDbContext : IdentityDbContext 
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<PartnerRequest> RequestPartners { get; set; }
        public DbSet<FindingPartnerUser> FindingPartnerUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Partnership> Partnerships { get; set; }
        public DbSet<BlockedRelation> BlockedRelations { get; set; }
        public DbSet<LevelTest> LevelTests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
    }
}
