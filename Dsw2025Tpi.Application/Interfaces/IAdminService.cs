﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Interfaces;

public interface IAdminService
{
    Task<RoleResponse> AssignRole(RoleRequest request);

    Task<RoleResponse> RemoveRole(RoleRequest request);

    Task<RoleResponse> CreateRole(string roleName);
}
