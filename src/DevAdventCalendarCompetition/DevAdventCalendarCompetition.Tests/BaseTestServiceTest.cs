﻿using AutoMapper;
using DevAdventCalendarCompetition.Repository.Interfaces;
using DevAdventCalendarCompetition.Repository.Models;
using DevAdventCalendarCompetition.Services;
using DevAdventCalendarCompetition.Services.Models;
using DevAdventCalendarCompetition.Services.Profiles;
using Moq;
using System;
using Xunit;

namespace DevAdventCalendarCompetition.Tests
{
    public class BaseTestServiceTest
    {
        private readonly Mock<IBaseTestRepository> _baseTestRepositoryMock;
        private IMapper _mapper;

        private Test _oldTest = new Test()
        {
            Id = 1,
            Number = 1,
            StartDate = DateTime.Today.AddDays(-2),
            EndDate = DateTime.Today.AddDays(-1),
            Answers = null
        };

        private Test _futureTest = new Test()
        {
            Id = 1,
            Number = 1,
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(2),
            Answers = null
        };

        private Test _currentTest = new Test()
        {
            Id = 1,
            Number = 1,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Answers = null
        };

        private TestAnswer _testAnswer = new TestAnswer()
        {
            Id = 1,
            Test = new Test(),
            TestId = 1,
            User = new ApplicationUser(),
            UserId = "1",
            AnsweringTimeOffset = new TimeSpan(),
            AnsweringTime = DateTime.Today
        };

        public BaseTestServiceTest()
        {
            _baseTestRepositoryMock = new Mock<IBaseTestRepository>();
        }

        [Fact]
        public void GetTestByNumber_DontGetOldTest()
        {
            //Arrange
            _baseTestRepositoryMock.Setup(mock => mock.GetByNumber(It.IsAny<int>())).Returns(_oldTest);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<TestProfile>()).CreateMapper();
            var baseTestService = new BaseTestService(_baseTestRepositoryMock.Object, _mapper);
            //Act
            var result = baseTestService.GetTestByNumber(_oldTest.Number);
            //Assert
            Assert.Null(result);
            _baseTestRepositoryMock.Verify(mock => mock.GetByNumber(It.Is<int>(x=>x.Equals(_oldTest.Number))), Times.Once);
        }

        [Fact]
        public void GetTestByNumber_DontGetFutureTest()
        {
            //Arange
            _baseTestRepositoryMock.Setup(mock => mock.GetByNumber(It.IsAny<int>())).Returns(_futureTest);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<TestProfile>()).CreateMapper();
            var baseTestService = new BaseTestService(_baseTestRepositoryMock.Object, _mapper);
            //Act
            var result = baseTestService.GetTestByNumber(_futureTest.Number);
            //Assert
            Assert.Null(result);
            _baseTestRepositoryMock.Verify(mock => mock.GetByNumber(It.Is<int>(x=>x.Equals(_futureTest.Number))), Times.Once());
        }

        [Fact]
        public void GetTestByNumber_ReturnCurrentTestDto()
        {
            //Arrange
            _baseTestRepositoryMock.Setup(mock => mock.GetByNumber(It.IsAny<int>())).Returns(_currentTest);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<TestProfile>()).CreateMapper();
            var baseTestService = new BaseTestService(_baseTestRepositoryMock.Object, _mapper);
            //Act
            var result = baseTestService.GetTestByNumber(_currentTest.Number);
            //Assert
            Assert.IsType<TestDto>(result);
            _baseTestRepositoryMock.Verify(mock => mock.GetByNumber(It.Is<int>(x=>x.Equals(_currentTest.Number))), Times.Once());
        }

        [Fact]
        public void GetAnswerByTestId_ReturnTestAnswerDto()
        {
            //Arrange
            _baseTestRepositoryMock.Setup(mock => mock.GetAnswerByTestId(It.IsAny<int>())).Returns(_testAnswer);
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<TestAnswerProfile>()).CreateMapper();
            var baseTestService = new BaseTestService(_baseTestRepositoryMock.Object, _mapper);
            //Act
            var result = baseTestService.GetAnswerByTestId(_testAnswer.Id);
            //Assert
            Assert.IsType<TestAnswerDto>(result);
            _baseTestRepositoryMock.Verify(mock => mock.GetAnswerByTestId(It.Is<int>(x=>x.Equals(_testAnswer.Id))), Times.Once());
        }

        [Fact]
        public void AddTestAnswer()
        {
            //Arrange
            _baseTestRepositoryMock.Setup(mock => mock.AddAnswer(It.IsAny<TestAnswer>()));
            var baseTestService = new BaseTestService(_baseTestRepositoryMock.Object, _mapper);
            //Act
            baseTestService.AddTestAnswer(_testAnswer.TestId, _testAnswer.UserId, DateTime.Now);
            //Assert
            _baseTestRepositoryMock.Verify(mock => mock.AddAnswer(It.Is<TestAnswer>(x=>x.UserId == _testAnswer.UserId && x.TestId == _testAnswer.TestId)), Times.Once());
        }
    }
}