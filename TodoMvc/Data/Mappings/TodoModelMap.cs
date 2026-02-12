using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoMvc.Models;

namespace TodoMvc.Data.Mappings;

public class TodoModelMap : IEntityTypeConfiguration<ToDoModel>
{
    public void Configure(EntityTypeBuilder<ToDoModel> builder)
    {
        builder.ToTable("ToDo");
            
        builder.HasKey(toDoModel => toDoModel.Id);
            
        builder.Property(toDoModel => toDoModel.Id)
            .ValueGeneratedOnAdd()
            .HasAnnotation("Sqlite:Autoincrement", true);

         builder.Property(toDoModel => toDoModel.Title)
            .IsRequired()
            .HasColumnName("Title")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(toDoModel => toDoModel.CreatedAt)
            .IsRequired()
            .HasColumnName("CreatedAt")
            .HasColumnType("DateTime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(toDoModel => toDoModel.Done)
            .IsRequired()
            .HasColumnName("Done")
            .HasColumnType("BOOLEAN")
            .HasDefaultValue(false);
    }
}