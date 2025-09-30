using Vkr.Application.Filters;
using Vkr.Application.Interfaces;
using Vkr.Application.Interfaces.Infractructure;
using Vkr.Application.Interfaces.Repositories.WorkerRepositories;
using Vkr.Application.Interfaces.Services.WorkerServices;
using Vkr.Application.Interfaces.Services.HistoryServices; // Добавлено
using Vkr.Application.DTOs.NotificationDTOs;
using Vkr.Domain.Entities.Worker;
using Vkr.Domain.Entities.Notification;
using Vkr.Domain.Entities.History; // Добавлено

namespace Vkr.Application.Services.WorkerServices;

public class WorkersService : IWorkersService
{
    private readonly IWorkersRepository _workersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly INotificationService _notificationService;
    private readonly IHistoryService _historyService; // Добавлено

    public WorkersService(
        IWorkersRepository workersRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        INotificationService notificationService,
        IHistoryService historyService) // Добавлено
    {
        _workersRepository = workersRepository ?? throw new ArgumentNullException(nameof(workersRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService)); // Добавлено
    }

    public async Task<Workers> CreateWorker(Workers workers, string password, int creatorId)
    {
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        // Normalize email
        workers.NormalizedEmail = workers.Email.ToUpper();
        // Hash password
        workers.PasswordHash = _passwordHasher.Generate(password);

        var createdWorker = await _workersRepository.CreateAsync(workers);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сотрудник '{workers.Email}' добавлен в организацию.",
            RelatedEntityId = createdWorker.Id,
            RelatedEntityType = HistoryEntityType.Worker,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == 1)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Новый сотрудник '{workers.Email}' добавлен в организацию.",
                RelatedEntityName = workers.Email ?? "Сотрудник",
                RelatedEntityId = createdWorker.Id,
                RelatedEntityType = EntityType.Organization,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return createdWorker;
    }

    public async Task<bool> DeleteWorkerAsync(int id, int creatorId)
    {
        if (id <= 0)
            throw new ArgumentException("Идентификатор сотрудника должен быть положительным.", nameof(id));
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        // Get worker email for notification
        var worker = await _workersRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Сотрудник с ID {id} не найден");
        var workerEmail = worker.Email;

        var deleted = await _workersRepository.DeleteAsync(id);
        if (!deleted)
            return false;

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Сотрудник '{workerEmail}' удален из организации.",
            RelatedEntityId = id,
            RelatedEntityType = HistoryEntityType.Worker,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == 1)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Сотрудник '{workerEmail}' удален из организации.",
                RelatedEntityName = workerEmail ?? "Сотрудник",
                RelatedEntityId = id,
                RelatedEntityType = EntityType.Organization,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return true;
    }

    public async Task<List<Workers>> GetWorkersByFilterAsync(WorkersFilter filter)
    {
        return await _workersRepository.GetByFilterAsync(filter);
    }

    public async Task<Workers> UpdateWorkerAsync(int id, Workers workers, int creatorId)
    {
        if (id <= 0)
            throw new ArgumentException("Идентификатор сотрудника должен быть положительным.", nameof(id));
        if (creatorId <= 0)
            throw new ArgumentException("Идентификатор создателя должен быть положительным.", nameof(creatorId));

        var updatedWorker = await _workersRepository.UpdateAsync(id, workers);

        // Add history record
        var historyDto = new CreateHistoryDTO
        {
            Text = $"Данные сотрудника '{workers.Email}' обновлены в организации.",
            RelatedEntityId = id,
            RelatedEntityType = HistoryEntityType.Worker,
            CreatorId = creatorId
        };
        await _historyService.AddHistoryItemAsync(historyDto);

        // Send notification to workers subscribed to the organization
        var subscriptions = await _notificationService.GetEntitySubscriptions(1, EntityType.Organization);
        var subscribedWorkerIds = subscriptions
            .Where(s => s.EntityType == EntityType.Organization && s.EntityId == 1)
            .Select(s => s.WorkerId)
            .ToList();

        if (subscribedWorkerIds.Any())
        {
            var notificationDto = new CreateNotificationDto
            {
                Text = $"Данные сотрудника '{workers.Email}' обновлены в организации.",
                RelatedEntityName = workers.Email ?? "Сотрудник",
                RelatedEntityId = id,
                RelatedEntityType = EntityType.Organization,
                CreatorId = creatorId,
                WorkerIds = subscribedWorkerIds
            };
            await _notificationService.CreateNotificationAsync(notificationDto);
        }

        return updatedWorker;
    }

    public async Task<Workers> GetWorkerByIdAsync(int id)
    {
        return await _workersRepository.GetByIdAsync(id);
    }

    public async Task<bool> IsEmailUnique(string candidateEmail)
    {
        return await _workersRepository.IsEmailUnique(candidateEmail);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _workersRepository.GetByEmail(email);

        if (user == null)
        {
            return string.Empty;
        }

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (!result)
        {
            return string.Empty;
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    public async Task<bool> IsEmailUniqueForUpdate(int workerId, string email)
    {
        var normalizedEmail = email.ToUpper();
        var workers = await _workersRepository.GetByFilterAsync(new WorkersFilter { Email = email });
        return !workers.Any(w => w.Id != workerId && w.NormalizedEmail == normalizedEmail);
    }
}