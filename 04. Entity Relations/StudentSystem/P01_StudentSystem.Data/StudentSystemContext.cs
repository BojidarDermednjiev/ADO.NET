namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD

    using Common;
    using Models;

=======
    
    using Common;
    using Models;
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    public class StudentSystemContext : DbContext
    {

        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }
<<<<<<< HEAD

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Homework> Homeworks { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentCourse> StudentsCourses { get; set; } = null!;
=======
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Homework> Homework { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StudentCourse> StudentCourses { get; set; } = null!;
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(DbConfig.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
<<<<<<< HEAD
            modelBuilder.Entity<StudentCourse>(entity => { entity.HasKey(ps => new { ps.StudentId, ps.CourseId}); });
=======
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

>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
        }
    }
}
