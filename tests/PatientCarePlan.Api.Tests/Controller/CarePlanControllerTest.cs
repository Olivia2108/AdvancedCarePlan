using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;
using FluentAssertions;
using Moq;
using Application.Common.Interfaces.IServices;
using WebAPI.Controllers;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PatientCarePlan.Api.Tests.Controller
{
    public class CarePlanControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPatientCarePlanService> _serviceMock;
        private readonly CarePlanController _sut;

        public CarePlanControllerTest()
        {
            _fixture= new Fixture();
            _serviceMock = _fixture.Freeze<Mock<IPatientCarePlanService>>();
            _sut = new CarePlanController(_serviceMock.Object);
        }


        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange

            var responseObj = _fixture.Create<ResponseVM>();
           

            _serviceMock.Setup(x => x.GetAllPatientCarePlans())
                .ReturnsAsync(responseObj);

            //Act
            var result = await _sut.GetAllPatientCarePlans().ConfigureAwait(false);

            //Assert

            Assert.NotNull(result);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value
                .Should().NotBeNull()
                .And
                .BeOfType(responseObj.GetType());

            result.Should().BeAssignableTo<ActionResult<ResponseVM>>();
            result.Should().BeAssignableTo<IEnumerable<PatientCarePlanVM>>();

            _serviceMock.Verify(x => x.GetAllPatientCarePlans(), Times.Once());
        }


        [Fact]
        public void GetAllPatientsCarePlan_ShouldReturnNotFound_WhenDataNotFound()
        {

        }
    }
}
