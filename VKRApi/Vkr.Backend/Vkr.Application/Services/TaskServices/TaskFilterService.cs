using System.Text.Json;
using Vkr.Application.Interfaces.Repositories;
using Vkr.Application.Interfaces.Services;
using Vkr.Domain.DTO.Task;
using Vkr.Domain.Entities;

namespace Vkr.Application.Services;

public class TaskFilterService : ITaskFilterService
{
    private readonly ITaskFilterRepository _taskFilterRepository;

    public TaskFilterService(ITaskFilterRepository taskFilterRepository)
    {
        _taskFilterRepository = taskFilterRepository;
    }

    public async Task<IEnumerable<TaskFilterDTO>> GetTaskFiltersAsync()
    {
        var filters = await _taskFilterRepository.GetTaskFiltersAsync();
        return filters.Select(f => new TaskFilterDTO
        {
            Name = f.Name,
            Options = JsonSerializer.Deserialize<TaskFilterOptionsDTO>(f.OptionsJson) ?? new TaskFilterOptionsDTO()
        }).ToList();
    }

    public async Task AddTaskFilterAsync(string name, TaskFilterOptionsDTO options)
    {
        var existingFilter = await _taskFilterRepository.GetTaskFilterByNameAsync(name);
        if (existingFilter != null)
            throw new ArgumentException($"Filter with name '{name}' already exists");

        var filter = new TaskFilter
        {
            Name = name,
            OptionsJson = JsonSerializer.Serialize(options)
        };
        await _taskFilterRepository.AddTaskFilterAsync(filter);
    }

    public async Task EditTaskFilterAsync(string name, TaskFilterOptionsDTO options)
    {
        var filter = await _taskFilterRepository.GetTaskFilterByNameAsync(name)
            ?? throw new ArgumentException($"Filter with name '{name}' not found");

        filter.OptionsJson = JsonSerializer.Serialize(options);
        await _taskFilterRepository.UpdateTaskFilterAsync(filter);
    }

    public async Task RemoveTaskFilterAsync(string name)
    {
        var filter = await _taskFilterRepository.GetTaskFilterByNameAsync(name)
            ?? throw new ArgumentException($"Filter with name '{name}' not found");

        await _taskFilterRepository.RemoveTaskFilterAsync(name);
    }
}