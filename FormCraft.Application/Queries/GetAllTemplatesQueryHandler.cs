using Dapper;
using FormCraft.Application.Common.Messaging;
using FormCraft.Application.Models.ViewModels;
using FormCraft.Domain.Aggregates.FormAggregate;
using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using System.Data;

namespace FormCraft.Application.Queries
{
    public class GetAllTemplatesQueryHandler : IQueryHandler<GetAllTemplatesQuery, IEnumerable<TemplateView>>
    {
        private readonly IDbConnection _dbCconnection;
        public GetAllTemplatesQueryHandler(IDbConnection dbCconnection)
        {
            _dbCconnection = dbCconnection;
        }

        public async Task<IEnumerable<TemplateView>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken)
        {
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
                    t.name AS Name,
                    q.""Id"" AS Id,
                    q.text AS Text,
                    q.type AS Type,
                    q.order_number AS OrderNumber
                FROM form f
                LEFT JOIN form_tag ft ON f.""Id"" = ft.form_id
                LEFT JOIN tag t ON ft.tag_id = t.""Id""
                INNER JOIN  question q ON f.""Id"" = q.form_id
                order by f.""Id"", q.order_number;";

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
                splitOn: "Name,Id");

            return formDic.Values;
        }
    }
}
