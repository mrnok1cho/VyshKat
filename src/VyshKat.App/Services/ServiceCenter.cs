using VyshKat.App.Domain.Contracts;

namespace VyshKat.App.Services;

public sealed class ServiceCenter : IServiceCenter
{
    public bool Approve(ITransport transport) => transport.IsTechnicallySound;
}
