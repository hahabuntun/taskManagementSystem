using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Configurations.WorkerConfigurations;

public class WorkersManagmentConfiguration : IEntityTypeConfiguration<WorkersManagement>
{
    public void Configure(EntityTypeBuilder<WorkersManagement> builder)
    {
        builder.ToTable("workers_management").HasKey(x => x.Id);
        
        builder
            .Property(x => x.ManagerId)
            .IsRequired();
            
        builder
            .Property(x => x.SubordinateId)
            .IsRequired();
        
        // настройка подчинённых
        builder
            .HasOne(x => x.Manager)
            .WithMany(y => y.SelfSubmissions)
            .HasForeignKey(x => x.ManagerId);
        
        // настройка управляющих
        builder
            .HasOne(x => x.Subordinate)
            .WithMany(y => y.SelfManager)
            .HasForeignKey(x => x.SubordinateId);

        // Seed data for manager-subordinate relationships (manually defined)
        builder.HasData(
            // Manager ID 1 (Ivan Petrov)
            new WorkersManagement { Id = 1, ManagerId = 1, SubordinateId = 2 },  // Anna Smirnova
            new WorkersManagement { Id = 2, ManagerId = 1, SubordinateId = 9 },  // Sergey Fedorov
            new WorkersManagement { Id = 3, ManagerId = 1, SubordinateId = 11 }, // Andrey Vorobiev
            new WorkersManagement { Id = 4, ManagerId = 1, SubordinateId = 12 }, // Yuliya Grigorieva
            new WorkersManagement { Id = 5, ManagerId = 1, SubordinateId = 13 }, // Pavel Sidorov
            new WorkersManagement { Id = 6, ManagerId = 1, SubordinateId = 14 }, // Tatiana Lebedeva
            new WorkersManagement { Id = 7, ManagerId = 1, SubordinateId = 15 }, // Viktor Belov
            new WorkersManagement { Id = 8, ManagerId = 1, SubordinateId = 16 }, // Maria Orlova
            new WorkersManagement { Id = 9, ManagerId = 1, SubordinateId = 17 }, // Roman Zaytsev
            new WorkersManagement { Id = 10, ManagerId = 1, SubordinateId = 18 }, // Svetlana Egorova
            new WorkersManagement { Id = 11, ManagerId = 1, SubordinateId = 20 }, // Kseniya Vasilyeva

            // Manager ID 3 (Mikhail Ivanov)
            new WorkersManagement { Id = 12, ManagerId = 3, SubordinateId = 2 },
            new WorkersManagement { Id = 13, ManagerId = 3, SubordinateId = 9 },
            new WorkersManagement { Id = 14, ManagerId = 3, SubordinateId = 11 },
            new WorkersManagement { Id = 15, ManagerId = 3, SubordinateId = 12 },
            new WorkersManagement { Id = 16, ManagerId = 3, SubordinateId = 13 },
            new WorkersManagement { Id = 17, ManagerId = 3, SubordinateId = 14 },
            new WorkersManagement { Id = 18, ManagerId = 3, SubordinateId = 15 },
            new WorkersManagement { Id = 19, ManagerId = 3, SubordinateId = 16 },
            new WorkersManagement { Id = 20, ManagerId = 3, SubordinateId = 17 },
            new WorkersManagement { Id = 21, ManagerId = 3, SubordinateId = 18 },
            new WorkersManagement { Id = 22, ManagerId = 3, SubordinateId = 20 },

            // Manager ID 4 (Ekaterina Kozlova)
            new WorkersManagement { Id = 23, ManagerId = 4, SubordinateId = 2 },
            new WorkersManagement { Id = 24, ManagerId = 4, SubordinateId = 9 },
            new WorkersManagement { Id = 25, ManagerId = 4, SubordinateId = 11 },
            new WorkersManagement { Id = 26, ManagerId = 4, SubordinateId = 12 },
            new WorkersManagement { Id = 27, ManagerId = 4, SubordinateId = 13 },
            new WorkersManagement { Id = 28, ManagerId = 4, SubordinateId = 14 },
            new WorkersManagement { Id = 29, ManagerId = 4, SubordinateId = 15 },
            new WorkersManagement { Id = 30, ManagerId = 4, SubordinateId = 16 },
            new WorkersManagement { Id = 31, ManagerId = 4, SubordinateId = 17 },
            new WorkersManagement { Id = 32, ManagerId = 4, SubordinateId = 18 },
            new WorkersManagement { Id = 33, ManagerId = 4, SubordinateId = 20 },

            // Manager ID 5 (Alexandr Sokolov)
            new WorkersManagement { Id = 34, ManagerId = 5, SubordinateId = 2 },
            new WorkersManagement { Id = 35, ManagerId = 5, SubordinateId = 9 },
            new WorkersManagement { Id = 36, ManagerId = 5, SubordinateId = 11 },
            new WorkersManagement { Id = 37, ManagerId = 5, SubordinateId = 12 },
            new WorkersManagement { Id = 38, ManagerId = 5, SubordinateId = 13 },
            new WorkersManagement { Id = 39, ManagerId = 5, SubordinateId = 14 },
            new WorkersManagement { Id = 40, ManagerId = 5, SubordinateId = 15 },
            new WorkersManagement { Id = 41, ManagerId = 5, SubordinateId = 16 },
            new WorkersManagement { Id = 42, ManagerId = 5, SubordinateId = 17 },
            new WorkersManagement { Id = 43, ManagerId = 5, SubordinateId = 18 },
            new WorkersManagement { Id = 44, ManagerId = 5, SubordinateId = 20 },

            // Manager ID 7 (Dmitry Morozov)
            new WorkersManagement { Id = 45, ManagerId = 7, SubordinateId = 2 },
            new WorkersManagement { Id = 46, ManagerId = 7, SubordinateId = 9 },
            new WorkersManagement { Id = 47, ManagerId = 7, SubordinateId = 11 },
            new WorkersManagement { Id = 48, ManagerId = 7, SubordinateId = 12 },
            new WorkersManagement { Id = 49, ManagerId = 7, SubordinateId = 13 },
            new WorkersManagement { Id = 50, ManagerId = 7, SubordinateId = 14 },
            new WorkersManagement { Id = 51, ManagerId = 7, SubordinateId = 15 },
            new WorkersManagement { Id = 52, ManagerId = 7, SubordinateId = 16 },
            new WorkersManagement { Id = 53, ManagerId = 7, SubordinateId = 17 },
            new WorkersManagement { Id = 54, ManagerId = 7, SubordinateId = 18 },
            new WorkersManagement { Id = 55, ManagerId = 7, SubordinateId = 20 },

            // Manager ID 8 (Elena Popova)
            new WorkersManagement { Id = 56, ManagerId = 8, SubordinateId = 2 },
            new WorkersManagement { Id = 57, ManagerId = 8, SubordinateId = 9 },
            new WorkersManagement { Id = 58, ManagerId = 8, SubordinateId = 11 },
            new WorkersManagement { Id = 59, ManagerId = 8, SubordinateId = 12 },
            new WorkersManagement { Id = 60, ManagerId = 8, SubordinateId = 13 },
            new WorkersManagement { Id = 61, ManagerId = 8, SubordinateId = 14 },
            new WorkersManagement { Id = 62, ManagerId = 8, SubordinateId = 15 },
            new WorkersManagement { Id = 63, ManagerId = 8, SubordinateId = 16 },
            new WorkersManagement { Id = 64, ManagerId = 8, SubordinateId = 17 },
            new WorkersManagement { Id = 65, ManagerId = 8, SubordinateId = 18 },
            new WorkersManagement { Id = 66, ManagerId = 8, SubordinateId = 20 },

            // Manager ID 10 (Natalya Kuznetsova)
            new WorkersManagement { Id = 67, ManagerId = 10, SubordinateId = 2 },
            new WorkersManagement { Id = 68, ManagerId = 10, SubordinateId = 9 },
            new WorkersManagement { Id = 69, ManagerId = 10, SubordinateId = 11 },
            new WorkersManagement { Id = 70, ManagerId = 10, SubordinateId = 12 },
            new WorkersManagement { Id = 71, ManagerId = 10, SubordinateId = 13 },
            new WorkersManagement { Id = 72, ManagerId = 10, SubordinateId = 14 },
            new WorkersManagement { Id = 73, ManagerId = 10, SubordinateId = 15 },
            new WorkersManagement { Id = 74, ManagerId = 10, SubordinateId = 16 },
            new WorkersManagement { Id = 75, ManagerId = 10, SubordinateId = 17 },
            new WorkersManagement { Id = 76, ManagerId = 10, SubordinateId = 18 },
            new WorkersManagement { Id = 77, ManagerId = 10, SubordinateId = 20 },

            // Manager ID 19 (Vladimir Krylov)
            new WorkersManagement { Id = 78, ManagerId = 19, SubordinateId = 2 },
            new WorkersManagement { Id = 79, ManagerId = 19, SubordinateId = 9 },
            new WorkersManagement { Id = 80, ManagerId = 19, SubordinateId = 11 },
            new WorkersManagement { Id = 81, ManagerId = 19, SubordinateId = 12 },
            new WorkersManagement { Id = 82, ManagerId = 19, SubordinateId = 13 },
            new WorkersManagement { Id = 83, ManagerId = 19, SubordinateId = 14 },
            new WorkersManagement { Id = 84, ManagerId = 19, SubordinateId = 15 },
            new WorkersManagement { Id = 85, ManagerId = 19, SubordinateId = 16 },
            new WorkersManagement { Id = 86, ManagerId = 19, SubordinateId = 17 },
            new WorkersManagement { Id = 87, ManagerId = 19, SubordinateId = 18 },
            new WorkersManagement { Id = 88, ManagerId = 19, SubordinateId = 20 }
        );
    }
}