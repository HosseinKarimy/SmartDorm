using Microsoft.EntityFrameworkCore;
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
