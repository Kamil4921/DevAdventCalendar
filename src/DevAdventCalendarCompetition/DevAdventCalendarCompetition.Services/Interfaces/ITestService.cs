using System;
using System.Collections.Generic;
using DevAdventCalendarCompetition.Services.Models;

namespace DevAdventCalendarCompetition.Services.Interfaces
{
    public interface ITestService
    {
        TestDto GetTestByNumber(int testNumber);

        bool HasUserAnsweredTest(string userId, int testNumber);

        void AddTestAnswer(int testId, string userId, DateTime testStartDate);

        UserTestCorrectAnswerDto GetAnswerByTestId(int testId);

        void AddTestWrongAnswer(string userId, int testId, string wrongAnswer, DateTime wrongAnswerDate);

        bool VerifyTestAnswer(string userAnswer, IEnumerable<string> correctAnswers);
    }
}