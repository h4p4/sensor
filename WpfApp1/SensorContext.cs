namespace WpfApp1
{
    using Microsoft.EntityFrameworkCore;

    public partial class SensorContext : DbContext
    {
        public SensorContext()
        {
            Database.EnsureCreated();
        }

        public SensorContext(DbContextOptions<SensorContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var settings = ConnectionSettings.Instance;

            if (settings == null)
                return;

            optionsBuilder.UseNpgsql(
                $"Host={settings.Host};" +
                $"Port={settings.Port};" +
                $"Database={settings.DatabaseName};" +
                $"Username={settings.DatabaseUserName};" +
                $"Password={settings.Password};" +
                $"Include Error Detail=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.ToTable("sensors", "sensor");

                entity.Property(e => e.SensorData)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.UserCreator)
                    .WithMany(p => p.Sensors)
                    .HasForeignKey(d => d.UserCreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sensors_UserCreatorId_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "sensor");

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

                entity.Property(e => e.UserDisplayname)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("user_displayname");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("user_password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}