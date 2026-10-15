global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using System.Linq.Expressions;
global using System.Text.Json;


global using Schedule.Domain.Entities;
global using Schedule.Domain.Exceptions;
global using Schedule.Domain.ValueObjects;
global using Schedule.Domain.Repositories.Interfaces;
global using Schedule.Application.Dtos;
global using Schedule.Application.Services.Interfaces;
global using Schedule.Application.Exceptions;
global using Schedule.Application.Auth.Interfaces;
global using Schedule.Application.Services;
global using Schedule.Infrastructure.Database;
global using Schedule.Infrastructure.Database.Models;
global using Schedule.Infrastructure.Repositories; 
global using Schedule.Infrastructure.EntityTypeConfiguration;
global using Schedule.Infrastructure.Exceptions; 
global using Schedule.Infrastructure.Services;
global using Schedule.Infrastructure.Auth; 
global using static Schedule.Application.CacheKeyProvider;
global using static Schedule.Infrastructure.Mapper;