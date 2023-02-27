using Application.Common.Interfaces.IRepository;
using Application.Common.Interfaces.IServices;
using Application.Dto;
using Application.ViewModels;
using AutoFixture;
using Domain.Constants;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Newtonsoft.Json;
using static Domain.Enums.GeneralEnums;

namespace PatientCarePlan.Infrastructure.Tests.Services
{
    public class PatientCarePlanServiceTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPatientCarePlanRepository> _repositoryMock; 
        private readonly PatientCarePlanService _sut; 
        private readonly PatientCarePlanDto _modelWithCompletedSetToTrue; 

        public PatientCarePlanServiceTest()
        {
            _fixture = new Fixture();
            _repositoryMock = _fixture.Freeze<Mock<IPatientCarePlanRepository>>();
            _sut = new PatientCarePlanService(_repositoryMock.Object); 
            var th =  DbInitializer.GenerateData(100);

            switch (_modelWithCompletedSetToTrue)
            {
                case var model when model == null:

                    var json = JsonConvert.SerializeObject(th.Where(x => x.Completed).FirstOrDefault());
                    _modelWithCompletedSetToTrue = JsonConvert.DeserializeObject<PatientCarePlanDto>(json);

                    break;
            }
        }




        #region GetAll



        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldReturnData_WhenDataFound()
        {
            //Arrange 
            var repoResponse = _fixture.CreateMany<PatientCarePlanVM>(2); 
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = new List<PatientCarePlanVM>(),
                Message = ResponseConstants.Found
            };  
            _repositoryMock.Setup(x => x.GetAllPatientCarePlans())
                           .ReturnsAsync(repoResponse.ToList());

            //Act
            var result = await _sut.GetAllPatientCarePlans();


            //Assert  
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType()); 
            result.Should().BeAssignableTo<ResponseVM>();
              
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeAssignableTo<List<PatientCarePlanVM>>();
            result.Message.Should().BeEquivalentTo(ResponseConstants.Found);


            _repositoryMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }


        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldReturnNoData_WhenDataNotFound()
        {
            //Arrange 
            List<PatientCarePlanVM> repoResponse = new List<PatientCarePlanVM>();  
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.Found
            };

            _repositoryMock.Setup(x => x.GetAllPatientCarePlans())
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.GetAllPatientCarePlans().ConfigureAwait(false);


            //Assert  
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            result.Success.Should().BeTrue();
            result.Data.Should().BeNull(); 
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);

            _repositoryMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }



        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldOnNull_WhenExceptionThrown()
        {
            //Arrange 
            List<PatientCarePlanVM> repoResponse = null;
            var serviceResponse = new ResponseVM
            {
                Success = false,
                Data = null,
                Message = ErrorConstants.Error
            };


            _repositoryMock.Setup(x => x.GetAllPatientCarePlans())
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.GetAllPatientCarePlans();

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ErrorConstants.ServiceFetchError);

            _repositoryMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }




        #endregion


        #region GetById




        [Fact]
        public async Task GetPatientsCarePlanById_ShouldReturnData_WhenDataFound()
        {
            //Arrange 
            var repoResponse = _fixture.Create<PatientCarePlanVM>();
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = new PatientCarePlanVM(),
                Message = ResponseConstants.Found
            };


            var id = _fixture.Create<int>();


            _repositoryMock.Setup(x => x.GetCarePlanById(id))
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.GetCarePlanById(id);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.Found);

            _repositoryMock.Verify(x => x.GetCarePlanById(id), Times.Once());
        }




        [Fact]
        public async Task GetPatientsCarePlanById_ShouldReturnNoData_WhenDataNotFound()
        {
            //Arrange
            PatientCarePlanVM repoResponse = null;
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.NotFound
            };

            var id = _fixture.Create<int>();


            _repositoryMock.Setup(x => x.GetCarePlanById(id))
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.GetCarePlanById(id);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);


            _repositoryMock.Verify(x => x.GetCarePlanById(id), Times.Once());
        }



        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        public async Task GetPatientsCarePlanById_ShouldReturnNoData_WhenInputEquaulsOrLessThanZero(int testId)
        {
            //Arrange
            PatientCarePlanVM repoResponse = null;
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.NotFound
            };

            var id = testId;


            _repositoryMock.Setup(x => x.GetCarePlanById(id))
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.GetCarePlanById(id);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);


            _repositoryMock.Verify(x => x.GetCarePlanById(id), Times.Once());
        }



        #endregion


        #region Create
    
         
        [Fact]
        public async Task CreatePatientsCarePlan_ShouldReturnData_WhenInsertedSuccessfully()
        {
            //Arrange
             
            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;
             
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };

            repoRequest.ActualEndDate = repoRequest.ActualStartDate.AddYears(1);

            _repositoryMock.Setup(x => x.AddPatientCarePlan(It.IsAny<PatientCarePlans>()))
                                 .ReturnsAsync(repoResponse);

            _repositoryMock.Setup(x => x.IsUsernameExist(It.IsAny<string>())).ReturnsAsync(false);
             
             

            //Act
            var result = await _sut.AddPatientCarePlan(_modelWithCompletedSetToTrue);


            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.Saved);


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }



        [Fact]
        public async Task CreatePatientsCarePlan_ShouldUsernameExists_WhenUsernameExists()
        {
            //Arrange 
            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;
             
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };

            repoRequest.ActualEndDate = repoRequest.ActualStartDate.AddYears(1);

            _repositoryMock.Setup(x => x.AddPatientCarePlan(It.IsAny<PatientCarePlans>()))
                                 .ReturnsAsync(repoResponse);

            _repositoryMock.Setup(x => x.IsUsernameExist(It.IsAny<string>())).ReturnsAsync(true);
             

            //Act
            var result = await _sut.AddPatientCarePlan(_modelWithCompletedSetToTrue);


            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.IsExist);


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }

        [Fact]
        public async Task CreatePatientsCarePlan_ShouldReturnInvalidObject_WhenNullObjectisPassed()
        {
            //Arrange

            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;

            PatientCarePlanDto serviceRequest = null;
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };


            _repositoryMock.Setup(x => x.AddPatientCarePlan(repoRequest))
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.AddPatientCarePlan(serviceRequest);

            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeFalse();
            result.Data.Should().BeNull(); 


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }


        [Theory]
        [InlineData("Username", null)]
        [InlineData("Username", "")]
        [InlineData("PatientName", null)]
        [InlineData("PatientName", "")]
        [InlineData("Action", null)]
        [InlineData("Action", "")]
        [InlineData("Reason", null)]
        [InlineData("Reason", "")]
        [InlineData("Title", null)]
        [InlineData("Title", "")] 
        [InlineData("ActualStartDate", "0001-01-01 00:00:00.0000000")] 
        [InlineData("TargetStartDate", "0001-01-01 00:00:00.0000000")]
        public async Task CreatePatientsCarePlan_ShouldReturnRequiredFields_WhenRequiredFieldIsNotSupplied(string testCase, string value)
        {
            //Arrange
            
            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;

            var serviceRequest = _fixture.Create<PatientCarePlanDto>();
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };
            switch (testCase)
            {
                case "Username":
                    serviceRequest.UserName = value;
                    break;

                case "PatientName":
                    serviceRequest.PatientName = value;
                    break;

                case "Action":
                    serviceRequest.Action = value;
                    break;

                case "Reason":
                    serviceRequest.Reason = value;
                    break;

                case "Title":
                    serviceRequest.Title = value;
                    break;

                case "ActualStartDate":
                    serviceRequest.ActualStartDate = Convert.ToDateTime(value);
                    break;

                case "TargetStartDate":
                    serviceRequest.TargetStartDate = Convert.ToDateTime(value);
                    break;
            }

            _repositoryMock.Setup(x => x.AddPatientCarePlan(repoRequest))
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.AddPatientCarePlan(serviceRequest);

            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeFalse();
            result.Data.Should().BeNull(); 


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }




        [Theory]
        [InlineData("CompletedYes_ParametaerPassed")]
        [InlineData("CompletedNo_ParametaerPassed")] 
        public async Task CreatePatientsCarePlan_ShouldReturnPass_WhenCompletedYesAndNoRequiredFieldIsPassed(string testCase)
        {
            //Arrange

            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;

            var serviceRequest = _fixture.Create<PatientCarePlanDto>();
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };
            switch (testCase)
            {
                case "CompletedYes_ParametaerPassed":
                    serviceRequest.Completed = true;
                    serviceRequest.Outcome = _fixture.Create<string>();
                    serviceRequest.ActualEndDate = serviceRequest.ActualStartDate.AddDays(30);
                    break;

                case "CompletedNo_ParametaerPassed":
                    serviceRequest.Completed = false;
                    serviceRequest.Outcome = null;
                    serviceRequest.ActualEndDate = Convert.ToDateTime("0001-01-01 00:00:00.0000000");
                    break;
 
            }

            _repositoryMock.Setup(x => x.AddPatientCarePlan(It.IsAny<PatientCarePlans>()))
                                 .ReturnsAsync(repoResponse); 

            //Act
            var result = await _sut.AddPatientCarePlan(serviceRequest);

            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }





        [Fact] 
        public async Task CreatePatientsCarePlan_ShouldReturnFail_WhenCompletedYesRequiredFieldIsNotPassed()
        {
            //Arrange

            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;

            var serviceRequest = _fixture.Create<PatientCarePlanDto>();
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };
             
            serviceRequest.Completed = true;
            serviceRequest.Outcome = null;
            serviceRequest.ActualEndDate = Convert.ToDateTime("0001-01-01 00:00:00.0000000");


            _repositoryMock.Setup(x => x.AddPatientCarePlan(It.IsAny<PatientCarePlans>()))
                                         .ReturnsAsync(repoResponse);


            //Act
            var result = await _sut.AddPatientCarePlan(serviceRequest);

            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }

        


        [Fact] 
        public async Task CreatePatientsCarePlan_ShouldReturnFail_WhenEndDateIsLesserThanStartDate()
        {
            //Arrange

            var repoRequest = _fixture.Create<PatientCarePlans>();
            long repoResponse = 2;

            var serviceRequest = _fixture.Create<PatientCarePlanDto>();
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };
             

            serviceRequest.Completed = true;
            serviceRequest.Outcome = "Outcome";
            serviceRequest.ActualEndDate = Convert.ToDateTime("0001-01-01 00:00:00.0000000");


            _repositoryMock.Setup(x => x.AddPatientCarePlan(It.IsAny<PatientCarePlans>()))
                                         .ReturnsAsync(repoResponse);


            //Act
            var result = await _sut.AddPatientCarePlan(serviceRequest);

            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.EndDateInvalid);


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }


        #endregion



        #region Delete



        [Fact]
        public async Task DeletePatientsCarePlanById_ShouldReturnData_WhenRecordIsDeleted()
        {

            //Arrange

            var id = _fixture.Create<int>();
            var ip = _fixture.Create<string>();
            var repoResponse = 2;
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Deleted
            };
             

            _repositoryMock.Setup(x => x.DeleteCarePlanById(id, ip))
            .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.DeleteCarePlanById(id, ip);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().NotBeEquivalentTo((int)DbInfo.NoIdFound);
            result.Message.Should().BeEquivalentTo(ResponseConstants.Deleted);

        }
        

        [Fact]
        public async Task DeletePatientsCarePlanById_ShouldReturnWrongId_WhenRecordIsNotDeleted()
        {

            //Arrange

            var id = _fixture.Create<int>();
            var ip = _fixture.Create<string>();
            var repoResponse = (int)DbInfo.NoIdFound;
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.NotDeleted
            };


            _repositoryMock.Setup(x => x.DeleteCarePlanById(id, ip))
            .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.DeleteCarePlanById(id, ip);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            result.Success.Should().BeTrue();
            result.Data.Should().BeNull(); 
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotDeleted);

        }




        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        public async Task DeletePatientsCarePlanById_ShouldReturnNoData_WhenInputEquaulsOrLessThanZero(int testId)
        {
            //Arrange
            var repoResponse = (int)DbInfo.NoIdFound;
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = null,
                Message = ResponseConstants.NotFound
            };

            var id = testId;
            var ip = _fixture.Create<string>();

             

            _repositoryMock.Setup(x => x.DeleteCarePlanById(id, ip))
                .ReturnsAsync(repoResponse);

            //Act
            var result = await _sut.DeleteCarePlanById(id, ip);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotDeleted);


            _repositoryMock.Verify(x => x.DeleteCarePlanById(id, ip), Times.Once());
        }


        #endregion


        #region Update


        [Fact]
        public async Task UpdatePatientsCarePlanById_ShouldReturnSuccess_WhenRecordIsUpdated()
        {

            //Arrange 
            var id = _fixture.Create<int>();
            int repoResponse = 2;
             
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Updated
            };


            _repositoryMock.Setup(x => x.UpdateCarePlanById(It.IsAny<long>(), It.IsAny<PatientCarePlans>()))
                                         .Returns(Task.FromResult(repoResponse));
               

            //Act
            var result = await _sut.UpdateCarePlanById(_modelWithCompletedSetToTrue, id);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.Updated);

        }



        [Fact]
        public async Task UpdatePatientsCarePlan_ShouldReturnFail_WhenEndDateIsLesserThanStartDate()
        {
            //Arrange

            var repoRequest = _fixture.Create<PatientCarePlans>();
            int repoResponse = 2;

            var serviceRequest = _fixture.Create<PatientCarePlanDto>();
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.Saved
            };


            var id = _fixture.Create<int>();
            serviceRequest.Completed = true;
            serviceRequest.Outcome = "Outcome";
            serviceRequest.ActualEndDate = Convert.ToDateTime("0001-01-01 00:00:00.0000000");


            _repositoryMock.Setup(x => x.UpdateCarePlanById(It.IsAny<long>(), It.IsAny<PatientCarePlans>()))
                                         .Returns(Task.FromResult(repoResponse));


            //Act
            var result = await _sut.UpdateCarePlanById( serviceRequest, id);

            //Assert
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());

            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.EndDateInvalid);


            _repositoryMock.Verify(x => x.AddPatientCarePlan(repoRequest), Times.Never());
        }



        [Fact]
        public async Task UpdatePatientsCarePlanById_ShouldReturnNoData_WhenDataNotFound()
        {
            //Arrange 
            var repoRequest = _fixture.Create<PatientCarePlans>();
            var id = _fixture.Create<int>();
            var repoResponse = (int)DbInfo.NoIdFound;
             
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.NotUpdated
            };



            _repositoryMock.Setup(x => x.UpdateCarePlanById(It.IsNotIn<long>(), It.IsAny<PatientCarePlans>()))
                                         .Returns(Task.FromResult(repoResponse));
             

            //Act
            var result = await _sut.UpdateCarePlanById(_modelWithCompletedSetToTrue, id);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);


            _repositoryMock.Verify(x => x.UpdateCarePlanById(id, repoRequest), Times.Never());
        }



        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public async Task UpdatePatientsCarePlanById_ShouldReturnNoData_WhenInputEquaulsOrLessThanZero(int testId)
        {
            //Arrange
            var repoRequest = _fixture.Create<PatientCarePlans>(); 
            var repoResponse = (int)DbInfo.NoIdFound;
             
            var serviceResponse = new ResponseVM
            {
                Success = true,
                Data = 2,
                Message = ResponseConstants.NotUpdated
            }; 

            var id = testId;

            _repositoryMock.Setup(x => x.UpdateCarePlanById(It.IsNotIn<long>(), It.IsAny<PatientCarePlans>()))
                                         .Returns(Task.FromResult(repoResponse));

            //Act
            var result = await _sut.UpdateCarePlanById(_modelWithCompletedSetToTrue, id);

            //Assert 
            result.Should().BeAssignableTo<ResponseVM>();
            result.Should()
                .NotBeNull()
                .And
                .BeOfType(serviceResponse.GetType());


            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
            result.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);


            _repositoryMock.Verify(x => x.UpdateCarePlanById(id, repoRequest), Times.Never());
        }




        #endregion
    }
}
