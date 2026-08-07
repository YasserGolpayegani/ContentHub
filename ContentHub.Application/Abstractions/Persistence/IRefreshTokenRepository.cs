using ContentHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentHub.Application.Abstractions.Persistence;

public interface IRefreshTokenRepository
{
    Task<List<RefreshToken>> GetByUserId(Guid id, CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    void Update(RefreshToken refreshToken);    
}
