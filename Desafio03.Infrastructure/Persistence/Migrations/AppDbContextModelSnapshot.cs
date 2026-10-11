using Desafio03.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Desafio03.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
public partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.31")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Desafio03.Domain.Entities.Cliente", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:Identity", "1, 1");

            b.Property<string>("Apellido")
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnType("nvarchar(80)");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<string>("Nombre")
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnType("nvarchar(80)");

            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.ToTable("Clientes");
        });

        modelBuilder.Entity("Desafio03.Domain.Entities.Producto", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:Identity", "1, 1");

            b.Property<string>("Descripcion")
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            b.Property<string>("Nombre")
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("nvarchar(120)");

            b.Property<decimal>("Precio")
                .HasColumnType("decimal(18,2)");

            b.Property<int>("Stock")
                .HasColumnType("int");

            b.HasKey("Id");
            b.ToTable("Productos");
        });

        modelBuilder.Entity("Desafio03.Domain.Entities.Pedido", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:Identity", "1, 1");

            b.Property<int>("Cantidad")
                .HasColumnType("int");

            b.Property<int>("ClienteId")
                .HasColumnType("int");

            b.Property<DateTime>("FechaPedido")
                .HasColumnType("datetime2");

            b.Property<int>("ProductoId")
                .HasColumnType("int");

            b.HasKey("Id");
            b.HasIndex("ClienteId");
            b.HasIndex("ProductoId");
            b.ToTable("Pedidos");
        });

        modelBuilder.Entity("Desafio03.Domain.Entities.Pedido", b =>
        {
            b.HasOne("Desafio03.Domain.Entities.Cliente", "Cliente")
                .WithMany("Pedidos")
                .HasForeignKey("ClienteId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.HasOne("Desafio03.Domain.Entities.Producto", "Producto")
                .WithMany("Pedidos")
                .HasForeignKey("ProductoId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Cliente");
            b.Navigation("Producto");
        });

        modelBuilder.Entity("Desafio03.Domain.Entities.Cliente", b =>
        {
            b.Navigation("Pedidos");
        });

        modelBuilder.Entity("Desafio03.Domain.Entities.Producto", b =>
        {
            b.Navigation("Pedidos");
        });
    }
}
