using MissionToMars.Domain.Enums;

namespace MissionToMars.Domain.Results;

public sealed class DispatchResult
{
    public DispatchResult(DispatchStatus status, string? failureReason = null)
    {
        Status = status;
        FailureReason = failureReason;
    }

    public DispatchStatus Status { get; }

    public string? FailureReason { get; }

    public bool IsAccepted => Status == DispatchStatus.AcceptedForDelivery;

    public static DispatchResult Accepted() => new(DispatchStatus.AcceptedForDelivery);

    public static DispatchResult Rejected(string reason) => new(DispatchStatus.Rejected, reason);
}