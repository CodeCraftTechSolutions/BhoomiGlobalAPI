using BhoomiGlobalAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BhoomiGlobalAPI
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<MenuCategory> MenuCategory { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<PageImage> PageImages { get; set; }
        public DbSet<PageCategory> PageCategory { get; set; }
        public DbSet<PageSection> PageSection { get; set; }
        public DbSet<PageSectionDetails> PageSectionDetails { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<WebSettings> WebSettings { get; set; }
        public DbSet<Contact> Contact { get; set; }


        //Email
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<EmailQueue> EmailQueue { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }

        //Newsletter
        public DbSet<NewsletterSubscriber> NewsletterSubscriber { get; set; }
        public DbSet<Newsletter> Newsletter { get; set; }

        //carousels
        public DbSet<Carousel> Carousels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }

    

}
