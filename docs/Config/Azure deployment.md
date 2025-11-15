Go to AppHost project directory:
`cd MentorSync/aspire/MentorSync.AppHost`
Init project for azure infra:
`azd init`
First of all, login in azure:
`azd auth login`
Then configure CD pipeline:
`azd pipeline config`
_It will also ask to configure env variables (if there are any) which will be stored in .azure/mentorsync-dev/config.json_
_set up it in GitHub Actions too in the same format as in config.json. Example:_

```
{
  "infra": {
    "parameters": {
      "appConfig": "vault://.../...",
      "keyVault": "vault://.../...",
      "landingZoneStorageAccount": "vault://.../...",
      "serviceBusApiSend": "vault://.../...",
      "serviceBusRuntimeListen": "vault://.../..."
    }
  }
```

Provision and deploy from local (temporary CD workflow does not work):
`azd provision --debug`
`azd deploy`

Clean up resources:
`azd down`

### Generate Aspire manifest

`dotnet run --publisher manifest --output-path aspire-manifest.json`

## Bicep bonus

Enable synth (do this once):
`azd config set alpha.infraSynth on`
Infra synth:
`azd infra synth`
check changes in new 'infra' folder
[[Deploy via Bicep]]
