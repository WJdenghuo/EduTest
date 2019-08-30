using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Edu.Entity.MySqlEntity
{
    public partial class BaseEduContext : DbContext
    {
        public BaseEduContext()
        {
        }

        public BaseEduContext(DbContextOptions<BaseEduContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LogInfo> LogInfo { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<PhoneUserCode> PhoneUserCode { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Meet> Meet { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySql("server=127.0.0.1;uid=root;pwd=cnki_123;database=BaseEdu");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogInfo>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Name).HasColumnType("varchar(255)");

                entity.Property(e => e.OpType).HasColumnType("int(11)");

                entity.Property(e => e.Remark).HasColumnType("text");

                entity.Property(e => e.TableName).HasColumnType("varchar(255)");

                entity.Property(e => e.Url).HasColumnType("varchar(255)");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FuncDes)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FuncId)
                    .IsRequired()
                    .HasColumnName("FuncID")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FuncLevel).HasColumnType("int(11)");

                entity.Property(e => e.FuncName)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.FuncType)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OrderNo).HasColumnType("int(11)");

                entity.Property(e => e.ParentId)
                    .IsRequired()
                    .HasColumnName("ParentID")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RoleIds)
                    .HasColumnName("RoleIDs")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.States).HasColumnType("int(11)");

                entity.Property(e => e.StatesDate).HasColumnType("datetime");

                entity.Property(e => e.SysLogo)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Target)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'_parent'");

                entity.Property(e => e.TargetUrl)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<PhoneUserCode>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code).HasColumnType("varchar(10)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Bz)
                    .HasColumnName("BZ")
                    .HasColumnType("varchar(2000)");

                entity.Property(e => e.Company).HasColumnType("varchar(255)");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateUser).HasColumnType("int(11)");

                entity.Property(e => e.Email).HasColumnType("varchar(255)");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Ipadress)
                    .HasColumnName("IPAdress")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OId)
                    .HasColumnName("oID")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Phone).HasColumnType("varchar(255)");

                entity.Property(e => e.Photo).HasColumnType("varchar(255)");

                entity.Property(e => e.Pwd).HasColumnType("varchar(255)");

                entity.Property(e => e.Qq)
                    .HasColumnName("QQ")
                    .HasColumnType("varchar(3)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.States).HasColumnType("int(11)");

                entity.Property(e => e.StatesDate).HasColumnType("datetime");

                entity.Property(e => e.TrueName).HasColumnType("varchar(255)");

                entity.Property(e => e.UserName).HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code).HasColumnType("varchar(255)");

                entity.Property(e => e.IsAdmin).HasColumnType("varchar(255)");

                entity.Property(e => e.Name).HasColumnType("varchar(255)");

                entity.Property(e => e.States).HasColumnType("int(11)");

                entity.Property(e => e.StatesDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Callsite).HasColumnType("varchar(255)");

                entity.Property(e => e.Exception).HasColumnType("varchar(255)");

                entity.Property(e => e.Level).HasColumnType("varchar(50)");

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasColumnType("varchar(250)");

                entity.Property(e => e.MachineName).HasColumnType("varchar(50)");

                entity.Property(e => e.Message).HasColumnType("varchar(255)");
            });
        }
    }
}
