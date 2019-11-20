using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            builder.Entity<Comment>()
                .HasOne(p => p.Parent)
                .WithMany(s => s.SubComments)
                .HasForeignKey(x => x.ParentId);
            builder.Entity<BlockedRelation>()
                .HasOne(o => o.Owner)
                .WithMany(b => b.BlockedRelations)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany(s => s.SentMessages)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Notification>()
                .HasOne(o => o.Owner)
                .WithMany(n => n.Notifications)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PartnerRequest>()
                .HasOne(s => s.Sender)
                .WithMany(s => s.SentPartnerRequests)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Partnership>()
                .HasOne(o => o.Owner)
                .WithMany(p => p.Partnerships)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<PartnerRequest> PartnerRequests { get; set; }
        public DbSet<FindingPartnerUser> FindingPartnerUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Partnership> Partnerships { get; set; }
        public DbSet<BlockedRelation> BlockedRelations { get; set; }
        public DbSet<LevelTest> LevelTests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
    }
}
