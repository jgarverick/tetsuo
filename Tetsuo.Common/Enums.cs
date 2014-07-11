namespace Tetsuo.Common
{
    public enum ReslovedEnpointTypes
    {
        Gateway = 0,
        Hub,
        Service
    }

    public enum TransmissionStatusCodes
    {
        EOT_GOOD,
        EOT_ERR,
        SVC_START,
        SVC_STOP,
        ROUTE,
        PARK,
        RESUME,
        BEGIN,
    }

    public enum InstrumentationSources
    {
        System,
        Gateway,
        Hub,
        Spoke,
    }
}