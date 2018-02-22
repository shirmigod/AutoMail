﻿using Automail.Api.Extensions;
using MimeKit;

namespace Automail.Api.Dtos.Requests
{
    public class SendMailRequest
    {
        /// <summary>
        /// Sender email adress.
        /// </summary>
        public string From { get; set; }

        public string FromName { get; set; }
        
        /// <summary>
        /// Recipient email address(es).
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Cc email address(es).
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// Email subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Email body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Define if mail is html.
        /// </summary>
        public bool IsHtml { get; set; }
    }

    public static class SendMailRequestExtensions
    {
        public static MimeMessage ToMimeMessage(this SendMailRequest dto, AppSettings settings)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(dto.FromName ?? dto.From ?? settings.Smtp.User, dto.From ?? settings.Smtp.User));
            emailMessage.To.AddAdresses(dto.To);
            emailMessage.Cc.AddAdresses(dto.Cc);
            emailMessage.Subject = dto.Subject;
            emailMessage.Body = new TextPart(dto.IsHtml ? "html" : "plain") { Text = dto.Body };
            return emailMessage;
        }

        public static bool IsValid(this SendMailRequest dto, AppSettings settings)
        {
            return !string.IsNullOrEmpty(dto.To) && (!string.IsNullOrEmpty(dto.From) || !string.IsNullOrEmpty(settings.Smtp.User));
        }
    }
}