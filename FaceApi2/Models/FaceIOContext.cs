using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FaceApi2.Models
{
    public partial class FaceIOContext : DbContext
    {
        public FaceIOContext()
        {
        }

        public FaceIOContext(DbContextOptions<FaceIOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<ClassSubject> ClassSubject { get; set; }
        public virtual DbSet<ClassSubjectSchedule> ClassSubjectSchedule { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentStudy> StudentStudy { get; set; }
        public virtual DbSet<StudentStudyAttendance> StudentStudyAttendance { get; set; }
        public virtual DbSet<StudentTeacherTicket> StudentTeacherTicket { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<TeacherTeach> TeacherTeach { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-LVMGB08\\SQLEXPRESS;Initial Catalog=FaceIO;User ID=sa;Password=sa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ClassSubject>(entity =>
            {
                entity.ToTable("Class_Subject");

                entity.Property(e => e.ClassId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassSubject)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Sub__Class__5DCAEF64");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.ClassSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Sub__Subje__5FB337D6");
            });

            modelBuilder.Entity<ClassSubjectSchedule>(entity =>
            {
                entity.ToTable("Class_Subject_Schedule");

                entity.Property(e => e.ClassSubjectId).HasColumnName("Class_Subject_Id");

                entity.HasOne(d => d.ClassSubject)
                    .WithMany(p => p.ClassSubjectSchedule)
                    .HasForeignKey(d => d.ClassSubjectId)
                    .HasConstraintName("FK__Class_Sub__Class__14270015");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.ClassSubjectSchedule)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__Class_Sub__Sched__04E4BC85");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Lpgid)
                    .HasColumnName("LPGId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student__Id__52593CB8");
            });

            modelBuilder.Entity<StudentStudy>(entity =>
            {
                entity.ToTable("Student_Study");

                entity.Property(e => e.ClassSubjectId).HasColumnName("Class_SubjectId");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClassSubject)
                    .WithMany(p => p.StudentStudy)
                    .HasForeignKey(d => d.ClassSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_S__Class__17F790F9");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentStudy)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_S__Stude__17036CC0");
            });

            modelBuilder.Entity<StudentStudyAttendance>(entity =>
            {
                entity.ToTable("Student_Study_Attendance");

                entity.HasOne(d => d.ClassSubjectSchedule)
                    .WithMany(p => p.StudentStudyAttendance)
                    .HasForeignKey(d => d.ClassSubjectScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_S__Class__1EA48E88");

                entity.HasOne(d => d.StudentStudy)
                    .WithMany(p => p.StudentStudyAttendance)
                    .HasForeignKey(d => d.StudentStudyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_S__Stude__1DB06A4F");
            });

            modelBuilder.Entity<StudentTeacherTicket>(entity =>
            {
                entity.ToTable("Student_Teacher_Ticket");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentTeacherTicket)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_T__Stude__2BFE89A6");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.StudentTeacherTicket)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_T__Teach__2B0A656D");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.StudentTeacherTicket)
                    .HasForeignKey(d => d.TicketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Student_T__Ticke__2CF2ADDF");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Pgid)
                    .HasColumnName("PGId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Teacher)
                    .HasForeignKey<Teacher>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Teacher__Id__4E88ABD4");
            });

            modelBuilder.Entity<TeacherTeach>(entity =>
            {
                entity.ToTable("Teacher_Teach");

                entity.Property(e => e.TeacherId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClassSubject)
                    .WithMany(p => p.TeacherTeach)
                    .HasForeignKey(d => d.ClassSubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Teacher_T__Class__6FE99F9F");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherTeach)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Teacher_T__Teach__70DDC3D8");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__tmp_ms_x__536C85E52CA5ADB5");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
