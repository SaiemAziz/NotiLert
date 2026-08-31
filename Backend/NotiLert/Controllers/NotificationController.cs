using Microsoft.AspNetCore.Mvc;
using NotiLert.DTOs;
using NotiLert.Services;

namespace NotiLert.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send-email-broadcast")]
    public async Task<IActionResult> SendBroadcast([FromBody] SendNotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message content cannot be empty.");

        int sentCount = await _notificationService.BroadcastAsync(request.Subject, request.Message);

        if (sentCount == 0)
            return NotFound("No active recipients found.");

        return Ok(new { Count = sentCount, Status = "Sent successfully." });
    }
}
