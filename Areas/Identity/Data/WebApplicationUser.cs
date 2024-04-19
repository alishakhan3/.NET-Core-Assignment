using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Areas.Identity.Data;
public class WebApplicationUser : IdentityUser
{
	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string Name { get; set; }

	[PersonalData]
	[Column(TypeName = "nvarchar(100)")]
	public string Address { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string accId { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string profileType { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string name { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string url { get; set; }
}

