using FluentMigrator;
using System.Data;

namespace FormCraft.Infrastructure.Persistance.Migrations
{
    [Migration(202507162155)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("form")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("author_id").AsGuid().NotNullable()
                .WithColumn("title").AsString(255).NotNullable()
                .WithColumn("description").AsString(255).NotNullable()
                .WithColumn("topic_name").AsString(255).NotNullable()
                .WithColumn("is_public").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("last_modified").AsDateTime().NotNullable()
                .WithColumn("creation_time").AsDateTime().NotNullable();
            Create.Table("tag")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("name").AsString(255).NotNullable();

            Create.Table("topic")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("name").AsString(255).NotNullable();

            Create.Table("user")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("email").AsString(255).NotNullable()
                .WithColumn("password_hash").AsString().NotNullable()
                .WithColumn("role").AsString().NotNullable()
                .WithColumn("refresh_token").AsString().NotNullable()
                .WithColumn("refresh_token_last_updated").AsDateTime().NotNullable();

            Create.Table("question")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("form_id").AsGuid().NotNullable()
                .WithColumn("author_id").AsGuid().NotNullable()
                .WithColumn("text").AsString(255).NotNullable()
                .WithColumn("type").AsString().NotNullable()
                .WithColumn("order_number").AsInt32().NotNullable();

            Create.Table("form_tag")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("form_id").AsGuid().NotNullable()
                .WithColumn("tag_id").AsGuid().NotNullable();

            Create.Table("answer")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("question_id").AsGuid().NotNullable()
                .WithColumn("author_id").AsGuid().NotNullable()
                .WithColumn("discriminator").AsString(8).NotNullable()
                .WithColumn("boolean_value").AsBoolean().Nullable()
                .WithColumn("number_value").AsInt32().Nullable()
                .WithColumn("text_value").AsString(255).Nullable();

            Create.ForeignKey("FK_question_form_form_id")
                .FromTable("question").ForeignColumn("form_id")
                .ToTable("form").PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.ForeignKey("FK_form_tag_form_form_id")
                .FromTable("form_tag").ForeignColumn("form_id")
                .ToTable("form").PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.ForeignKey("FK_form_tag_tag_tag_id")
                .FromTable("form_tag").ForeignColumn("tag_id")
                .ToTable("tag").PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.ForeignKey("FK_answer_question_question_id")
                .FromTable("answer").ForeignColumn("question_id")
                .ToTable("question").PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.Index("IX_answer_question_id").OnTable("answer").OnColumn("question_id");
            Create.Index("IX_form_title").OnTable("form").OnColumn("title");
            Create.Index("IX_form_topic_name").OnTable("form").OnColumn("topic_name");
            Create.Index("IX_form_tag_form_id").OnTable("form_tag").OnColumn("form_id");
            Create.Index("IX_form_tag_tag_id").OnTable("form_tag").OnColumn("tag_id");
            Create.Index("IX_question_form_id").OnTable("question").OnColumn("form_id");
            Create.Index("IX_tag_name").OnTable("tag").OnColumn("name");
            Create.Index("IX_user_email").OnTable("user").OnColumn("email").Unique();
        }
        public override void Down()
        {
            Delete.ForeignKey("FK_answer_question_question_id").OnTable("answer");
            Delete.ForeignKey("FK_form_tag_tag_tag_id").OnTable("form_tag");
            Delete.ForeignKey("FK_form_tag_form_form_id").OnTable("form_tag");
            Delete.ForeignKey("FK_question_form_form_id").OnTable("question");
            Delete.Table("answer");
            Delete.Table("form_tag");
            Delete.Table("question");
            Delete.Table("user");
            Delete.Table("topic");
            Delete.Table("tag");
            Delete.Table("form");
        }
    }
}
