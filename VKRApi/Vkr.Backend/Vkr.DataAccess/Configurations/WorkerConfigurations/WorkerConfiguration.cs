using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vkr.Domain.Entities.Worker;

namespace Vkr.DataAccess.Configurations.WorkerConfigurations;

public class WorkerConfiguration : IEntityTypeConfiguration<Workers>
{
    public void Configure(EntityTypeBuilder<Workers> builder)
    {
        builder.ToTable("workers").HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Workers.PropertyMaxLength);

        builder
            .Property(x => x.SecondName)
            .IsRequired()
            .HasMaxLength(Workers.PropertyMaxLength);

        builder
            .Property(x => x.ThirdName)
            .IsRequired()
            .HasMaxLength(Workers.PropertyMaxLength);

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(Workers.PropertyMaxLength);

        builder
            .Property(x => x.NormalizedEmail)
            .HasMaxLength(Workers.PropertyMaxLength);

        builder.Property(x => x.Phone)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(84) // Стандартная длина для хэша BCrypt
            .IsRequired(false);

        builder.Property(x => x.CanManageWorkers)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CanManageProjects)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(b => b.CreatedOn)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder
            .HasOne(x => x.WorkerPosition)
            .WithMany(x => x.Workers);

        builder
            .Property(x => x.WorkerStatus)
            .HasConversion<int>();

        builder
            .HasMany(w => w.WorkerFiles)
            .WithOne(of => of.Creator);

        // Связь работника с проектами
        builder.HasMany(x => x.ProjectsList)
            .WithMany(x => x.WorkersList)
            .UsingEntity("WorkerProgect");

        // Связь работника с наблюдаемыми задачами через TaskObservers
        builder.HasMany(w => w.TaskObservers)
            .WithOne(to => to.Worker)
            .HasForeignKey(to => to.WorkerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new Workers[]
        {
            new Workers {
                Id = 1,
                Name = "Иван",
                SecondName = "Петров",
                ThirdName = "Александрович",
                Email = "ivan.petrov@example.com",
                NormalizedEmail = "IVAN.PETROV@EXAMPLE.COM",
                WorkerPositionId = 1,
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                CanManageWorkers = true,
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" //admin
            },
            new Workers {
                Id = 2,
                Name = "Анна",
                SecondName = "Смирнова",
                ThirdName = "Сергеевна",
                Email = "anna.smirnova@example.com",
                NormalizedEmail = "ANNA.SMIRNOVA@EXAMPLE.COM",
                WorkerPositionId = 2,
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                CanManageWorkers = false,
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi"
            },
            new Workers {
                Id = 3,
                Name = "Михаил",
                SecondName = "Иванов",
                ThirdName = "Владимирович",
                Email = "mikhail.ivanov@example.com",
                NormalizedEmail = "MIKHAIL.IVANOV@EXAMPLE.COM",
                WorkerPositionId = 3,
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                CanManageWorkers = true,
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi"
            },
            new Workers {
                Id = 4,
                Name = "Екатерина",
                SecondName = "Козлова",
                ThirdName = "Дмитриевна",
                Email = "ekaterina.kozlova@example.com",
                NormalizedEmail = "EKATERINA.KOZLOVA@EXAMPLE.COM",
                WorkerPositionId = 4,
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc),
                CanManageWorkers = true,
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 5, 
                Name = "Александр", 
                SecondName = "Соколов", 
                ThirdName = "Игоревич", 
                Email = "alexandr.sokolov@example.com", 
                NormalizedEmail = "ALEXANDR.SOKOLOV@EXAMPLE.COM", 
                WorkerPositionId = 5, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = true, 
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 6, 
                Name = "Ольга", 
                SecondName = "Новикова", 
                ThirdName = "Павловна", 
                Email = "olga.novikova@example.com", 
                NormalizedEmail = "OLGA.NOVIKOVA@EXAMPLE.COM", 
                WorkerPositionId = 6, 
                WorkerStatus = WorkerStatus.Active, 
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 7, 
                Name = "Дмитрий", 
                SecondName = "Морозов", 
                ThirdName = "Алексеевич", 
                Email = "dmitry.morozov@example.com", 
                NormalizedEmail = "DMITRY.MOROZOV@EXAMPLE.COM", 
                WorkerPositionId = 7, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = true, 
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers{ 
                Id = 8, 
                Name = "Елена", 
                SecondName = "Попова", 
                ThirdName = "Николаевна", 
                Email = "elena.popova@example.com", 
                NormalizedEmail = "ELENA.POPOVA@EXAMPLE.COM", 
                WorkerPositionId = 8, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers =true, 
                CanManageProjects =true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 9, 
                Name = "Сергей", 
                SecondName = "Федоров", 
                ThirdName = "Михайлович", 
                Email = "sergey.fedorov@example.com", 
                NormalizedEmail = "SERGEY.FEDOROV@EXAMPLE.COM", 
                WorkerPositionId = 9, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 10, 
                Name = "Наталья", 
                SecondName = "Кузнецова", 
                ThirdName = "Викторовна", 
                Email = "natalya.kuznetsova@example.com", 
                NormalizedEmail = "NATALYA.KUZNETSOVA@EXAMPLE.COM", 
                WorkerPositionId = 10, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = true, 
                CanManageProjects = true,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 11, 
                Name = "Андрей", 
                SecondName = "Воробьев", 
                ThirdName = "Петрович", 
                Email = "andrey.vorobiev@example.com", 
                NormalizedEmail = "ANDREY.VOROBIEV@EXAMPLE.COM", 
                WorkerPositionId = 11, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 12, 
                Name = "Юлия", 
                SecondName = "Григорьева", 
                ThirdName = "Андреевна", 
                Email = "yuliya.grigorieva@example.com", 
                NormalizedEmail = "YULIYA.GRIGORIEVA@EXAMPLE.COM", 
                WorkerPositionId = 12, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 13, 
                Name = "Павел", 
                SecondName = "Сидоров", 
                ThirdName = "Евгеньевич", 
                Email = "pavel.sidorov@example.com", 
                NormalizedEmail = "PAVEL.SIDOROV@EXAMPLE.COM", 
                WorkerPositionId = 13, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 14, 
                Name = "Татьяна", 
                SecondName = "Лебедева", 
                ThirdName = "Игоревна", 
                Email = "tatiana.lebedeva@example.com", 
                NormalizedEmail = "TATIANA.LEBEDEVA@EXAMPLE.COM", 
                WorkerPositionId = 14, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 15, 
                Name = "Виктор", 
                SecondName = "Белов", 
                ThirdName = "Анатольевич", 
                Email = "viktor.belov@example.com", 
                NormalizedEmail = "VIKTOR.BELOV@EXAMPLE.COM", 
                WorkerPositionId = 15, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 16, 
                Name = "Мария", 
                SecondName = "Орлова", 
                ThirdName = "Владимировна", 
                Email = "maria.orlova@example.com", 
                NormalizedEmail = "MARIA.ORLOVA@EXAMPLE.COM", 
                WorkerPositionId = 16, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 17, 
                Name = "Роман", 
                SecondName = "Зайцев", 
                ThirdName = "Сергеевич", 
                Email = "roman.zaytsev@example.com", 
                NormalizedEmail = "ROMAN.ZAYTSEV@EXAMPLE.COM", 
                WorkerPositionId = 17, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 18, 
                Name = "Светлана", 
                SecondName = "Егорова", 
                ThirdName = "Михайловна", 
                Email = "svetlana.egorova@example.com", 
                NormalizedEmail = "SVETLANA.EGOROVA@EXAMPLE.COM", 
                WorkerPositionId = 18, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 19, 
                Name = "Владимир", 
                SecondName = "Крылов", 
                ThirdName = "Дмитриевич", 
                Email = "vladimir.krylov@example.com", 
                NormalizedEmail = "VLADIMIR.KRYLOV@EXAMPLE.COM", 
                WorkerPositionId = 19, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = true, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            },
            new Workers { 
                Id = 20, 
                Name = "Ксения", 
                SecondName = "Васильева", 
                ThirdName = "Егоровна", 
                Email = "kseniya.vasilyeva@example.com", 
                NormalizedEmail = "KSENIYA.VASILYEVA@EXAMPLE.COM", 
                WorkerPositionId = 20, 
                WorkerStatus = WorkerStatus.Active,
                CreatedOn = new DateTime(2025, 5, 19, 12, 27, 0, DateTimeKind.Utc), 
                CanManageWorkers = false, 
                CanManageProjects = false,
                PasswordHash = "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi" 
            }
        });
    }
}