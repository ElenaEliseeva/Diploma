using Diploma.Dto;
using Diploma.Models;

namespace Diploma.Mapping;

public static class DomainToDtoMapping
{
    public static QuizDto ToDto(this Test test)
    {
        var questions = test.TestQuestions.Select(x => x.Question).ToList();
        var quizDto = new QuizDto();
        var questionsList = (from question in questions
            let listOfAnswer = question.QuestionAnswers.Select(questionAnswer => new AnswerDto
            {
                AnswerResult = questionAnswer.Answer.AnswerResult, AnswerText = questionAnswer.Answer.AnswerText,
                AnswerTextResult = questionAnswer.Answer.AnswerTextResult
            }).ToList()
            select new QuestionDto { QuestionText = question.QuestionText, Answers = listOfAnswer }).ToList();

        quizDto.QuestionDto = questionsList;
        return quizDto;
    }
}