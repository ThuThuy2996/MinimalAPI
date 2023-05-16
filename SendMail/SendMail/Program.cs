using Microsoft.AspNetCore.Mvc;
using SendMailApi.Provider;
using SendMailApi.Commons.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<EmailHostedService>();
builder.Services.AddHostedService(provider => provider.GetService<EmailHostedService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/send-email", async (EmailHostedService emailHostedService) =>
{
    await SendMail(emailHostedService);
    return Results.Ok();
}).WithName("SendEMail");

app.MapPost("/send-email-tattachment", async ([FromBody] EmailModel emailModel, EmailHostedService emailHostedService) =>
{
   bool rs = await SendMailWithAttachment(emailModel,emailHostedService);
   
    return rs ? Results.Ok() : Results.BadRequest();
}).WithName("SendEMailAttachment");

app.Run();

async Task SendMail(EmailHostedService emailHostedService)
{
    await emailHostedService.SendEmailAsync(new EmailModel
    {
        EmailAddress = "thuyntt04@ominext.com",
        Subject = "Test Send Email Using Minimal API",
        Body = "Minimal API là một tính năng mới trong ASP.NET Core 6, cung cấp cách để định nghĩa các API đơn giản hơn và linh hoạt hơn mà không cần phải sử dụng các thành phần truyền thống của ASP.NET Core như Controllers, Actions, Routes, Attributes, Middlewares, Dependency Injection,... \r\n" +

            "Với Minimal API, bạn có thể định nghĩa một API bằng cách sử dụng các phương thức tiện ích như MapGet(), MapPost(), MapPut(),... và xử lý các yêu cầu đến bằng cách truy cập trực tiếp các đối tượng HttpRequest và HttpResponse.Điều này giúp tăng hiệu suất và giảm thiểu số lượng mã cần phải viết." +
            "Minimal API còn có khả năng kết hợp với Swagger và OpenAPI để tạo tài liệu API, và hỗ trợ tích hợp với giao thức WebSocket."
    });
}
async Task<bool> SendMailWithAttachment(EmailModel emailModel,EmailHostedService emailHostedService)
{
    return await emailHostedService.SendEmailAsync(emailModel);   
}

