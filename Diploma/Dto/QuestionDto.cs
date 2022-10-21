namespace Diploma.Dto;

public class QuestionDto
{
    public string QuestionText { get; set; } = null!;
    public List<AnswerDto> Answers { get; set; } = null!;
}