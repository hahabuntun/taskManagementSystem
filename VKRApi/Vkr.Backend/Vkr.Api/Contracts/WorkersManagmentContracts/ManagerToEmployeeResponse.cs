namespace Vkr.API.Contracts.WorkersManagmentContracts;

public record ManagerToEmployeeResponse(
    int MangerId,
    int SubordinateId
);