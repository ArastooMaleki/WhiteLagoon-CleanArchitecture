using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Tests
{
    public class VillaNumberIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
    {
        private readonly CustomWebAppFactory<Program> _factory;
        private readonly HttpClient _client;

        public VillaNumberIntegrationTests(CustomWebAppFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Create_ShouldWorkProperly()
        {
            //Arrange 
            var database = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var temp = database.VillaNumbers.FirstOrDefault(v => v.Villa_Number == -1);
            temp.Should().BeNull();
            var newVilla = new VillaNumber() {Villa_Number = 10 , VillaId = 1,SpecialDetails="k" };
            var content = new StringContent(JsonConvert.SerializeObject(newVilla),Encoding.UTF8,"application/json");
            //Act 
            var result = await _client.PostAsync("/VillasController/VillaNumber/Create", content);
            //Assert 
            result.EnsureSuccessStatusCode();
            var afterCreation = database.VillaNumbers.FirstOrDefault(v => v.Villa_Number == 10);
            afterCreation.Should().NotBeNull();
        }
    }
}
