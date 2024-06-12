﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QuotationSystem.Data.Models;

#nullable disable

namespace QuotationSystem.Data
{
    public partial class QuotationContext : DbContext
    {
        public QuotationContext()
        {
        }

        public QuotationContext(DbContextOptions<QuotationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CConfig> CConfigs { get; set; }
        public virtual DbSet<MDepartment> MDepartments { get; set; }
        public virtual DbSet<MItem> MItems { get; set; }
        public virtual DbSet<MLocation> MLocations { get; set; }
        public virtual DbSet<MMenu> MMenus { get; set; }
        public virtual DbSet<MUnit> MUnits { get; set; }
        public virtual DbSet<MUser> MUsers { get; set; }
        public virtual DbSet<MUserPermission> MUserPermissions { get; set; }
        public virtual DbSet<MWh> MWhs { get; set; }
        public virtual DbSet<TQuotationDetail> TQuotationDetails { get; set; }
        public virtual DbSet<TQuotationHeader> TQuotationHeaders { get; set; }
        public virtual DbSet<TRunningNo> TRunningNos { get; set; }
        public virtual DbSet<TStock> TStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CConfig>(entity =>
            {
                entity.HasKey(e => e.ConfCode);

                entity.ToTable("c_config");

                entity.Property(e => e.ConfCode)
                    .HasMaxLength(30)
                    .HasColumnName("conf_code");

                entity.Property(e => e.ConfDescription)
                    .HasMaxLength(150)
                    .HasColumnName("conf_description");

                entity.Property(e => e.ConfName)
                    .HasMaxLength(30)
                    .HasColumnName("conf_name");

                entity.Property(e => e.ConfValue)
                    .IsUnicode(false)
                    .HasColumnName("conf_value");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.ToTable("m_department");

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(30)
                    .HasColumnName("department_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DepartmentDesc)
                    .HasMaxLength(150)
                    .HasColumnName("department_desc");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("department_name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MItem>(entity =>
            {
                entity.HasKey(e => e.ItemCode);

                entity.ToTable("m_item");

                entity.Property(e => e.ItemCode)
                    .HasMaxLength(30)
                    .HasColumnName("item_code");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemDesc)
                    .HasMaxLength(30)
                    .HasColumnName("item_desc");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("item_name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UnitId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("unit_id");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.MItems)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_m_item_m_unit");
            });

            modelBuilder.Entity<MLocation>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("m_location");

                entity.Property(e => e.LocationId)
                    .HasMaxLength(30)
                    .HasColumnName("location_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("location_name");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MMenu>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.ToTable("m_menu");

                entity.Property(e => e.MenuId)
                    .HasMaxLength(30)
                    .HasColumnName("menu_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(150)
                    .HasColumnName("menu_name");

                entity.Property(e => e.ParentMenu)
                    .HasMaxLength(10)
                    .HasColumnName("parent_menu");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.ToTable("m_unit");

                entity.Property(e => e.UnitId)
                    .HasMaxLength(30)
                    .HasColumnName("unit_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UnitDesc)
                    .HasMaxLength(150)
                    .HasColumnName("unit_desc");

                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("unit_name");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("m_user");

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .HasColumnName("user_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.ChangePassword)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("change_password")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(30)
                    .HasColumnName("department_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("password")
                    .HasDefaultValueSql("(N'default')");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.MUsers)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_m_user_m_department");
            });

            modelBuilder.Entity<MUserPermission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.MenuId });

                entity.ToTable("m_user_permission");

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .HasColumnName("user_id");

                entity.Property(e => e.MenuId)
                    .HasMaxLength(30)
                    .HasColumnName("menu_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MUserPermissions)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_m_user_permission_m_menu");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MUserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_m_user_permission_m_user");
            });

            modelBuilder.Entity<MWh>(entity =>
            {
                entity.HasKey(e => e.WhId);

                entity.ToTable("m_wh");

                entity.Property(e => e.WhId)
                    .HasMaxLength(30)
                    .HasColumnName("wh_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WhName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("wh_name");
            });

            modelBuilder.Entity<TQuotationDetail>(entity =>
            {
                entity.HasKey(e => new { e.QuotationNo, e.ItemCode });

                entity.ToTable("t_quotation_detail");

                entity.Property(e => e.QuotationNo)
                    .HasMaxLength(30)
                    .HasColumnName("quotation_no");

                entity.Property(e => e.ItemCode)
                    .HasMaxLength(30)
                    .HasColumnName("item_code");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");

                entity.Property(e => e.ItemQty).HasColumnName("item_qty");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ItemCodeNavigation)
                    .WithMany(p => p.TQuotationDetails)
                    .HasForeignKey(d => d.ItemCode)
                    .HasConstraintName("FK_t_quotation_detail_m_item");

                entity.HasOne(d => d.QuotationNoNavigation)
                    .WithMany(p => p.TQuotationDetails)
                    .HasForeignKey(d => d.QuotationNo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_t_quotation_detail_t_quotation_header");
            });

            modelBuilder.Entity<TQuotationHeader>(entity =>
            {
                entity.HasKey(e => e.QuotationNo);

                entity.ToTable("t_quotation_header");

                entity.Property(e => e.QuotationNo)
                    .HasMaxLength(30)
                    .HasColumnName("quotation_no");

                entity.Property(e => e.ActiveStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerAddress)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("customer_address");

                entity.Property(e => e.CustomerContact)
                    .HasMaxLength(150)
                    .HasColumnName("customer_contact");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("customer_name");

                entity.Property(e => e.GrandTotal).HasColumnName("grand_total");

                entity.Property(e => e.QuotationDate)
                    .HasColumnType("date")
                    .HasColumnName("quotation_date");

                entity.Property(e => e.Remark)
                    .HasMaxLength(150)
                    .HasColumnName("remark");

                entity.Property(e => e.Seller)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("seller");

                entity.Property(e => e.TaxId)
                    .HasMaxLength(150)
                    .HasColumnName("tax_id");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Vat).HasColumnName("vat");
            });

            modelBuilder.Entity<TRunningNo>(entity =>
            {
                entity.HasKey(e => new { e.TypeNo, e.RunningDate });

                entity.ToTable("t_running_no");

                entity.Property(e => e.TypeNo)
                    .HasMaxLength(30)
                    .HasColumnName("type_no");

                entity.Property(e => e.RunningDate)
                    .HasMaxLength(8)
                    .HasColumnName("running_date");

                entity.Property(e => e.RunningNo)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("running_no");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(30)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<TStock>(entity =>
            {
                entity.HasKey(e => e.LabelId);

                entity.ToTable("t_stock");

                entity.Property(e => e.LabelId)
                    .HasMaxLength(30)
                    .HasColumnName("label_id");

                entity.Property(e => e.ActiveStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("active_status")
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.CargoStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("cargo_status")
                    .HasDefaultValueSql("('S')")
                    .IsFixedLength(true);

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("item_code");

                entity.Property(e => e.LocationId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("location_id");

                entity.Property(e => e.LotNo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("lot_no");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.StockInDate)
                    .HasColumnType("datetime")
                    .HasColumnName("stock_in_date");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(30)
                    .HasColumnName("update_by");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WhId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("wh_id");

                entity.HasOne(d => d.ItemCodeNavigation)
                    .WithMany(p => p.TStocks)
                    .HasForeignKey(d => d.ItemCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_stock_m_item");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TStocks)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_stock_m_location");

                entity.HasOne(d => d.Wh)
                    .WithMany(p => p.TStocks)
                    .HasForeignKey(d => d.WhId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_t_stock_m_wh");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
