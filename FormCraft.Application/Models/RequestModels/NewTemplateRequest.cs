using FormCraft.Application.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCraft.Application.Models.RequestModels
{
    public class NewTemplateRequest
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Topic { get; init; } = string.Empty;
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<QuestionDTO> Questions {  get; set; }
        public bool IsPublic { get; set; }
    }
}
