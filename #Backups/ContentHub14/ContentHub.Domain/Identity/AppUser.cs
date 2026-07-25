using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ContentHub.Domain.Identity;

public class AppUser:IdentityUser<Guid>
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;    
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;

    private AppUser() { }

    public AppUser(string firstName,string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;        
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Active() => IsActive = true;

}
