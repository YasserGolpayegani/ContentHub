using ContentHub.Application.Abstractions.Authentication;
using ContentHub.Application.Abstractions.Messaging;
using ContentHub.Application.Abstractions.Persistence;
using ContentHub.Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Behaviors;

public sealed class AuditBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IAuditRepository _auditRepository;
    private readonly ICurrentUserService _currentUserService;

    public AuditBehavior(
        IAuditRepository auditRepository,
        ICurrentUserService currentUserService)
    {
        _auditRepository = auditRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IAuditableCommand auditableCommand)
        {
            return await next();
        }

        try
        {
            TResponse response = await next();

            await AddAuditAsync(auditableCommand, true, null, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            await AddAuditAsync(auditableCommand, false, ex.Message, cancellationToken);

            throw;
        }
    }

    private Task AddAuditAsync(
        IAuditableCommand command,
        bool isSuccessful,
        string? failureReason,
        CancellationToken cancellationToken)
    {
        var audit = new AuditLog(
            _currentUserService.UserId,
            _currentUserService.UserName,
            command.Action,
            command.EntityName,
            command.EntityId,
            DateTime.UtcNow,
            isSuccessful,
            failureReason,
            _currentUserService.IpAddress);

        return _auditRepository.AddAsync(
            audit,
            cancellationToken);
    }
}
