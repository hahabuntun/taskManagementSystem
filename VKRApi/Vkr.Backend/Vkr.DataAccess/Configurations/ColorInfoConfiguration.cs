using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities;

namespace Vkr.DataAccess.Configurations
{
    public class ColorInfoConfiguration : IEntityTypeConfiguration<ColorInfo>
    {
        public void Configure(EntityTypeBuilder<ColorInfo> builder)
        {
            builder.ToTable("colors");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Code).IsRequired();

            builder.HasData(
                new ColorInfo { Id = 1, Code = "#FF5733", Name = "Vibrant orange" },
                new ColorInfo { Id = 2, Code = "#32CD32", Name = "Green" },
                new ColorInfo { Id = 3, Code = "#3357FF", Name = "Strong blue" },
                new ColorInfo { Id = 4, Code = "#DC143C", Name = "Pink-red" },
                new ColorInfo { Id = 5, Code = "#00BFFF", Name = "Teal-blue" },
                new ColorInfo { Id = 7, Code = "#FFA500", Name = "Dark orange" },
                new ColorInfo { Id = 8, Code = "#8A2BE2", Name = "Purple" },
                new ColorInfo { Id = 9, Code = "#228B22", Name = "Dark green" },
                new ColorInfo { Id = 10, Code = "#8B008B", Name = "Dark purple-red" },
                new ColorInfo { Id = 11, Code = "#FFD700", Name = "Gold" },
                new ColorInfo { Id = 12, Code = "#A0522D", Name = "Brown" },
                new ColorInfo { Id = 13, Code = "#ADD8E6", Name = "Light blue" },
                new ColorInfo { Id = 14, Code = "#808080", Name = "Gray" },
                new ColorInfo { Id = 15, Code = "#C0C0C0", Name = "Light gray" },
                new ColorInfo { Id = 16, Code = "#90EE90", Name = "Medium Green" },
                new ColorInfo { Id = 17, Code = "#FF69B4", Name = "Pink-red" },
                new ColorInfo { Id = 18, Code = "#FFA500", Name = "Orange-red" },
                new ColorInfo { Id = 19, Code = "#000080", Name = "Dark blue" },
                new ColorInfo { Id = 20, Code = "#008080", Name = "Teal-Green" }
            );

        }
    }
}
