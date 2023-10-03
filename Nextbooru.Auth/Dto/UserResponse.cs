namespace Nextbooru.Auth.Dto;

public class UserResponse
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
}
