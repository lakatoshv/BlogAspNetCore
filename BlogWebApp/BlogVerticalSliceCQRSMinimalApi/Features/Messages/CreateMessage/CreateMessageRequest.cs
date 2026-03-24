namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.CreateMessage;

public class CreateMessageRequest(string SenderId, string RecipientId, string SenderEmail, string SenderName,
    string Subject, string Body, int MessageType);
