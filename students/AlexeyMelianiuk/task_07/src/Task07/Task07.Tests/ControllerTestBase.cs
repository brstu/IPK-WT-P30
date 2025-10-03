using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Task07.Application.Interfaces;
using Task07.Domain.Entities;
using Xunit;

namespace Task07.Tests;

public abstract class ControllerTestBase
{
    protected static List<Item> GetTestItems()
    {
        return new List<Item>
        {
            new Item { Id = 1, Name = "Test Item 1", Description = "Description 1", CreatedAt = DateTime.UtcNow },
            new Item { Id = 2, Name = "Test Item 2", Description = "Description 2", CreatedAt = DateTime.UtcNow }
        };
    }
}