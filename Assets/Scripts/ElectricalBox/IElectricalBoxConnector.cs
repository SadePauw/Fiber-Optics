public interface IElectricalBoxConnector
{
    public void SetConnectedBoxFrom(ElectricalBoxPower box);

    public void SetConnectedBoxTo(ElectricalBoxPower box);

    public void ConnectToRouter(Router router);

    public void ClearConnections();
}
