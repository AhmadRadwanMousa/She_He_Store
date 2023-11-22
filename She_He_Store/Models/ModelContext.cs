using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace She_He_Store.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Todo> Todos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=She_He_StoreDatabase;PASSWORD=Ahmadzuiod2;DATA SOURCE=localhost:1521/XEPDB1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("SHE_HE_STOREDATABASE")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C008442");

            entity.ToTable("CARD");

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Cardamount)
                .HasDefaultValueSql("3000")
                .HasColumnType("NUMBER")
                .HasColumnName("CARDAMOUNT");
            entity.Property(e => e.Cardholdername)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CARDHOLDERNAME");
            entity.Property(e => e.Cardnumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER")
                .HasColumnName("CVV");
            entity.Property(e => e.Expirtydate)
                .HasColumnType("NUMBER")
                .HasColumnName("EXPIRTYDATE");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008414");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Categorypicture)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CATEGORYPICTURE");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("SYS_C008423");

            entity.ToTable("ORDERS");

            entity.Property(e => e.Orderid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Orderdate)
                .HasColumnType("DATE")
                .HasColumnName("ORDERDATE");
            entity.Property(e => e.Orderstatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ORDERSTATUS");
            entity.Property(e => e.Paymentid)
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Shippingaddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SHIPPINGADDRESS");
            entity.Property(e => e.Totalamount)
                .HasDefaultValueSql("0.00 ")
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALAMOUNT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Paymentid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008425");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008424");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Orderitemid).HasName("SYS_C008427");

            entity.ToTable("ORDERITEM");

            entity.Property(e => e.Orderitemid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERITEMID");
            entity.Property(e => e.Orderid)
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Quantity)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.Totalprice)
                .HasDefaultValueSql("0.00\n")
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALPRICE");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008428");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008429");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008411");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Cardid)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Paymentdate)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Card).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Cardid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008443");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008412");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("SYS_C008419");

            entity.ToTable("PRODUCT");

            entity.Property(e => e.Productid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Addingdate)
                .HasColumnType("DATE")
                .HasColumnName("ADDINGDATE");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Isavaliable)
                .HasPrecision(1)
                .HasColumnName("ISAVALIABLE");
            entity.Property(e => e.Productdetails)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("PRODUCTDETAILS");
            entity.Property(e => e.Productname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("PRODUCTNAME");
            entity.Property(e => e.Productpicture)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PRODUCTPICTURE");
            entity.Property(e => e.Productprice)
                .HasDefaultValueSql("0.00 ")
                .HasColumnType("FLOAT")
                .HasColumnName("PRODUCTPRICE");
            entity.Property(e => e.Rating)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER")
                .HasColumnName("RATING");
            entity.Property(e => e.Sale)
                .HasDefaultValueSql("0.00")
                .HasColumnType("FLOAT")
                .HasColumnName("SALE");
            entity.Property(e => e.Stockquantity)
                .HasDefaultValueSql("0\n")
                .HasColumnType("NUMBER")
                .HasColumnName("STOCKQUANTITY");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008420");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("SYS_C008437");

            entity.ToTable("REVIEWS");

            entity.Property(e => e.Reviewid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REVIEWID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Rating)
                .HasColumnType("NUMBER")
                .HasColumnName("RATING");
            entity.Property(e => e.Reviewat)
                .HasColumnType("DATE")
                .HasColumnName("REVIEWAT");
            entity.Property(e => e.Reviewcomment)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("REVIEWCOMMENT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008432");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008431");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008385");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008434");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Testimonialcomment)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALCOMMENT");
            entity.Property(e => e.Testimonialstate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALSTATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008435");
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Todolistid).HasName("SYS_C008446");

            entity.ToTable("TODO");

            entity.Property(e => e.Todolistid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TODOLISTID");
            entity.Property(e => e.Tododescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TODODESCRIPTION");
            entity.Property(e => e.Todostatues)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TODOSTATUES");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008394");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("DATE")
                .HasColumnName("CREATEDAT");
            entity.Property(e => e.Dateofbirth)
                .HasColumnType("DATE")
                .HasColumnName("DATEOFBIRTH");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Postalcode)
                .HasColumnType("NUMBER")
                .HasColumnName("POSTALCODE");
            entity.Property(e => e.Profilepicture)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PROFILEPICTURE");
            entity.Property(e => e.Totalbalance)
                .HasDefaultValueSql("500")
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALBALANCE");
            entity.Property(e => e.Updatedat)
                .HasColumnType("DATE")
                .HasColumnName("UPDATEDAT");
            entity.Property(e => e.Userstatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERSTATUS");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Userloginid).HasName("SYS_C008400");

            entity.ToTable("USERLOGIN");

            entity.HasIndex(e => e.Username, "SYS_C008401").IsUnique();

            entity.Property(e => e.Userloginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGINID");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008403");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008402");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Wishlistid).HasName("SYS_C008448");

            entity.ToTable("WISHLIST");

            entity.Property(e => e.Wishlistid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("WISHLISTID");
            entity.Property(e => e.Addedat)
                .HasColumnType("DATE")
                .HasColumnName("ADDEDAT");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008450");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008449");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
