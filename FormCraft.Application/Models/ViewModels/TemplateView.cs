﻿using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;

namespace FormCraft.Application.Models.ViewModels
{
    public class TemplateView
    {
        public Guid FormId { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TopicName { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModified { get; set; }
        public uint Xmin { get; set; }
        public List<Tag> Tags { get; set; }
        public List<QuestionView> Questions { get; set; }
    }
}
