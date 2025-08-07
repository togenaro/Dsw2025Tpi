using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos;

public record RoleRequest(string Username, string Role);

public record RoleResponse(string Username, string Role);


