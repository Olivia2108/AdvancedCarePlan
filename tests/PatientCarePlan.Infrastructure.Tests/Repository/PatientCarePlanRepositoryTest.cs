using Application.Common.Interfaces.IDbContext;
using Application.Common.Interfaces.IServices;
using Application.Dto;
using Application.ViewModels;
using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using PatientCarePlan.Infrastructure.Tests.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.GeneralEnums;

namespace PatientCarePlan.Infrastructure.Tests.Repository
{
    public class PatientCarePlanRepositoryTest : MoqDbContextFixture
    {
        private readonly IFixture _fixture; 
        private readonly PatientCarePlanRepository _sutA;
        private readonly PatientCarePlanRepository _sut;

        public PatientCarePlanRepositoryTest()
        {
            _fixture = new Fixture(); 
            _sutA = new PatientCarePlanRepository(GetMock().Item3.Object);
            _sut = new PatientCarePlanRepository(_context);
        }


        #region GetAll


        [Fact]
        public async Task GetAllPatientsCarePlan_ShouldBeSuccessful()
        {
            //Arrange 
            var id = _fixture.Create<long>();


            //Act 
            var result = await _sut.GetAllPatientCarePlans();

            //Assert 
            result.Count().Should().NotBe(0);
            result.Count().Should().BeGreaterThan(0);


        }

        #endregion


        #region GetById


        [Fact]
        public async Task GetPatientsCarePlanById_ShouldBeSuccessful()
        {
            //Arrange   
            var first = await _context.PatientCarePlans.FirstOrDefaultAsync();

            //Act 
            var result = await _sut.GetCarePlanById(first.Id);

            //Assert

            result.Should().BeOfType<PatientCarePlanVM>(); 
            result.UserName.Should().NotBe(null); 


        }

        

        [Fact]
        public async Task GetPatientsCarePlanById_NotValid_ShouldNotBeSuccessful()
        {
            //Arrange   
            var wrongId = DbInitializer.GiveMeANumber(_context.PatientCarePlans.ToList());

            //Act 
            var result = await _sut.GetCarePlanById(wrongId);

            //Assert

            result.Should().BeNull();  


        }



        [Fact]
        public async Task GetPatientsCarePlanByUsername_ShouldBeSuccessful()
        {
            //Arrange 
            var first = await _context.PatientCarePlans.FirstOrDefaultAsync();

            //Act 
            var result = await _sut.IsUsernameExist(first.UserName);

            //Assert
             
            result.Should().BeTrue();


        }



        [Fact]
        public async Task GetPatientsCarePlanByUsername_ShouldNotBeSuccessful()
        {
            //Arrange 
            var id = _fixture.Create<long>();
             

            //Act 
            var result = await _sut.IsUsernameExist("UserName");

            //Assert
             
            result.Should().BeFalse();


        }



        #endregion


        #region Create 


         

        [Fact]
        public async Task CreatePatientsCarePlan_ShouldReturnData_WhenInsertedSuccessfully()
        {
            //Arrange 
            var repoRequest = _fixture.Create<PatientCarePlans>(); 


            //Act 
            var result = await _sut.AddPatientCarePlan(repoRequest);
             
            //Assert

            result.Should().NotBe(0); 
            result.Should().BeGreaterThan(0); 

             
        }



        #endregion



        #region Delete

        [Fact]
        public async Task DeletePatientsCarePlan_ShouldBeSuccessful_ForValidId()
        {
            //Arrange

            var first = await _context.PatientCarePlans.FirstOrDefaultAsync(); 
            var ip = _fixture.Create<string>();


            //Act 
            var result = await _sut.DeleteCarePlanById(first.Id, ip);

            //Assert

            result.Should().NotBe(0);
            result.Should().NotBe((int)DbInfo.NoIdFound);


        }

        [Fact]
        public async Task DeletePatientsCarePlan_ShouldNotBeSuccessful_ForInvalidId()
        {
            //Arrange 
            var wrongId = DbInitializer.GiveMeANumber(_context.PatientCarePlans.ToList()); 
            var ip = _fixture.Create<string>();


            //Act 
            var result = await _sut.DeleteCarePlanById(wrongId, ip);

            //Assert 
            result.Should().Be((int)DbInfo.NoIdFound);


        }


        #endregion


        #region Update


        [Fact]
        public async Task UpdatePatientsCarePlan_ShouldBeSuccessful_ForValidId()
        {
            //Arrange

            var first = await _context.PatientCarePlans.FirstOrDefaultAsync(); 


            //Act 
            var result = await _sut.UpdateCarePlanById(first.Id, first);

            //Assert

            result.Should().NotBe(0);
            result.Should().NotBe((int)DbInfo.NoIdFound);


        }

        [Fact]
        public async Task UpdatePatientsCarePlan_ShouldNotBeSuccessful_ForInvalidId()
        {
            //Arrange 
            var first = await _context.PatientCarePlans.FirstOrDefaultAsync();
            var wrongId = DbInitializer.GiveMeANumber(_context.PatientCarePlans.ToList()); 


            //Act 
            var result = await _sut.UpdateCarePlanById(wrongId, first);

            //Assert

            result.Should().NotBe(0);
            result.Should().Be((int)DbInfo.NoIdFound);


        }
        #endregion
    }
}
