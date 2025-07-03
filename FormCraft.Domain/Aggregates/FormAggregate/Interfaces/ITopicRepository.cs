﻿using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Domain.Aggregates.FormAggregate.Interfaces
{
    public interface ITopicRepository
    {
        Task<Topic> FindByNameAsync(string name);
    }
}
