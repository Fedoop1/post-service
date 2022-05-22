using System.Security.Authentication;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using PostService.Common.Enums;
using PostService.Common.Types;
using PostService.Identity.Exceptions;

namespace PostService.Identity.Models.Domain;

public class User : IIdentifiable
{
    private static readonly Regex EmailRegex = new (
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User(string userName, string email, Role role)
    {
        if (string.IsNullOrEmpty(userName)) throw new InvalidUserException("User name can't be null or empty");
        if (!EmailRegex.IsMatch(email)) throw new InvalidUserException("User email doesn't match email regex");

        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        Role = role;

        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrEmpty(password)) throw new InvalidCredentialException("User password can't be null or empty");
        if (passwordHasher is null) throw new InvalidUserException("Password hasher can't be null");

        this.PasswordHash = passwordHasher.HashPassword(this, password);

        this.UpdatedAt = DateTime.Now;
    }

    public bool VerifyPassword(string password, IPasswordHasher<User> passwordHasher)
    {
        if (string.IsNullOrEmpty(password)) throw new InvalidCredentialException("User password can't be null or empty");
        if (passwordHasher is null) throw new InvalidUserException("Password hasher can't be null");

        return passwordHasher.VerifyHashedPassword(this, this.PasswordHash, password) ==
            PasswordVerificationResult.Success;
    }
}

