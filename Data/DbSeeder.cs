using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OpenIddict;
using CryptoHelper;
using Microsoft.Extensions.Configuration;

namespace OpenGameList.Data
{
    public class DbSeeder
    {
        private ApplicationDbContext _dbContext;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public DbSeeder(ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            _dbContext.Database.EnsureCreated();
            // OpenIddictApplications , needs to be populated with a single row corresponding to our web application.
            if (!_dbContext.Applications.Any()) CreateApplication();
            if (await _dbContext.Users.CountAsync() == 0) await CreateUsers();
            if (await _dbContext.Items.CountAsync() == 0) CreateItems();
        }

        private void CreateApplication()
        {
            _dbContext.Applications.Add(new OpenIddictApplication
            {
                Id = _configuration["Authentication:OpenIddict:ApplicationId"],
                DisplayName = _configuration["Authentication:OpenIddict:DisplayName"],
                RedirectUri = _configuration["Authentication:OpenIddict:TokenEndPoint"],
                LogoutRedirectUri = "/",
                ClientId = _configuration["Authentication:OpenIddict:ClientId"],
                ClientSecret = Crypto.HashPassword(_configuration["Authentication:OpenIddict:ClientSecret"]),
                Type = OpenIddictConstants.ClientTypes.Public
            });

            _dbContext.SaveChanges();
        }

        private void CreateItems()
        {
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;
            var authorId = _dbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;
#if DEBUG
            var num = 1000;
            for (int id = 1; id <= num; id++)
            {
                _dbContext.Items.Add(GetSampleItem(id, authorId, num - id, new DateTime(2015, 12, 31).AddDays(-num)));
            }
#endif
            EntityEntry<Item> e1 = _dbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Magarena",
                Description = "Single-player fantasy card game similar to Magic: The Gathering",
                Text = @"Loosely based on Magic: The Gathering, the game lets you play against a computer opponent or another human being.
                                The game features a well-developed AI, an intuitive and clear interface and an enticing level of gameplay.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 2343,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Item> e2 = _dbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Minetest",
                Description = "Open-Source alternative to Minecraft",
                Text = @"The Minetest gameplay is very similar to Minecraft's:
                        you are playing in a 3D open world, where you can create and/or remove various types of blocks.
                        Minetest feature both single-player and multi-player game modes.
                        It also has support for custom mods, additional texture packs and other custom/personalization options.
                        Minetest has been released in 2015 under GNU Lesser General Public License.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Item> e3 = _dbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Relic Hunters Zero",
                Description = "A free game about shooting evil space ducks with tiny, cute guns.",
                Text = @"Relic Hunters Zero is fast, tactical and also very smooth to play.
                        It also enables the users to look at the source
                        code, so they can can get creative and keep this game alive, fun and free for years to come.
                        The game is also available on Steam.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });
            EntityEntry<Item> e4 = _dbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "SuperTux",
                Description = "A classic 2D jump and run, side-scrolling game similar to the Super Mario series.",
                Text = @"The game is currently under Milestone 3. The Milestone2, which is currently out, features the following:
                        - a nearly completely rewritten game engine based on OpenGL, OpenAL, SDL2, ...
                        - support for translations
                        - in-game manager for downloadable add-ons and translations
                        - Bonus Island III, a for now unfinished Forest Island and the development levels in Incubator Island
                        - a final boss in Icy Island
                        - new and improved soundtracks and sound effects
                        ... and much more!
                        The game has been released under the GNU GPL license.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 9602,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });
            EntityEntry<Item> e5 = _dbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Scrabble3D",
                Description = "A 3D-based revamp to the classic Scrabble game.",
                Text = @"Scrabble3D extends the gameplay of the classic game
                        Scrabble by adding a new whole third dimension.
                        Other than playing left to right or top to bottom,
                        you'll be able to place your tiles above or beyond other tiles.
                        Since the game features more fields, it also uses a larger letter set.
                        You can either play against the computer, players from your LAN or from the Internet.
                        The game also features a set of game servers where 
                        you can challenge players from all over the world and get ranked into an
                        official, ELO-based rating/ladder system.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 6754,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            // Create default Comments (if there are none)
            if (_dbContext.Comments.Count() == 0)
            {
                int numComments = 10;   // comments per item
                for (int i = 1; i <= numComments; i++)
                    _dbContext.Comments.Add(GetSampleComment(i, e1.Entity.Id, authorId,
                    createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    _dbContext.Comments.Add(GetSampleComment(i, e2.Entity.Id, authorId,
                    createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    _dbContext.Comments.Add(GetSampleComment(i, e3.Entity.Id, authorId,
                    createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    _dbContext.Comments.Add(GetSampleComment(i, e4.Entity.Id, authorId,
                    createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    _dbContext.Comments.Add(GetSampleComment(i, e5.Entity.Id, authorId,
                    createdDate.AddDays(i)));
            }
            _dbContext.SaveChanges();
        }

        private async Task CreateUsers()
        {
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;
            const string roleAdministrators = "Administrators";
            const string roleRegistered = "Registered";

            // Create Role (if they doesn't exist yet)
            if (!await _roleManager.RoleExistsAsync(roleAdministrators))
                await _roleManager.CreateAsync(
                    new IdentityRole(roleAdministrators));
            if (!await _roleManager.RoleExistsAsync(roleRegistered))
                await _roleManager.CreateAsync(
                    new IdentityRole(roleRegistered));

            // Create the "Admin" ApplicationUser account (if it doesn't exist already)
            var user_Admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            // Insert "Admin" applicationuser with role into the Database 
            if (await _userManager.FindByIdAsync(user_Admin.Id) == null)
            {
                await _userManager.CreateAsync(user_Admin, "Pass4Admin");
                await _userManager.AddToRoleAsync(user_Admin, roleAdministrators);
                // Remove Lockout and E-mail confirmation
                user_Admin.EmailConfirmed = true;
                user_Admin.LockoutEnabled = false;
            }
#if DEBUG
            // Create some sample registered user accounts (if they don't exist already)
            var user_Ryan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Solice = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            var user_Vodan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };
            // Insert sample registered users into the Database
            if (await _userManager.FindByIdAsync(user_Ryan.Id) == null)
            {
                await _userManager.CreateAsync(user_Ryan, "Pass4Ryan");
                await _userManager.AddToRoleAsync(user_Ryan, roleRegistered);
                // Remove Lockout and E-Mail confirmation.
                user_Ryan.EmailConfirmed = true;
                user_Ryan.LockoutEnabled = false;
            }
            if (await _userManager.FindByIdAsync(user_Solice.Id) == null)
            {
                await _userManager.CreateAsync(user_Solice, "Pass4Solice");
                await _userManager.AddToRoleAsync(user_Solice, roleRegistered);
                // Remove Lockout and E-Mail confirmation.
                user_Solice.EmailConfirmed = true;
                user_Solice.LockoutEnabled = false;
            }
            if (await _userManager.FindByIdAsync(user_Vodan.Id) == null)
            {
                await _userManager.CreateAsync(user_Vodan, "Pass4Vodan");
                await _userManager.AddToRoleAsync(user_Vodan, roleRegistered);
                // Remove Lockout and E-Mail confirmation.
                user_Vodan.EmailConfirmed = true;
                user_Vodan.LockoutEnabled = false;
            }

#endif
            await _dbContext.SaveChangesAsync();
        }

        private Item GetSampleItem(int id, string authorId, int viewCount, DateTime createdDate)
        {
            return new Item()
            {
                UserId = authorId,
                Title = String.Format("Item {0} Title", id),
                Description = String.Format("This is a sample description for item {0}: Lorem ipsum dolor sit amet.", id),
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = viewCount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
        }

        private Comment GetSampleComment(int n, int itemId, string authorId, DateTime createdDate)
        {
            return new Comment()
            {
                ItemId = itemId,
                UserId = authorId,
                ParentId = null,
                Text = String.Format("Sample comment #{0} for the item #{1}", n, itemId),
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
        }
    }
}