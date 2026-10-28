using Microsoft.EntityFrameworkCore;
using NotiLert.Application;
using NotiLert.Application.Notifications.DTOs;
using NotiLert.Domain.Entities;
using NotiLert.Infrastructure;
using NotiLert.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
   .WithName("HealthCheck");

var notificationsApi = app.MapGroup("/api/notifications");

notificationsApi.MapPost("/", async (CreateNotificationRequest request, NotiLertDbContext db) =>
{
    var notification = Notification.Create(
        request.Recipient,
        request.Subject,
        request.Body,
        request.Channel);

    db.Notifications.Add(notification);
    await db.SaveChangesAsync();

    return Results.Created($"/api/notifications/{notification.Id}", NotificationResponse.FromEntity(notification));
})
.WithName("CreateNotification");

notificationsApi.MapGet("/", async (NotiLertDbContext db) =>
{
    var notifications = await db.Notifications
        .OrderByDescending(n => n.CreatedAtUtc)
        .Select(n => NotificationResponse.FromEntity(n))
        .ToListAsync();

    return Results.Ok(notifications);
})
.WithName("GetNotifications");

notificationsApi.MapGet("/{id:guid}", async (Guid id, NotiLertDbContext db) =>
{
    var notification = await db.Notifications.FindAsync(id);
    return notification is not null
        ? Results.Ok(NotificationResponse.FromEntity(notification))
        : Results.NotFound();
})
.WithName("GetNotificationById");

app.Run();
