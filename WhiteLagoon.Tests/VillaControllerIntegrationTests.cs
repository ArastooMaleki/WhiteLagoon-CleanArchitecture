using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using AutoMapper;
using WhiteLagoon.Web.AutoMapper;
using WhiteLagoon.Web.Models;

namespace WhiteLagoon.Tests
{
    public class VillaControllerIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebAppFactory<Program> _factory;

        public VillaControllerIntegrationTests(CustomWebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
            this._factory = factory;
        }
        [Theory]
        [InlineData("/Villa")]
        [InlineData("/Home")]
        [InlineData("/Home/Privacy")]
        [InlineData("/Home/Index")]
        [InlineData("/Villa/Index")]
        public async Task IndexAsync_ShouldReturnView(string url)
        {
            HttpResponseMessage response =  await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            /*var temp = response.Content.Headers.ContentType.ToString();
            Assert.Equal("text/html; charset=utf-8", temp);*/
            response.Content.Headers.ContentType.ToString().Should().BeEquivalentTo("text/html; charset=utf-8");
        }
        [Fact]
        public async Task DeleteAsync_SouldDelteFromDatbaseCorrectly()
        {
            IServiceScope scope = _factory.Services.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Villas.AddAsync(new Villa() { Name= "DeleteAsync_SouldDelteFromDatbaseCorrectly",Description="helloDis"});
            await dbContext.SaveChangesAsync();
            dbContext.Villas.Where(v => v.Name == "DeleteAsync_SouldDelteFromDatbaseCorrectly").FirstOrDefault().Should().NotBeNull();
            int id = dbContext.Villas.Where(v => v.Name == "DeleteAsync_SouldDelteFromDatbaseCorrectly").FirstOrDefault().Id;
            HttpClient client = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect=false, BaseAddress = new Uri("https://localhost") });
            var res = await client.DeleteAsync($"/Villa/Delete/{id}");
            Villa? result = dbContext.Villas.FirstOrDefault(v => v.Id == id);
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProperly()
        {
            IServiceScope scope = _factory.Services.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IMapper mapper = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            }).CreateMapper();
            var oldVilla = dbContext.Villas.AsNoTracking().FirstOrDefault(v => v.Id == 1);
            var price = 156;
            var description = "test description";
            var name = "test name";
            oldVilla.Name = name; 
            oldVilla.Description = description; 
            oldVilla.Price = price;
            var content = new StringContent(JsonConvert.SerializeObject(mapper.Map<VillaUpdateViewModel>(oldVilla))
                ,Encoding.UTF8, "application/json");
            await _client.PutAsync("/Villa/Update" ,content);
            var newVilla = dbContext.Villas.FirstOrDefault(v => v.Id == 1);
            Assert.Equal(name,newVilla.Name);
            Assert.Equal(description,newVilla.Description);
            Assert.Equal(price,newVilla.Price);
        }
        [Fact]
        public async Task CreateAsync_ShouldCreateVillaSuccessfully()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var villBeforeCreation = database.Villas.FirstOrDefault(v=>v.Name=="test Create async");
            villBeforeCreation.Should().BeNull() ;
            var content = new StringContent(JsonConvert.SerializeObject
                (new VillaCreateViewModel() { Name= "test Create async" ,Description="thossodigaeio;gnja;eoirnfinhgv"})
                ,Encoding.UTF8,"application/json");
            await _client.PostAsync("/Villa/Create",content);
            var villafterCreation = database.Villas.FirstOrDefault(v => v.Name == "test Create async");
            villafterCreation.Should().NotBeNull() ;
        }
    }
}
