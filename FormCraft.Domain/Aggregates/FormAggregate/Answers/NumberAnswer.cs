﻿using FormCraft.Domain.Aggregates.FormAggregate.ValueObjects;
using FormCraft.Domain.Aggregates.UserAggregate.Interfaces;

namespace FormCraft.Domain.Aggregates.FormAggregate.Answers
{
    public class NumberAnswer : Answer
    {
        public NumberAnswer() { }

        private NumberAnswer(
            Guid userId,
            Guid questionId,
            int value)
        {
            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("User unauthorized");

            if (questionId == Guid.Empty)
                throw new ArgumentException("Question cannot be empty");

            if (value <= 0)
                throw new ArgumentException("Answer cannot be zero or negative");

            AuthorId = userId;
            QuestionId = questionId;
            Value = value;
        }

        public int Value { get; private set; }
        public override QuestionType Type => QuestionType.Number;

        public static NumberAnswer Create(
            Guid userId,
            Guid questionId,
            int value)
        {
            return new NumberAnswer(
                userId,
                questionId,
                value);
        }

        public void ChangeValue(int value, ICurrentUserService currentUserService)
        {
            if (!UserIsAuthorOrAdmin(currentUserService))
                throw new ArgumentException("User not author or admin");

            if (value <= 0)
                throw new ArgumentException("Answer cannot be zero or negative");

            if (value == Value)
                return;

            Value = value;
        }
    }
}
