namespace Core5.Conventions
{
    using System;
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region MessageConventions

            var conventions = busConfiguration.Conventions();
            conventions.DefiningCommandsAs(
                type =>
                {
                    return type.Namespace == "MyNamespace.Messages.Commands";
                });
            conventions.DefiningEventsAs(
                type =>
                {
                    return type.Namespace == "MyNamespace.Messages.Events";
                });
            conventions.DefiningMessagesAs(
                type =>
                {
                    return type.Namespace == "MyNamespace.Messages";
                });
            conventions.DefiningDataBusPropertiesAs(
                property =>
                {
                    return property.Name.EndsWith("DataBus");
                });
            conventions.DefiningExpressMessagesAs(
                type =>
                {
                    return type.Name.EndsWith("Express");
                });
            conventions.DefiningTimeToBeReceivedAs(
                type =>
                {
                    if (type.Name.EndsWith("Expires"))
                    {
                        return TimeSpan.FromSeconds(30);
                    }
                    return TimeSpan.MaxValue;
                });

            #endregion
        }
    }
}