using Server;
using Server.Data;
using System;

MongoDbCommand.Init();
Console.WriteLine("数据库已初始化");
HoldListManager.Init();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddAntDesign();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.MaximumReceiveMessageSize = null;
}).AddNewtonsoftJsonProtocol();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHub<TouHouHub>("/TouHouHub");
app.Urls.Add("http://localhost:495");
Console.WriteLine("已载入回应中心");
Console.WriteLine("服务端已启动");
Task.Run(() =>
{
    while (true)
    {
        Console.ReadLine();
        //RoomManager.Rooms.ForEach(room => Console.WriteLine(room.Summary.ToJson()));
        RoomManager.Rooms.FirstOrDefault()?.Summary.UploadAgentSummary(0,0);
        Console.WriteLine("上传完毕");
    }
});
Timer timer = new Timer(new TimerCallback((o) => HoldListManager.Match()));
timer.Change(0, 5000);
app.Run();
