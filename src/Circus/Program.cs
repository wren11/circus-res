using Circus.Services.Abstraction;
using Circus.Services.Hosted;
using Circus.Services.Hubs;
using Circus.Shared.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenAiSettings>(builder.Configuration.GetSection("OpenAi"));

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();

builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
builder.Services.AddSingleton<IOpenAiService, OpenAiService>();

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>((serviceProvider, client) =>
{
    var openAiSettings = serviceProvider.GetRequiredService<IOptions<OpenAiSettings>>().Value;
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiSettings.AccessToken);
    client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v2");
});
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost3000");
app.UseAuthorization();

app.MapControllers();
app.MapHub<UploadProgressHub>("/uploadProgressHub");

app.Run();
