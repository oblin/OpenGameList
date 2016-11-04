
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OpenGameList.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Comment> Comments { get; set; }
        // ApplicationUser had been defined in IdentityDbContext
        // public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 定義 ApplicationUser 對應到資料庫的名稱
            modelBuilder.Entity<ApplicationUser>().ToTable("users");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Id).HasColumnName("id");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.UserName).HasColumnName("user_name");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.NormalizedUserName).HasColumnName("normalized_user_name");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.PasswordHash).HasColumnName("password_hash");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.PhoneNumber).HasColumnName("phone_number");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.DisplayName).HasColumnName("display_name");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Notes).HasColumnName("notes");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Type).HasColumnName("type");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Flags).HasColumnName("flags");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.CreatedDate).HasColumnName("created_date");
            modelBuilder.Entity<ApplicationUser>().Property(u => u.LastModifiedDate).HasColumnName("last_modified_date");
            // 定義 AspNetRoles 對應到資料庫的名稱
            modelBuilder.Entity<IdentityRole>().ToTable("roles");
            modelBuilder.Entity<IdentityRole>().Property(u => u.Id).HasColumnName("id");
            modelBuilder.Entity<IdentityRole>().Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            modelBuilder.Entity<IdentityRole>().Property(u => u.Name).HasColumnName("name");
            modelBuilder.Entity<IdentityRole>().Property(u => u.NormalizedName).HasColumnName("normalized_name");
            // 定義 AspNetUserRoles 對應到資料庫的名稱
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityUserRole<string>>().Property(u => u.UserId).HasColumnName("user_id");
            modelBuilder.Entity<IdentityUserRole<string>>().Property(u => u.RoleId).HasColumnName("role_id");

            // 定義 relations: foreign keys
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Items).WithOne(i => i.Author);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments).WithOne(i => i.Author).HasPrincipalKey(u => u.Id);

            // 定義 Item 對應到資料庫的名稱
            modelBuilder.Entity<Item>()
                .ToTable("items")
                .Property(i => i.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Item>().Property(i => i.Id).HasColumnName("id");
            modelBuilder.Entity<Item>().Property(i => i.Title).HasColumnName("title");
            modelBuilder.Entity<Item>().Property(i => i.Description).HasColumnName("description");
            modelBuilder.Entity<Item>().Property(i => i.Text).HasColumnName("text");
            modelBuilder.Entity<Item>().Property(i => i.Notes).HasColumnName("notes");
            modelBuilder.Entity<Item>().Property(i => i.Type).HasColumnName("type");
            modelBuilder.Entity<Item>().Property(i => i.Flags).HasColumnName("flags");
            modelBuilder.Entity<Item>().Property(i => i.UserId).HasColumnName("user_id");
            modelBuilder.Entity<Item>().Property(i => i.ViewCount).HasColumnName("view_count");
            modelBuilder.Entity<Item>().Property(u => u.CreatedDate).HasColumnName("created_date");
            modelBuilder.Entity<Item>().Property(u => u.LastModifiedDate).HasColumnName("last_modified_date");
            // 定義 relations: foreign keys
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Author).WithMany(u => u.Items);
            modelBuilder.Entity<Item>().HasMany(i => i.Comments).WithOne(c => c.Item);

            // 定義 Comment 對應到資料庫的名稱
            modelBuilder.Entity<Comment>().ToTable("comments");
            modelBuilder.Entity<Comment>().Property(c => c.Id).HasColumnName("id");
            modelBuilder.Entity<Comment>().Property(c => c.ItemId).HasColumnName("item_id");
            modelBuilder.Entity<Comment>().Property(c => c.Text).HasColumnName("text");
            modelBuilder.Entity<Comment>().Property(i => i.Type).HasColumnName("type");
            modelBuilder.Entity<Comment>().Property(i => i.Flags).HasColumnName("flags");
            modelBuilder.Entity<Comment>().Property(i => i.UserId).HasColumnName("user_id");
            modelBuilder.Entity<Comment>().Property(u => u.LastModifiedDate).HasColumnName("last_modified_date");
            modelBuilder.Entity<Comment>().Property(u => u.CreatedDate).HasColumnName("created_date");
            // 定義 relations: foreign keys
            modelBuilder.Entity<Comment>().HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasOne(c => c.Item).WithMany(i => i.Comments);
            modelBuilder.Entity<Comment>().HasOne(c => c.Parent).WithMany(c => c.Children);
            modelBuilder.Entity<Comment>().HasMany(c => c.Children).WithOne(c => c.Parent);
        }
    }
}