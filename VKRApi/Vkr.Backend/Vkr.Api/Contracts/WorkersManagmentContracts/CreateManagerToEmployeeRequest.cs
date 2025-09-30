namespace Vkr.API.Contracts.WorkersManagmentContracts;

public record CreateManagerToEmployeeRequest(
    int ManagerId,
    int SubordinateId
);