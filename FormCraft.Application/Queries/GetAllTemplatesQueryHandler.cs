using Dapper;
using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;
using FormCraft.Domain.Aggregates.UserAggregate.ValueObjects;
using System.Data;

namespace FormCraft.Application.Queries
{
    public class GetAllTemplatesQueryHandler : IQueryHandler<GetAllTemplatesQuery, IEnumerable<TemplateView>>
    {
        private readonly IDbConnection _dbCconnection;
        private readonly ICurrentUserService _currentUserService;

        public GetAllTemplatesQueryHandler(
            IDbConnection dbCconnection,
            ICurrentUserService currentUserService)
        {
            _dbCconnection = dbCconnection;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<TemplateView>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
        {
            var sql = @"
                SELECT
                    f.""Id"" AS Id,
                    f.author_id AS AuthorId,
                    f.title AS Title,
                    f.description AS Description,
                    f.topic_name AS TopicName,
                    f.is_public AS IsPublic,
                    f.last_modified AS LastModified,
                    f.creation_time AS CreationTime,
                    t.name AS Name,
                    q.""Id"" AS Id,
                    q.text AS Text,
                    q.type AS Type,
                    q.order_number AS OrderNumber
                FROM form f
                LEFT JOIN form_tag ft ON f.""Id"" = ft.form_id
                LEFT JOIN tag t ON ft.tag_id = t.""Id""
                INNER JOIN  question q ON f.""Id"" = q.form_id
                WHERE 1 = 1";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.TagName))
            {
                sql += " AND t.name ILIKE @TagName";
                parameters.Add("TagName", $"%{request.TagName}%");
            }

            if (!string.IsNullOrWhiteSpace(request.TopicName))
            {
                sql += " AND f.topic_name ILIKE @TopicName";
                parameters.Add("TopicName", $"%{request.TopicName}%");
            }

            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                sql += " AND f.title ILIKE @TitleSearch";
                parameters.Add("TitleSearch", $"%{request.TitleSearch}%");
            }

            if (_currentUserService.IsAuthenticated())
            {
                var userId = _currentUserService.GetUserId();
                var userRole = _currentUserService.GetRole();
                var isAdmin = Role.FromName<Role>(userRole) == Role.Admin;

                if (!isAdmin)
                {
                    sql += " AND (f.is_public = true OR f.author_id = @UserId)";
                    parameters.Add("UserId", userId);
                }
            }
            else
            {
                sql += " AND f.is_public = true";
            }

            sql += @" ORDER BY f.""Id"", q.order_number;";

            var formDic = new Dictionary<Guid, TemplateView>();

            var result = await _dbCconnection.QueryAsync<Form, Tag, QuestionView, TemplateView>(
                sql,
                (form, tag, question) =>
                {
                    if (!formDic.TryGetValue(form.Id, out var formView))
                    {
                        formView = new TemplateView()
                        {
                            Id = form.Id,
                            AuthorId = form.AuthorId,
                            Title = form.Title,
                            Description = form.Description,
                            TopicName = form.TopicName,
                            IsPublic = form.IsPublic,
                            LastModified = form.LastModified,
                            CreationTime = form.CreationTime,
                            Tags = new List<string>(),
                            Questions = new List<QuestionView>()
                        };
                        formDic.Add(form.Id, formView);
                    }
                    if (tag != null && !formView.Tags.Contains(tag.Name))
                    {
                        formView.Tags.Add(tag.Name);
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
                parameters,
                splitOn: "Name,Id");

            return formDic.Values;
        }
    }
}
