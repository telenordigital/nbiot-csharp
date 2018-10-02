# nbiot-csharp
NBIoT-CSharp provides a C# client for the REST API for Telenor NB-IoT.

## Configuration

The configuration file is located at `${HOME}/.telenor-nbiot`. The file is a simple
list of key/value pairs. Additional values are ignored. Comments must start
with a `#`:

    #
    # This is the URL of the Telenor NB-IoT REST API. The default value is
    # https://api.nbiot.telenor.io and can usually be omitted.
    address=https://api.nbiot.telenor.io

    #
    # This is the API token. Create new token by logging in to the Telenor NB-IoT
    # front-end at https://nbiot.engineering and create a new token there.
    token=<your api token goes here>


The configuration file settings can be overridden by setting the environment
variables `TELENOR_NBIOT_ADDRESS` and `TELENOR_NBIOT_TOKEN`. If you only use environment variables
the configuration file can be ignored.  Finally, there is a Client constructor that
accepts the address and token directly.

## Device Output

Support for receiving device-sent data via WebSockets is not yet implemented.
We are open to recommendations for functioning cross-platform WebSocket libraries.
