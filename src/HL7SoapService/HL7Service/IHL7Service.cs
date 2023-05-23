namespace Contoso
{
    [ServiceContract]
    public interface IHL7Service
    {
        [OperationContract]
        string ProcessHl7Message(string message);
    }
}
