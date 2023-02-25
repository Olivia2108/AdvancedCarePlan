
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI;
using Xunit;

namespace CarePlanUnitTest
{ 
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{

		}
		[Fact]
        public async void Test2()
        {
			var webfactory = new WebApplicationFactory<Program>();
			var httpclient = webfactory.CreateDefaultClient();
			var response = await httpclient.GetAsync("");
			var result = await response.Content.ReadAsStringAsync();

			result.Should().BeSameAs("");

        }
    }
}