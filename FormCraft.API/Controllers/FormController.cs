﻿using FormCraft.Application.Commands.FormCommands;
using FormCraft.Application.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FormCraft.API.Controllers
{
    public class FormController : BaseApiController
    {
        public FormController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("setAnswerToQuestion")]
        public async Task<IActionResult> SetAnswer([FromForm] AnswerRequest answerRequest)
        {
            var command = new SetAnswerToQuestionCommand(answerRequest);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("deleteAnswersFromQuestions")]
        public async Task<IActionResult> DeleteAnswers(IEnumerable<AnswersToDeleteRequestModel> AnswersToDeleteRequest)
        {
            var command = new DeleteAnswersFromQoestionsCommand(AnswersToDeleteRequest);
            await Mediator.Send(command);
            return Ok();
        }
    }
}
