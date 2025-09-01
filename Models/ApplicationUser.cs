using Microsoft.AspNetCore.Identity;

namespace Stores.Models;

public class ApplicationUser  : IdentityUser
{
    public string Name { get; set; }

    public string GetUserID()
    {
        return this.Id;
    }

}