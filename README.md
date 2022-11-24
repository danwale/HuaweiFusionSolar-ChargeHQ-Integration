# Huawei Fusion Solar to ChargeHQ Push API Integration Module

[![Docker Image CI](https://github.com/danwale/HuaweiFusionSolar-ChargeHQ-Integration/actions/workflows/docker-image.yml/badge.svg?branch=main)](https://github.com/danwale/HuaweiFusionSolar-ChargeHQ-Integration/actions/workflows/docker-image.yml)

[![Donate PayPal](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/danwale?country.x=AU&locale.x=en_AU) <a href="https://www.buymeacoffee.com/danwale" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" alt="Buy Me A Coffee" style="height: 20px !important;width: 105px !important;" ></a>

The intention of this integration is to poll the Huawei Fusion Solar API to determine how much solar power is being generated vs how much is being consumed from the grid and provide this to ChargeHQ's Push API that will enable ChargeHQ to regulate the charge rate of a Tesla car to maximise the use of solar energy rather than grid supplied power.

This service is only needed while ChargeHQ doesn't support Huawei inverters, it's on the roadmap for their service so one day this might not be needed.

It is in early development and testing with my own home setup for now, once initial testing is complete I'll open up the Docker repository for public access (it's private while I do early alpha version testing) and welcome any feedback on your experience. 
**Of course if you really like this service and want to thank me a donation would be much appreciated, see the various donation method buttons above.**

**NOTE**: The testing of the values calculated for grid consumption vs solar consumption is still required, I've added a feature toggle in the
settings (HUAWEI:SENDGRIDVALUES) to be able to turn this data on/off easily for testing which gives the desired effect.

Sample docker-compose.yml:

```yaml
version: "3"

services:
  huawei-solar:
    image: danwale/huaweifusionsolar2chargehq:latest
    restart: always
    environment:
      - HUAWEI__BASEURI= #insert the base URI for the FusionSolar API you're setup on e.g.: https://intl.fusionsolar.huawei.com/
      - HUAWEI__USERNAME= #insert the username Huawei have provided
      - HUAWEI__PASSWORD= #insert the system code/password Huawei have issued
      - HUAWEI__STATIONNAME= #insert the plant/station name here
      - HUAWEI__POLLRATE=5 #in minutes (Huawei have a limit of once every 5 minutes)
      - HUAWEI__SENDGRIDVALUES= #insert boolean value of if you should send the grid values, defaults to true
      - CHARGEHQ__APIKEY= #insert your ChargeHQ API Key here
      - SERILOG__MINIMUMLEVEL=Information # Use Debug if you want to see information on message payloads
    volumes:
     - huaweisolar-conf:/etc/huaweisolar

volumes:
  huaweisolar-conf:

```

Run the following command to run the service:

```bash
docker-compose up -d
```

If you want to watch the logs run the command:

```bash
docker-compose logs --follow huawei-solar
```

To stop the service run:

```bash
docker-compose stop huawei-solar
```

You can configure the entire service using environment variables as show above but if you want to use a configuration file you can put an **appsettings.json** in the mapped volume and it will take effect, environment variables will override any equivalent settings:

```json
{
    "Huawei": {
        "BaseURI": "https://intl.fusionsolar.huawei.com/",
        "Username":"<Username Here>",
        "Password": "<System Code Here>",
        "StationName": "<Station Name Here>",
        "PollRate": 5,
        "SendGridValues": true
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
