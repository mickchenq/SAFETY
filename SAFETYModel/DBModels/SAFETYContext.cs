using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class SAFETYContext : DbContext
    {
        public SAFETYContext()
        {
        }

        public SAFETYContext(DbContextOptions<SAFETYContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DataCenter> DataCenter { get; set; }
        public virtual DbSet<House> House { get; set; }
        public virtual DbSet<HouseType> HouseType { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Layer> Layer { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LocationChangeOrder> LocationChangeOrder { get; set; }
        public virtual DbSet<NotificationDetail> NotificationDetail { get; set; }
        public virtual DbSet<NotificationOrder> NotificationOrder { get; set; }
        public virtual DbSet<ProcessDetail> ProcessDetail { get; set; }
        public virtual DbSet<ProcessOrder> ProcessOrder { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductPackage> ProductPackage { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<ReturnedDetail> ReturnedDetail { get; set; }
        public virtual DbSet<ReturnedOrder> ReturnedOrder { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<RoomType> RoomType { get; set; }
        public virtual DbSet<Shelf> Shelf { get; set; }
        public virtual DbSet<ShelfType> ShelfType { get; set; }
        public virtual DbSet<ShippingDetail> ShippingDetail { get; set; }
        public virtual DbSet<ShippingOrder> ShippingOrder { get; set; }
        public virtual DbSet<StockAdjustment> StockAdjustment { get; set; }
        public virtual DbSet<StockAdjustmentDetail> StockAdjustmentDetail { get; set; }
        public virtual DbSet<StockChangeOrder> StockChangeOrder { get; set; }
        public virtual DbSet<StockInDetail> StockInDetail { get; set; }
        public virtual DbSet<StockInLocationDetail> StockInLocationDetail { get; set; }
        public virtual DbSet<StockInOrder> StockInOrder { get; set; }
        public virtual DbSet<StockOutDetail> StockOutDetail { get; set; }
        public virtual DbSet<StockOutLocationDetail> StockOutLocationDetail { get; set; }
        public virtual DbSet<StockOutOrder> StockOutOrder { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SysFunction> SysFunction { get; set; }
        public virtual DbSet<SysParam> SysParam { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<TempLayer> TempLayer { get; set; }
        public virtual DbSet<UploadFiles> UploadFiles { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserRoleFunction> UserRoleFunction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-VVH8L2CB\\MSSQL2019;Initial Catalog=SAFETY;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasComment("儲區資料");

                entity.Property(e => e.AreaId).HasComment("儲區id");

                entity.Property(e => e.AreaCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("儲區代碼");

                entity.Property(e => e.AreaName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("儲區名稱");

                entity.Property(e => e.AreaType).HasComment("儲區型態");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateId).HasComment("建立者id");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否停用 | Y:停用 N:不停用");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasComment("修改時間");

                entity.Property(e => e.ModifyId).HasComment("修改者id");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')")
                    .HasComment("備註");

                entity.Property(e => e.RoomId).HasComment("庫別id");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasComment("客戶基本資料");

                entity.Property(e => e.CustomerId).HasComment("客戶id");

                entity.Property(e => e.Addr)
                    .HasMaxLength(100)
                    .HasComment("地址");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .HasComment("城市");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(10)
                    .HasComment("國家");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateId).HasComment("建立者id");

                entity.Property(e => e.CustomerCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("客戶代碼");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("客戶名稱");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否停用 | Y:停用 N:不停用");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasComment("修改時間");

                entity.Property(e => e.ModifyId).HasComment("修改者id");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasComment("電話");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')")
                    .HasComment("備註");

                entity.Property(e => e.VatNo)
                    .HasMaxLength(20)
                    .HasComment("統一編號");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .HasComment("郵遞區號");
            });

            modelBuilder.Entity<DataCenter>(entity =>
            {
                entity.HasKey(e => e.DcId)
                    .HasName("PK__DataCent__7D999EDBCA327E6D");

                entity.HasComment("物流中心");

                entity.Property(e => e.DcId).HasComment("物流中心id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateId).HasComment("建立者id");

                entity.Property(e => e.DcCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("物流中心代碼");

                entity.Property(e => e.DcName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("物流中心名稱");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否停用 | Y:停用 N:不停用");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasComment("修改時間");

                entity.Property(e => e.ModifyId).HasComment("修改者id");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')")
                    .HasComment("備註");
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.HasComment("倉別資料");

                entity.Property(e => e.HouseId).HasComment("倉別id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateId).HasComment("建立者id");

                entity.Property(e => e.DcId).HasComment("物流中心id");

                entity.Property(e => e.HouseCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("倉別代碼");

                entity.Property(e => e.HouseName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("倉別名稱");

                entity.Property(e => e.HouseTypeId).HasComment("倉別類型id");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')")
                    .HasComment("是否停用 | Y:停用 N:不停用");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasComment("修改時間");

                entity.Property(e => e.ModifyId).HasComment("修改者id");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')")
                    .HasComment("備註");
            });

            modelBuilder.Entity<HouseType>(entity =>
            {
                entity.HasComment("倉別類型資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HouseTypeCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.HouseTypeName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.Property(e => e.InventoryId).HasComment("出入庫流水號");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.InvDate).HasColumnType("date");

                entity.Property(e => e.InventoryKind)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("類別 | I:入庫 O:出庫 A:盤點調整 S:庫存調整");

                entity.Property(e => e.InventoryStatus).HasComment("狀態 | 1:待上架 2:已上架 3:待揀貨 4:已揀貨");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.HasComment("層架資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Depth).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Height).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.LayerCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasComment("儲位資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsMixable)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsStackable)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.LocationCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.MedianX).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.MedianY).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Square).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.TagAddr).HasComment("Tag機器ID");

                entity.Property(e => e.TagGateWay).HasComment("儲位GateWay");

                entity.Property(e => e.Weight).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Width).HasColumnType("decimal(9, 2)");
            });

            modelBuilder.Entity<LocationChangeOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tmp_ms_x__C3905BCFD2A507ED");

                entity.Property(e => e.ChangeReason)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<NotificationDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK__Notifica__135C316DB63978AA");

                entity.HasComment("進貨通知單明細");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.MakeDate).HasColumnType("date");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Unit).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<NotificationOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tmp_ms_x__C3905BCFCD1B2FDB");

                entity.HasComment("進貨通知單");

                entity.Property(e => e.AcceptDate)
                    .HasColumnType("date")
                    .HasComment("進貨驗收日期");

                entity.Property(e => e.AcceptId).HasComment("驗收人員");

                entity.Property(e => e.AcceptRemarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')")
                    .HasComment("驗收備註");

                entity.Property(e => e.ActualReceiveDate).HasColumnType("date");

                entity.Property(e => e.ContactAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerOrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EstimatedReceiveDate).HasColumnType("date");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ProcessDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK__ProcessD__135C316DFB027F20");

                entity.HasComment("加工通知單明細");
            });

            modelBuilder.Entity<ProcessOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__ProcessO__C3905BCF03052215");

                entity.HasComment("加工通知單");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EstimatedProcessingDate).HasColumnType("date");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasComment("客戶商品資料");

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__ProductC__19093A0B494C2072");

                entity.HasComment("產品分類");

                entity.Property(e => e.CategoryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ProductPackage>(entity =>
            {
                entity.HasKey(e => e.PackageId)
                    .HasName("PK__ProductP__322035CC9C9C389A");

                entity.HasComment("商品包裝資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Height).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.IsMinSku)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Length).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.PackageName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Weight).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Width).HasColumnType("decimal(9, 2)");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__ProductT__516F03B5EB8E9A2D");

                entity.HasComment("產品類型");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ReturnedDetail>(entity =>
            {
                entity.HasComment("退貨通知單明細");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.MakeDate).HasColumnType("date");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ReturnedOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tmp_ms_x__C3905BCF0415B442");

                entity.HasComment("退貨通知單");

                entity.Property(e => e.AcceptDate).HasColumnType("date");

                entity.Property(e => e.AcceptRemarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerOrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EstimatedReceiveDate).HasColumnType("date");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReturnedDate).HasColumnType("date");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasComment("庫別資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DcId).HasComment("物流中心id");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RoomCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.RoomName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.HasComment("庫別類別資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RoomTypeCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.RoomTypeName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Shelf>(entity =>
            {
                entity.HasComment("貨架資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DownAisleWidth).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.LeftAisleWidth).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Length).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RightAisleWidth).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ShelfCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UpAisleWidth).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Width).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.X1).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.X2).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Y1).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Y2).HasColumnType("decimal(9, 2)");
            });

            modelBuilder.Entity<ShelfType>(entity =>
            {
                entity.HasComment("貨架類型資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsFlat)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ShelfTypeCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ShelfTypeName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ShippingDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK__tmp_ms_x__135C316D937BCC65");

                entity.HasComment("出貨通知單明細");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Unit).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ShippingOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Shipping__C3905BCF2B403168");

                entity.HasComment("出貨通知單");

                entity.Property(e => e.ContactAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EstimatedReceiveDate).HasColumnType("date");

                entity.Property(e => e.EstimatedShippingDate).HasColumnType("date");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ZipCode).HasMaxLength(10);
            });

            modelBuilder.Entity<StockAdjustment>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__StockAdj__C3905BCF69019BF5");

                entity.Property(e => e.OrderId).HasComment("盤點通知單");

                entity.Property(e => e.AdjustStatus)
                    .HasDefaultValueSql("('1')")
                    .HasComment("盤點狀態 | 1:盤點通知  2.盤點中 3:盤點完成");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<StockAdjustmentDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK__StockAdj__D3B9D36C0B13162B");

                entity.HasComment("盤點通知單明細");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<StockChangeOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__StockCha__C3905BCF707C10BB");

                entity.Property(e => e.OrderId).HasComment("庫存調整單");

                entity.Property(e => e.ChangeReason)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ChangeStatus)
                    .HasDefaultValueSql("('1')")
                    .HasComment("調整狀態 | 1:調整通知  2.調整中 3:調整完成");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<StockInDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK__StockInD__D3B9D36CC8641F14");

                entity.Property(e => e.OrderDetailId).HasComment("入庫明細id");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.MakeDate).HasColumnType("date");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Unit).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<StockInLocationDetail>(entity =>
            {
                entity.HasKey(e => e.LocationDetailId)
                    .HasName("PK__StockInL__632D9B0EA5F2FB88");

                entity.HasComment("入庫單上架資料");

                entity.Property(e => e.DetailStatus)
                    .HasDefaultValueSql("('1')")
                    .HasComment("狀態 | 1:待上架 2:上架中 3:已上架");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Unit).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<StockInOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tmp_ms_x__C3905BCF0B40A4FD");

                entity.Property(e => e.OrderId).HasComment("入庫單id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OrderType).HasDefaultValueSql("((1))");

                entity.Property(e => e.StockStatus)
                    .HasDefaultValueSql("('1')")
                    .HasComment("狀態 | 1:待入庫  2.入庫中 3:已入庫");
            });

            modelBuilder.Entity<StockOutDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK__StockOut__D3B9D36CE945479D");

                entity.HasComment("出庫單明細");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.ProductLotNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Unit).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<StockOutLocationDetail>(entity =>
            {
                entity.HasKey(e => e.LocationDetailId)
                    .HasName("PK__StockOut__632D9B0E3F0CDECC");

                entity.HasComment("出庫單下架資料");

                entity.Property(e => e.DetailStatus)
                    .HasDefaultValueSql("('1')")
                    .HasComment("狀態 | 1:待揀貨 2:揀貨中 3:已揀貨");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Unit).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<StockOutOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tmp_ms_x__C3905BCFEB38710E");

                entity.Property(e => e.OrderId).HasComment("出庫單id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StockStatus)
                    .HasDefaultValueSql("('1')")
                    .HasComment("狀態 | 1:待出庫  2.出庫中 3:已出庫");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasComment("供應商基本資料");

                entity.Property(e => e.Addr).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(20);

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SupplierCode).HasMaxLength(10);

                entity.Property(e => e.SupplierName).HasMaxLength(20);

                entity.Property(e => e.VatNo).HasMaxLength(20);

                entity.Property(e => e.ZipCode).HasMaxLength(10);
            });

            modelBuilder.Entity<SysFunction>(entity =>
            {
                entity.HasKey(e => e.FunctionId);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FnArea)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FnClass)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FnController)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FnGroup)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FnGroupName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FnPageName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FunctionName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<SysParam>(entity =>
            {
                entity.HasComment("參數設定資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataValue)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.SysParamCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.SysParamNameCh)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.SysParamNameEn)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__SysUser__1788CC4CB3AE4303");

                entity.HasComment("使用者帳號");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LoginPwd)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TempLayer>(entity =>
            {
                entity.HasKey(e => e.TempId)
                    .HasName("PK__TempLaye__06C703C19D37C7D0");

                entity.HasComment("溫層資料");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.MaxTemp).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.MinTemp).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TempCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TempName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<UploadFiles>(entity =>
            {
                entity.HasKey(e => e.UploadId)
                    .HasName("PK__UploadFi__6D16C84DF495FC00");

                entity.HasComment("上傳資料");

                entity.Property(e => e.UploadId).HasComment("上傳檔案id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateId).HasComment("建立者id");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("檔案名稱");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("檔案路徑");

                entity.Property(e => e.FormId).HasComment("表單Id");

                entity.Property(e => e.FormKind).HasComment("表單類別");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasComment("修改時間");

                entity.Property(e => e.ModifyId).HasComment("修改者id");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsStop)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserRoleName)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<UserRoleFunction>(entity =>
            {
                entity.HasKey(e => new { e.FunctionId, e.UserRoleId })
                    .HasName("PK__tmp_ms_x__7272825BB2125225");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public async Task<KeyValuePair<bool, string>> NewSaveChangesAsync()
        {
            var dic = new Dictionary<bool, string>();
            try
            {
                //var res = base.SaveChanges();
                var res = await base.SaveChangesAsync();
                dic.Add(true, "");
                return dic.FirstOrDefault();
            }
            catch (Exception ex)
            {
                dic.Add(false, ex.Message + ex.InnerException?.Message);
                return dic.FirstOrDefault();
            }
        }
    }
}
