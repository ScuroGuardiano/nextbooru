namespace Nextbooru.Auth.Dto;

public class UserResponse
{
    // public required Guid Id { get; init; }
    // User shouldn't know his Id.
    public required string Username { get; init; }

    public required string DisplayName { get; init; }
}
