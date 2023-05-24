using MyLibrary;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseHttpsRedirection();

app.MapPost("/api/getCountUniqueWords", async (HttpContext context) =>
{
    using StreamReader reader = new StreamReader(context.Request.Body);
    string text = await reader.ReadToEndAsync();
    return TextHelper.GetCountUniqueWordsThread(text);
});

app.MapGet("api", () => "Start WebAPI");

app.Run();

