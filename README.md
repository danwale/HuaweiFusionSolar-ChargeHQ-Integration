# Huawei Fusion Solar to ChargeHQ Push API Integration Module

[![Docker Image CI](https://github.com/danwale/HuaweiFusionSolar-ChargeHQ-Integration/actions/workflows/docker-image.yml/badge.svg?branch=main)](https://github.com/danwale/HuaweiFusionSolar-ChargeHQ-Integration/actions/workflows/docker-image.yml)

[![Donate PayPal](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://paypal.me/danwale?country.x=AU&locale.x=en_AU) <a href="https://www.buymeacoffee.com/danwale" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-blue.png" alt="Buy Me A Coffee" style="height: 20px !important;width: 105px !important;" ></a>

The intention of this integration is to poll the Huawei Fusion Solar API to determine how much solar power is being generated and provide this to ChargeHQ's Push API that will enable ChargeHQ to regulate the charge rate of a Tesla car to maximise the use of solar energy rather than grid supplied power.

This service is only needed while ChargeHQ doesn't support Huawei inverters, it's on the roadmap for their service so one day this might not be needed.

**Of course if you really like this service and want to thank me a donation would be much appreciated, see the various donation method buttons above.**

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
        "PollRate": 5
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
