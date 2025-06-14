namespace ChatApplicationCoreANDReact.Models
{
    //public record MessageViewDTO(long Id, long SenderId, string SenderName, string ReceiverName, string Message, string Timestamp, bool IsSender)
    //{
    //    public MessageViewDTO(long Id, long SenderId, string SenderName, string ReceiverName, string Message, string Timestamp)
    //    {
    //        this.Id = Id;
    //        this.SenderId = SenderId;
    //        this.SenderName = SenderName;
    //        this.ReceiverName = ReceiverName;
    //        this.Message = Message;
    //        this.Timestamp = Timestamp;
    //    }
    //}


    public record MessageViewDTO(
    long Id,
    long SenderId,
    string SenderName,
    string ReceiverName,
    string Message,
    string Timestamp,
    bool IsSender
);
}
