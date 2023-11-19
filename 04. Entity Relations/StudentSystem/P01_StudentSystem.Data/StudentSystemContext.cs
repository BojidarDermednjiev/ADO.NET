namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    
    using Common;
    using Models;
    public class StudentSystemContext : DbContext
    {

        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Homework> Homework { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentCourse> StudentCourses { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(DbConfig.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(ps => new { ps.CourseId, ps.StudentId});
            });
            modelBuilder.Entity<Homework>(entity =>
            {
                entity
                    .HasOne(g => g.Student)
                    .WithMany(t => t.Homework)
                    .HasForeignKey(g => g.CourseId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(g => g.Course)
                    .WithMany(t => t.Homework)
                    .HasForeignKey(g => g.StudentId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

        }
    }
}
