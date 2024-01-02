using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DAL.Models
{
    public partial class motorentoContext : DbContext
    {
        public motorentoContext()
        {
        }

        public motorentoContext(DbContextOptions<motorentoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-K5GNBFH;Initial Catalog=motorento;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.BranchName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("branchName");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(8, 6)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("longitude");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasIndex(e => e.LicensePlateNumber, "UQ__Cars__599406EE904F62E2")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Branch).HasColumnName("branch");

                entity.Property(e => e.InWorkingOrder).HasColumnName("inWorkingOrder");

                entity.Property(e => e.LicensePlateNumber)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("licensePlateNumber");

                entity.Property(e => e.Mileage)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("mileage");

                entity.Property(e => e.ModelId).HasColumnName("modelId");

                entity.Property(e => e.Pic).HasColumnName("pic");

                entity.HasOne(d => d.BranchNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Branch)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cars__branch__37A5467C");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cars__modelId__36B12243");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DailyPrice)
                    .HasColumnType("numeric(19, 4)")
                    .HasColumnName("dailyPrice");

                entity.Property(e => e.DelayPricePerDay)
                    .HasColumnType("numeric(19, 4)")
                    .HasColumnName("delayPricePerDay");

                entity.Property(e => e.Gear).HasColumnName("gear");

                entity.Property(e => e.ManufactureYear)
                    .HasColumnType("date")
                    .HasColumnName("manufactureYear");

                entity.Property(e => e.Manufacturer)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("manufacturer");

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("modelName");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarId).HasColumnName("carId");

                entity.Property(e => e.DateReturned)
                    .HasColumnType("date")
                    .HasColumnName("dateReturned");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__carId__398D8EEE");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__userName__38996AB5");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164C1673CF2")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("birthDate");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Pic).HasColumnName("pic");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("role");

                entity.Property(e => e.Sex).HasColumnName("sex");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
