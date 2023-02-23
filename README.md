_**NOTE:** I've created a new project over [here](https://github.com/danwale/SolarUseOptimiser) for any developers interested in creating their own integrations with other solar systems, it's based on this but I've decoupled it quite a bit so that you can easily implement an IDataSource and configure it as a route for data to be retrived from and pushed onto ChargeHQ (or another target if you want to implement the IDataTarget interface)_. Added a few extra brands like Growatt and IoTaWatt as examples of how it can be extended. **I will also keep this project active and updated as it shares most of the same code.**

# Huawei Fusion Solar to ChargeHQ Push API Integration Module

[![Docker Image CI](https://github.com/danwale/HuaweiFusionSolar-ChargeHQ-Integration/actions/workflows/docker-image.yml/badge.svg?branch=main)](https://github.com/danwale/HuaweiFusionSolar-ChargeHQ-Integration/actions/workflows/docker-image.yml)

[![Donate PayPal](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/danwale?country.x=AU&locale.x=en_AU) <a href="https://www.buymeacoffee.com/danwale" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" alt="Buy Me A Coffee" style="height: 20px !important;width: 105px !important;" ></a>

The intention of this integration is to poll the Huawei Fusion Solar API to determine how much solar power is being generated and provide this to ChargeHQ's Push API that will enable ChargeHQ to regulate the charge rate of a Tesla car to maximise the use of solar energy rather than grid supplied power.



If you have power sensor devices or grid meter devices, and/or Huawei batteries in your solar setup it is able to pull data from those devices to help send more accurate information onto ChargeHQ to make more informed decisions on the charging. If a Power Sensor exists it will use that over a Grid Meter, if you wish to just use the inverters production data and manage the buffer of "excess" solar power the premises needs you can turn off the extra data sources with toggles in the settings (they default to being enabled with a value of true). This extra functionality hasn't been tested extensively by myself because I don't have these devices in my setup but I've looked at the values from a user for a number of the device types and think this should work, let me know if it doesn't work for you and we can work together to get it fixed. There is no need to turn off the functionality if you don't have the devices, the software is smart enough to detect the devices the system has available to it.

This service is only needed while ChargeHQ doesn't support Huawei inverters directly, it's on the roadmap for their service so one day this might not be needed.

**Of course if you really like this service and want to thank me a donation would be much appreciated, see the various donation method buttons above.**

## How to get a Huawei Username and Password

In late 2022 Huawei changed the process for getting access to the API's, it now requires you to request an API account from the company that installed your Huawei solar system as the installer now has the responsiblity to create what they call a Northbound API Account that would be used. The limitation on the Huawei system is each installer company can only create a maximum of 5 of these Northbound API Accounts as it's intended that they are used by an integration point rather than individual users. To faciliate this shortcoming it's best to ask the installer to create a Northbound API Account for their entire company that will be shared by all users, this software will only pull the information relevant to the specific station/plant that is named in the configuration.

I'll prepare a document detailing how to setup a Northbound API Account for sending onto your installer in case they haven't seen this new functionality before and help guide them through the process. Things they'll need to setup for their account are:

- **System Name**: This is a globally unique name for the Northbound API Account being created, I suggest they include their company name initials in the name to help achieve this, e.g. if the company was called _**The Best Solar Firm**_ they'd use the System Name _**TBSF_ChargeHQ_Poller**_. If the name is taken when they try to save/create the new account it will show a dialog informing them that the System Name is already in use.
- **Username**: Again this is globally unique so they should use a similar technique for naming the username, using the sample company name above they might use _**TBSFChargeHQ**_.
- **Password**: This is the password for the account, this can be anything they pick but they need to be able to share it with their customers who ask them to use this account.
- **Account Expiry**: There is a mandatory field for the expiry of the Northbound API Account, I would suggest they pick a date that exceeds the expected lifetime of the systems that could be relying on it, setting it as 2199-12-31 23:59:59 would guarantee that it's effectively set to never end (_if your solar system lasts that long and your great great grand children are using it I'd be very surprised :)_)

The next step in the setup is to grant access to certain (or all) plants and what interfaces of the API and data points the account has access to. I would suggest that an installer would turn on access to all interfaces and data points and would add plants into the Northbound API Account as customers ask for the access to the API.

The installer would then provide the user with the **Username** and **Password** they created for the Northbound API Account and provide you with what your station/plant name is (though you can get this yourself from the FusionSolar Moblie App, if you click **Devices** in the app then select your inverter and then click **Basic Info** you'll get a screen that shows the **Plant name**)

## Configuration

Sample **docker-compose.yml**:

```yaml
version: "3"

services:
  huawei-solar:
    image: danwale/huaweifusionsolar2chargehq:latest
    restart: always
    environment:
      - HUAWEI__BASEURI= #insert the base URI for the FusionSolar API you're setup on e.g.: https://intl.fusionsolar.huawei.com/
      - HUAWEI__USERNAME= #insert the username Huawei or your installer have provided
      - HUAWEI__PASSWORD= #insert the system code/password Huawei or your installer have issued
      - HUAWEI__STATIONNAME= #insert the plant/station name here
      - HUAWEI__POLLRATE=5 #in minutes (Huawei have a limit of once every 5 minutes)
      - HUAWEI__USEPOWERSENSORDATA= #true or false value, if a power sensor is present should its data be collected and passed on
      - HUAWEI__USEGRIDMETERDATA= #true or false value, if a grid meter is present should its data be collected and passed on
      - HUAWEI__USEBATTERYDATA= #true or false value, if a battery is present should its data be collected and passed on
      - CHARGEHQ__APIKEY= #insert your ChargeHQ API Key here
      - SERILOG__MINIMUMLEVEL=Information # Use Debug if you want to see information on message payloads
      - TZ=Australia/Perth # If you want your logs to display in the servers local timezone rather than UTC set the TZ environment variable to suit
    volumes:
     - huaweisolar-conf:/etc/huaweisolar

volumes:
  huaweisolar-conf:

```

**Run the following command to run the service:**

```bash
docker-compose up -d
```

**If you want to watch the logs run the command:**

```bash
docker-compose logs --follow huawei-solar
```

_pressing Ctrl+C will end this command following the logs_
  
**To stop the service run:**

```bash
docker-compose stop huawei-solar
```

You can configure the entire service using environment variables as shown above, but if you want to use a configuration file, you can put an **appsettings.json** in the mapped volume and it will take effect, environment variables will override any equivalent settings in a file:

```json
{
    "Huawei": {
        "BaseURI": "https://intl.fusionsolar.huawei.com/",
        "Username":"<Username Here>",
        "Password": "<System Code/Password Here>",
        "StationName": "<Station Name Here>",
        "PollRate": 5,
        "UsePowerSensorData": true,
        "UseGridMeterData": true,
        "UseBatteryData": true
    },
    "ChargeHQ": {
        "PushURI": "https://api.chargehq.net/api/public/push-solar-data",
        "ApiKey": "<ChargeHQ API Key Here>"
    },
    "Serilog" : {
        "Using":  [ "Serilog.Sinks.Console" ],
        "MinimumLevel": "Information"
    }
}
```

Happy EV motoring, charging as cleanly (and cheaply) as possible!
