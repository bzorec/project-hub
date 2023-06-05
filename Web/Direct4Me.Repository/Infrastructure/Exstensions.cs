using Direct4Me.Repository.Entities;

namespace Direct4Me.Repository.Infrastructure;

public static class Exstensions
{
    public static PostboxEntity UpdateUnlockCount(this PostboxEntity model, string type)
    {
        model.StatisticsEntity.LastModified = DateTime.Now;
        switch (type)
        {
            case "NFC":
                model.StatisticsEntity.NfcUnlock++;
                break;
            case "QR":
                model.StatisticsEntity.QrCodeUnlock++;
                break;
            default:
                model.StatisticsEntity.QrCodeUnlock++;
                break;
        }

        return model;
    }
}