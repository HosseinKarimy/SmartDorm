using Microsoft.EntityFrameworkCore;
using SmartDorm.Enums;
using SmartDorm.Models;

namespace SmartDorm.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Dormitory> Dormitories => Set<Dormitory>();
        public DbSet<DormitoryManager> DormitoryManagers => Set<DormitoryManager>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
        public DbSet<StudentPreference> StudentPreferences => Set<StudentPreference>();
        public DbSet<SpecialCondition> SpecialConditions => Set<SpecialCondition>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetRelations(modelBuilder);
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // -----------------------------------------
            // PASSWORD HASH FOR ADMIN ("Admin123!")
            // -----------------------------------------
            using var sha = System.Security.Cryptography.SHA256.Create();
            var adminPasswordHash = Convert.ToBase64String(
                sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Admin123!"))
            );

            // -----------------------------------------
            // SEED: USER ACCOUNTS
            // -----------------------------------------
            modelBuilder.Entity<UserAccount>().HasData(
                new UserAccount
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = adminPasswordHash,
                    Role = UserRole.Admin,
                    IsActive = true
                },
                new UserAccount
                {
                    Id = 2,
                    Username = "dormmanager",
                    PasswordHash = adminPasswordHash,
                    Role = UserRole.DormitoryManager,
                    IsActive = true
                }
            );

            // -----------------------------------------
            // SEED: DORMITORIES
            // -----------------------------------------
            modelBuilder.Entity<Dormitory>().HasData(
                new Dormitory
                {
                    DormitoryId = 1,
                    Name = "Sadra",
                    Address = "Campus North Zone",
                    CampusLocation = "North",
                    TotalCapacity = 300,
                    GenderType = GenderType.Male,
                    ManagerId = 1 // reference DormitoryManager.Id
                },
                new Dormitory
                {
                    DormitoryId = 2,
                    Name = "Sadaf",
                    Address = "Campus West Zone",
                    CampusLocation = "West",
                    TotalCapacity = 250,
                    GenderType = GenderType.Female,
                    ManagerId = 1
                }
            );

            // -----------------------------------------
            // SEED: DORMITORY MANAGER
            // -----------------------------------------
            modelBuilder.Entity<DormitoryManager>().HasData(
                new DormitoryManager
                {
                    Id = 1,
                    UserId = 2,          // maps to dormmanager user
                    FullName = "mosayebi",
                    PhoneNumber = "555-1234",
                }
            );

            // -----------------------------------------
            // SEED: ROOMS
            // -----------------------------------------
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = 1,
                    DormitoryId = 1,
                    RoomNumber = 336,
                    Floor = 1,
                    WingOrBlock = 3,
                    Capacity = 2,
                    HasPrivateBathroom = false,
                    GenderType = GenderType.Male,
                    IsActive = true
                },
                new Room
                {
                    RoomId = 2,
                    DormitoryId = 1,
                    RoomNumber = 212,
                    Floor = 1,
                    WingOrBlock = 1,
                    Capacity = 3,
                    HasPrivateBathroom = true,
                    GenderType = GenderType.Male,
                    IsActive = true
                },
                new Room
                {
                    RoomId = 3,
                    DormitoryId = 2,
                    RoomNumber = 5,
                    Floor = 2,
                    WingOrBlock = 3,
                    Capacity = 2,
                    HasPrivateBathroom = true,
                    GenderType = GenderType.Female,
                    IsActive = true
                }
            );
        }

        private static void SetRelations(ModelBuilder modelBuilder)
        {
            // 1-to-1 Student <-> StudentPreference
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Preference)
                .WithOne(p => p.Student)
                .HasForeignKey<StudentPreference>(p => p.StudentId);

            // 1-to-many Student <-> SpecialConditions
            modelBuilder.Entity<Student>()
                .HasMany(s => s.SpecialConditions)
                .WithOne(c => c.Student)
                .HasForeignKey(c => c.StudentId);

            // 1-to-1 UserAccount <-> Student
            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.Student)
                .WithOne(s => s.UserAccount)
                .HasForeignKey<Student>(s => s.UserId);

            // 1-to-1 UserAccount <-> DormitoryManager
            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.DormitoryManager)
                .WithOne(m => m.UserAccount)
                .HasForeignKey<DormitoryManager>(m => m.UserId);

            // DormitoryManager – Dormitory (1-many)
            modelBuilder.Entity<Dormitory>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId);

            // Dormitory – Room (1-many)
            modelBuilder.Entity<Dormitory>()
                .HasMany(d => d.Rooms)
                .WithOne(r => r.Dormitory)
                .HasForeignKey(r => r.DormitoryId);
        }
    }
}
