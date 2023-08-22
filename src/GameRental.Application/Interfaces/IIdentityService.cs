using GameRental.Application.DTO.User;
using GameRental.Application.SomeLogic;
using GameRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameRental.Application.Interfaces;

public interface IIdentityService
{
    Task<OperationDetails> CreateUserAsync(UserRegistrationDto userRegistrationDto);
    Task<OperationDetails> AuthenticateAsync(UserLoginDto user);
    Task LogOutAsync();
    Task<string> GetTokenForChangePasswordAsync(string email);
    Task<OperationDetails> ChangePasswordAsync(string email, string newPassword, string token);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDetailsDto> GetUserDetailsAsync(string id);
    Task<List<string>> GetAllRolesAsync();
    Task ChangeRoleAsync(UserChangeRoleDto userChangeRoleDto);
}