using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using MySeries;
using MySeries.Watchlists;
using MySeries.Notifications;
using MySeries.Usuarios;

namespace MySeries.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class MySeriesDbContext :
    AbpDbContext<MySeriesDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    // Agregar las entidades aqui


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
     public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    
    public DbSet<Serie> series { get; set; }
    public DbSet<WatchList> WatchLists { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<WatchListSerie> WatchListSeries { get; set; }

    public MySeriesDbContext(DbContextOptions<MySeriesDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();


        // --- Configuración de Serie ---
        builder.Entity<Serie>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Series", MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired().HasMaxLength(256);
            b.Property(x => x.Genre).IsRequired().HasMaxLength(128);
            b.Property(x => x.Year).IsRequired().HasMaxLength(20);
        });

        // --- Configuración de WatchList  ---
        builder.Entity<WatchList>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "WatchLists", MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
        });

        // --- Configuración de Qualification  ---
        builder.Entity<Qualification>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Qualifications", MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
        });

        // --- Configuración de Notification  ---
        builder.Entity<Notification>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Notifications", MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
        });

        // --- Configuración de Usuario  ---
        builder.Entity<Usuario>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Usuarios", MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.UserName).IsRequired().HasMaxLength(60);
            b.Property(x => x.Password).IsRequired().HasMaxLength(15);
        });

        // --- Configuración de WatchListSerie  ---
        builder.Entity<WatchListSerie>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "WatchListSeries", MySeriesConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(x => new { x.WatchListId, x.SerieId });

            b.HasOne(x => x.WatchList)
                .WithMany(w => w.WatchListSeries)
                .HasForeignKey(x => x.WatchListId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Serie)
                .WithMany(s => s.WatchListSeries)
                .HasForeignKey(x => x.SerieId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(MySeriesConsts.DbTablePrefix + "YourEntities", MySeriesConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
