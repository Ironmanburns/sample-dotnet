using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SampleDotnet.Tests;

/// <summary>
/// In-process contract test for the sample-dotnet health app, using
/// WebApplicationFactory against the real Program (which exposes a public
/// partial class). Asserts /healthz and /version per STANDARD.md.
/// </summary>
public class ContractTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContractTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Healthz_Returns_Ok()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/healthz");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        Assert.Equal("ok", doc.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Version_Reports_Service()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/version");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        Assert.Equal("sample-dotnet", doc.RootElement.GetProperty("service").GetString());
        // version is present and non-empty (assembly version or APP_VERSION)
        Assert.False(string.IsNullOrEmpty(doc.RootElement.GetProperty("version").GetString()));
    }
}
