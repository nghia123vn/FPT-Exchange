using Microsoft.EntityFrameworkCore;

namespace FPT_Exchange_Data.Entities;

public partial class FptExchangeDbContext : DbContext
{
    public FptExchangeDbContext()
    {
    }

    public FptExchangeDbContext(DbContextOptions<FptExchangeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ImageProduct> ImageProducts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductActivy> ProductActivies { get; set; }

    public virtual DbSet<ProductTransfer> ProductTransfers { get; set; }

    public virtual DbSet<ProductTransferItem> ProductTransferItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC076BC6D286");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ImageProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ImagePro__3214EC07606F6EB5");

            entity.ToTable("ImageProduct");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Url).HasMaxLength(256);

            entity.HasOne(d => d.Product).WithMany(p => p.ImageProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImageProd__Produ__5070F446");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07C56274DC");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.SendToNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SendTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__SendT__5441852A");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC076426E13A");

            entity.ToTable("Product");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AddById).HasColumnName("AddByID");
            entity.Property(e => e.BuyerId).HasColumnName("BuyerID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SellerId).HasColumnName("SellerID");
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.StatusId).HasColumnName("Status_ID");

            entity.HasOne(d => d.AddBy).WithMany(p => p.ProductAddBies)
                .HasForeignKey(d => d.AddById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__AddByID__38996AB5");

            entity.HasOne(d => d.Buyer).WithMany(p => p.ProductBuyers)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK__Product__BuyerID__3A81B327");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__35BCFE0A");

            entity.HasOne(d => d.Seller).WithMany(p => p.ProductSellers)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__Product__SellerI__398D8EEE");

            entity.HasOne(d => d.Station).WithMany(p => p.Products)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Station__37A5467C");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Status___36B12243");
        });

        modelBuilder.Entity<ProductActivy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductA__3214EC070F5CF566");

            entity.ToTable("ProductActivy");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StationsId).HasColumnName("Stations_ID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.NewStatusNavigation).WithMany(p => p.ProductActivyNewStatusNavigations)
                .HasForeignKey(d => d.NewStatus)
                .HasConstraintName("FK__ProductAc__NewSt__4222D4EF");

            entity.HasOne(d => d.OldStatusNavigation).WithMany(p => p.ProductActivyOldStatusNavigations)
                .HasForeignKey(d => d.OldStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__OldSt__412EB0B6");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductActivies)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__Produ__3F466844");

            entity.HasOne(d => d.Stations).WithMany(p => p.ProductActivies)
                .HasForeignKey(d => d.StationsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__Stati__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.ProductActivies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductAc__UserI__3E52440B");
        });

        modelBuilder.Entity<ProductTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC071BD01AD2");

            entity.ToTable("ProductTransfer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StationIdform).HasColumnName("StationIDForm");
            entity.Property(e => e.StationIdto).HasColumnName("StationIDTo");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.StationIdformNavigation).WithMany(p => p.ProductTransferStationIdformNavigations)
                .HasForeignKey(d => d.StationIdform)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Stati__45F365D3");

            entity.HasOne(d => d.StationIdtoNavigation).WithMany(p => p.ProductTransferStationIdtoNavigations)
                .HasForeignKey(d => d.StationIdto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Stati__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.ProductTransfers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__UserI__47DBAE45");
        });

        modelBuilder.Entity<ProductTransferItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC074DCCA930");

            entity.ToTable("ProductTransferItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductTransferId).HasColumnName("ProductTransferID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTransferItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Produ__571DF1D5");

            entity.HasOne(d => d.ProductTransfer).WithMany(p => p.ProductTransferItems)
                .HasForeignKey(d => d.ProductTransferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductTr__Produ__5812160E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC075FF46E04");

            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stations__3214EC076105E6B2");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07220FC4E0");

            entity.ToTable("Status");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07B2DE8640");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Produ__4BAC3F29");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Walle__4CA06362");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07D41D0E8A");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccessToken).HasMaxLength(256);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.RefreshToken).HasMaxLength(256);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__RoleID__2C3393D0");

            entity.HasOne(d => d.Station).WithMany(p => p.Users)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__User__StationID__2D27B809");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC27A396EA38");

            entity.ToTable("Wallet");

            entity.HasIndex(e => e.UserId, "UQ__Wallet__1788CCAD016FD305").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wallet_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
