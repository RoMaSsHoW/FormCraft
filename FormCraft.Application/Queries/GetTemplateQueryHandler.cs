using Dapper;
using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using System.Data;

namespace FormCraft.Application.Queries
{
    public class GetTemplateQueryHandler : IQueryHandler<GetTemplateQuery, TemplateView>
    {
        private readonly IDbConnection _dbCconnection;
        private readonly ICurrentUserService _currentUserService;

        public GetTemplateQueryHandler(
            IDbConnection dbCconnection,
            ICurrentUserService currentUserService)
        {
            _dbCconnection = dbCconnection;
            _currentUserService = currentUserService;
        }

        public async Task<TemplateView> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated())
                throw new UnauthorizedAccessException("User unauthorized");

            const string sql = @"
                SELECT
                    f.""Id"" AS Id,
                    f.author_id AS AuthorId,
                    f.title AS Title,
                    f.description AS Description,
                    f.topic_name AS TopicName,
                    f.is_public AS IsPublic,
                    f.last_modified AS LastModified,
                    f.creation_time AS CreationTime,
                    t.""Id"" AS Id,
                    t.name AS Name,
                    q.""Id"" AS Id,
                    q.text AS Text,
                    q.type AS Type,
                    q.order_number AS OrderNumber
                FROM form f
                LEFT JOIN form_tag ft ON f.""Id"" = ft.form_id
                LEFT JOIN tag t ON ft.tag_id = t.""Id""
                INNER JOIN  question q ON f.""Id"" = q.form_id
                WHERE f.""Id"" = @TemplateId
                order by f.""Id"", q.order_number;";
            var formDic = new Dictionary<Guid, TemplateView>();
            var parameters = new
            {
                TemplateId = request.TemplateId
            };

            var result = await _dbCconnection.QueryAsync<Form, Tag, QuestionView, TemplateView>(
                sql,
                (form, tag, question) =>
                {
                    if (!formDic.TryGetValue(form.Id, out var formView))
                    {
                        formView = new TemplateView()
                        {
                            FormId = form.Id,
                            AuthorId = form.AuthorId,
                            Title = form.Title,
                            Description = form.Description,
                            TopicName = form.TopicName,
                            IsPublic = form.IsPublic,
                            LastModified = form.LastModified,
                            CreationTime = form.CreationTime,
                            Tags = new List<Tag>(),
                            Questions = new List<QuestionView>()
                        };
                        formDic.Add(form.Id, formView);
                    }
                    if (tag != null && !formView.Tags.Any(t => t.Id == tag.Id))
                    {
                        formView.Tags.Add(tag);
                    }
                    if (question != null && !formView.Questions.Any(q => q.Id == question.Id))
                    {
                        formView.Questions.Add(new QuestionView()
                        {
                            Id = question.Id,
                            Text = question.Text,
                            Type = question.Type.ToString(),
                            OrderNumber = question.OrderNumber
                        });
                    }
                    return formView;
                },
                param: parameters,
                splitOn: "Id,Id");

            return formDic.Values.FirstOrDefault();
        }
    }
}
